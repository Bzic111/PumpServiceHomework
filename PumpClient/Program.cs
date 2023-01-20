using PumpClient.PumpServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PumpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InstanceContext context = new InstanceContext(new CallBackHandler());
            PumpServiceClient client = new PumpServiceClient(context);

            client.UpdateAndCompile(@"d:\Scripts\Sample.script");
            client.RunScript();
            Console.WriteLine("Working...");
            Console.ReadKey(false);
            client.Close();
        }
    }
}
