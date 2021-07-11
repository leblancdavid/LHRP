using System;
using LHRP.Api.Runtime.Scheduling;

namespace LHRP.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var scheduleStream = new ConsolePrintScheduleStream();

            //var example = new AddReagentExample(scheduleStream);
            var example = new TransferSamplesExample(scheduleStream);
            example.RunExample();

            Console.WriteLine("Done... Hit enter key to continue.");
            Console.ReadLine();
        }

    }
}
