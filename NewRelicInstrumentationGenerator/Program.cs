using System.IO;

namespace NewRelicInstrumentationGenerator
{
    public class Program
    {
        static void Main()
        {
            GenerateZeroMqRequestHandlerInstrumentationXml();
            GenerateServiceBusMessageHandlerInstrumentationXml();
        }
        
        private static void GenerateZeroMqRequestHandlerInstrumentationXml()
        {
            var instrumentionFactory = new ZeroMqRequestHandlerInstrumentation(@"C:\Dev\Auth\src\IQ.Auth.OAuth2.ZeroMQServer\RequestHandlers", "BaseRequestHandler");
            var content = instrumentionFactory.Generate();
            File.WriteAllText(@"C:\Dev\Auth\src\IQ.Auth.OAuth2.ZeroMQServer\CustomInstrumentation.xml", content);
        }
        
        private static void GenerateServiceBusMessageHandlerInstrumentationXml()
        {
            var instrumentionFactory = new ServiceBusMessageHandlerInstrumentation(@"C:\Dev\Auth\src\IQ.Auth.OAuth2.ServiceBus", "BaseMessageHandler");
            var content = instrumentionFactory.Generate();
            File.WriteAllText(@"C:\Dev\Auth\src\IQ.Auth.OAuth2.Web\newrelic\extensions\BusMessageHandlerInstrumentation.xml", content);
        }
    }
}
