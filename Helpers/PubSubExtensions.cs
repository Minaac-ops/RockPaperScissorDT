using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Internals;
using Events;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace Helpers
{
    public static class PubSubExtensions
    {
        public static readonly TextMapPropagator Propagator = new TraceContextPropagator();

        public static Task PublishWithTracingAsync<T>(this IPubSub con, T message) where T : TracingEventBase
    {        
        using var activity = Monitoring.ActivitySource.StartActivity(ActivityKind.Producer);
        var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
        
        var propagationContext = new PropagationContext(activityContext, Baggage.Current);
        Propagators.DefaultTextMapPropagator.Inject(propagationContext, message, (msg, key, value) =>
        {
            msg.Headers[key] = value;
        });
        return con.PublishAsync(message);
    }

    public static AwaitableDisposable<SubscriptionResult> SubscribeWithTracingAsync<T>(this IPubSub con, string subscriptionId, Action<T> onMessage) where T : TracingEventBase
    {
        return con.SubscribeAsync(subscriptionId, (T message) =>
        {
            var parentContext = Propagator.Extract(default, message, (msg, key) =>
            {
                if (message.Headers.TryGetValue(key, out var value))
                {
                    return new[] { value.ToString() };
                }

                return Enumerable.Empty<string>();
            });
            
            using var activity = Monitoring.ActivitySource.StartActivity("Received message", ActivityKind.Consumer, parentContext.ActivityContext);
            onMessage(message);
        });
    }
    }
}