using PumpClient.PumpServiceReference;
using System;

namespace PumpClient
{
    public class CallBackHandler : IPumpServiceCallback
    {
        public void UpadateStatistic(StatisticService statistic)
        {
            Console.Clear();
            Console.WriteLine($"Script execution statistic\n" +
                $"All tacts:\t\t{statistic.AllTacts}\n" +
                $"Seccess tacts:\t{statistic.SuccessTacts}\n" +
                $"Error tacts:\t{statistic.ErrorTacts}");

        }
    }
}
