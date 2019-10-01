using System;
using System.Collections.Generic;
using System.Text;
using NServiceBus;

namespace Shared.Configuration
{
    public static class Configuration
    {
        public static EndpointConfiguration ConfigureEndpoint(string endpointName)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UsePersistence<LearningPersistence>();

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
            conventions.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"));
            conventions.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));

            // Don't do retries at all for this demo.
            endpointConfiguration.Recoverability().Immediate(s => s.NumberOfRetries(0));
            endpointConfiguration.Recoverability().Delayed(s => s.NumberOfRetries(0));

            return endpointConfiguration;
        }
    }
}
