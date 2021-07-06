using System;
using LHRP.Api.Runtime.Scheduling;

namespace LHRP.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var scheduleStream = new ConsolePrintScheduleStream();

            var examples = new AddReagentExample(scheduleStream);

            examples.RunExample();

            Console.WriteLine("Done... Hit enter key to continue.");
            Console.ReadLine();
        }

    }
}
