using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Events;
using Helpers;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace CopyPlayerService;

public static class Program
{
    private static readonly IPlayer Player = new CopyPlayer();
    
    public static async Task Main()
    {
        var connectionEstablished = false;

        while (!connectionEstablished)
        {
            var bus = ConnectionHelper.GetRMQConnection();
            var subscriptionResult = bus.PubSub.SubscribeAsync<GameStartedEvent>("RPS", e =>
            {
                var moveEvent = Player.MakeMove();
                bus.
                var propagator = new TraceContextPropagator();
                var parentCtx = propagator.Extract(default, e, (r, key) =>
                {
                    return new List<string>(new[] { r.Header.ContainsKey(key) ? r.Header[key].ToString() : string.Empty} );
                });
                Baggage.Current = parentCtx.Baggage;
                using var activity = Monitoring.ActivitySource.StartActivity("Message received", ActivityKind.Consumer, parentCtx.ActivityContext);

                
                var moveEvent = Player.MakeMove(e);
                bus.PubSub.PublishAsync(moveEvent);
            }).AsTask();

            await subscriptionResult.WaitAsync(CancellationToken.None);
            connectionEstablished = subscriptionResult.Status == TaskStatus.RanToCompletion;
            if(!connectionEstablished) Thread.Sleep(1000);
        }

        while (true) Thread.Sleep(5000);
    }
}