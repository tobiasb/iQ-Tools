namespace NewRelicInstrumentationGenerator
{
    public class ServiceBusMessageHandlerInstrumentation : NewRelicInstrumentationBase
    {
        private const string Bit = @"    <tracerFactory name=""NewRelic.Agent.Core.Tracer.Factories.BackgroundThreadTracerFactory"" metricName=""Accounts/{0}"">
      <match assemblyName=""IQ.Auth.OAuth2.ServiceBus"" className=""IQ.Auth.OAuth2.ServiceBus.{1}"">
        <exactMethodMatcher methodName=""Handle"" />
      </match>
    </tracerFactory>";

        public ServiceBusMessageHandlerInstrumentation(string baseFolder, string baseClassIdentifier) 
            : base(baseFolder, baseClassIdentifier)
        {
        }

        protected override string GenerateTracerFactoryXml((string, string) tuple)
        {
            return string.Format(Bit, tuple.Item1, tuple.Item1);
        }
    }
}
