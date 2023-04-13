using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Events;
using Helpers;
using Monolith;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

public class Program
{
    private static Game game = new Game();

    public static async Task Main()
    {
        using var activity = Monitoring.ActivitySource.StartActivity();
        var connectionEstablished = false;

        using var bus = ConnectionHelper.GetRMQConnection();
        while (!connectionEstablished)
        {
            var subscriptionResult = bus.PubSub
                .SubscribeAsync<PlayerMovedEvent>("RPS", e =>
                {
                     var finishedEvent = game.ReceivePlayerEvent(e);
                     if (finishedEvent != null)
                     {
                         bus.PubSub.PublishAsync(finishedEvent);
                     }
                })
                .AsTask();

            await subscriptionResult.WaitAsync(CancellationToken.None);
            connectionEstablished = subscriptionResult.Status == TaskStatus.RanToCompletion;
            if (!connectionEstablished) Thread.Sleep(1000);
        }

        await bus.PubSub.PublishAsync(game.Start());
        
        while (true) Thread.Sleep(5000);
    }
}