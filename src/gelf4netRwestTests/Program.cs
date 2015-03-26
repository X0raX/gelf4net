using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace gelf4netRwestTests
{
    using System.Reflection;

    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var random = new Random();
            var message1 = string.Format("RandomNumberInfo:{0}", random.Next(1000));
            var message2 = string.Format("Hello SMART.ETL Team, It's Tuesday - RandomNumberInfo:{0}", random.Next(1000));
            var message3 = string.Format("RandomNumberInfo:{0}\r\n{1}", random.Next(1000), Environment.StackTrace);
            Log.Debug(message1);
            Log.Info(message2);
            Log.Error(message3);
            Console.ReadLine();
        }
    }
}
