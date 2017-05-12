namespace NewRelicInstrumentationGenerator
{
    public class ZeroMqRequestHandlerInstrumentation : NewRelicInstrumentationBase
    {
        private const string Bit = @"    <tracerFactory name=""NewRelic.Agent.Core.Tracer.Factories.BackgroundThreadTracerFactory"" metricName=""ZeroMQServer/V1_{0}"" transactionNamingPriority=""7"">
      <match assemblyName=""IQ.Auth.OAuth2.ZeroMQServer"" className=""IQ.Auth.OAuth2.ZeroMQServer.RequestHandlers.{1}"">
        <exactMethodMatcher methodName=""Handle"" parameters=""IQ.Auth.OAuth2.ProtectedResource.ZeroMQ.MessagingContracts.V1.{2}"" />
      </match>
    </tracerFactory>";

        public ZeroMqRequestHandlerInstrumentation(string baseFolder, string baseClassIdentifier) 
            : base(baseFolder, baseClassIdentifier)
        {
        }

        private string Sanitize(string input)
        {
            var replaceableTerms = new[] { "RequestHandler" };

            foreach (var term in replaceableTerms)
            {
                if (input.EndsWith(term))
                {
                    input = input.Substring(0, input.Length - term.Length);
                }
            }
            return input;
        }

        protected override string GenerateTracerFactoryXml((string, string) tuple)
        {
            return string.Format(Bit, Sanitize(tuple.Item1), tuple.Item1, tuple.Item2);
        }
    }
}
