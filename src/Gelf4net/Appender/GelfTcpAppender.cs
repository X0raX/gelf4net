namespace gelf4net.Appender
{
    using System.Net.Sockets;
    using System.Text;

    using log4net.Appender;
    using log4net.Core;

    public class GelfTcpAppender : AppenderSkeleton
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public Encoding Encoding { get; set; }
        
        public GelfTcpAppender()
        {
            Encoding = Encoding.UTF8;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = RenderLoggingEvent(loggingEvent).GzipMessage(Encoding);
            //var message = Encoding.GetBytes(loggingEvent.RenderedMessage + System.Environment.NewLine);

            using (TcpClient tcpClient = new TcpClient(Hostname, Port))
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(message, 0, message.Length);
                tcpClient.Close();
            }
        }
    }
}