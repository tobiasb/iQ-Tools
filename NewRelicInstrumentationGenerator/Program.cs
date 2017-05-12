using System.IO;
using System.Linq;

namespace NewRelicInstrumentationGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var generateZmqHandlerInstrumentationXml = false;
            var generateMessageHandlerInstrumentationXml = false;

            if (args.Length == 0 || args.Contains("zmq")) generateZmqHandlerInstrumentationXml = true;
            if (args.Length == 0 || args.Contains("asb")) generateMessageHandlerInstrumentationXml = true;

            if (generateZmqHandlerInstrumentationXml)
            {
                GenerateZeroMqRequestHandlerInstrumentationXml();
            }

            if (generateMessageHandlerInstrumentationXml)
            {
                GenerateServiceBusMessageHandlerInstrumentationXml();
            }
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
