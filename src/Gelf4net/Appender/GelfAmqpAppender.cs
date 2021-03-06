﻿using log4net.Appender;
using log4net.Util;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace gelf4net.Appender
{
    public class GelfAmqpAppender : AppenderSkeleton
    {
        public GelfAmqpAppender()
        {
            Encoding = Encoding.UTF8;
        }

        protected ConnectionFactory ConnectionFactory { get; set; }
        public string RemoteAddress { get; set; }
        public int RemotePort { get; set; }
        public string Exchange { get; set; }
        public string Key { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Encoding Encoding { get; set; }

        public override void ActivateOptions()
        {
            base.ActivateOptions();

            InitializeConnectionFactory();
        }

        protected virtual void InitializeConnectionFactory()
        {
            ConnectionFactory = new ConnectionFactory()
            {
                Protocol = Protocols.DefaultProtocol,
                HostName = RemoteAddress,
                Port = RemotePort,
                VirtualHost = VirtualHost,
                UserName = Username,
                Password = Password
            };
        }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            var message = RenderLoggingEvent(loggingEvent).GzipMessage(Encoding);

            using (IConnection conn = ConnectionFactory.CreateConnection())
            {
                var model = conn.CreateModel();
                byte[] messageBodyBytes = message;
                model.BasicPublish(Exchange, Key, null, messageBodyBytes);
            }
        }
    }
}
