namespace gelf4net.Appender
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    using log4net.Appender;
    using log4net.Core;

    public class MyGelfTcpAppender : AppenderSkeleton, IDisposable
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public bool KeepConnectionAlive { get; set; }
        public Encoding Encoding { get; set; }

        private TcpClient TcpClient { get; set; }

        public MyGelfTcpAppender()
        {
            Encoding = Encoding.UTF8;
        }

        public void Dispose()
        {
            if (TcpClient != null)
            {
                if (TcpClient.Connected)
                {
                    TcpClient.Close();
                }
                TcpClient.Client.Dispose();
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            byte[] message = RenderLoggingEvent(loggingEvent).GzipMessage(Encoding);

            if (KeepConnectionAlive)
            {
                Connect();
                SendMessage(message);
            }
            else
            {
                using (TcpClient = new TcpClient(Hostname, Port))
                {
                    SendMessage(message);
                    TcpClient.Close();
                }
            }
        }

        private void Connect()
        {
            if (TcpClient == null || !TcpClient.Connected)
            {
                TcpClient = new TcpClient(Hostname, Port);
            }
        }

        private void SendMessage(byte[] message)
        {
            NetworkStream stream = TcpClient.GetStream();
            stream.Write(message, 0, message.Length);
        }
    }
}