using System;

namespace LHRP.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var example = new AddReagentExample(scheduleStream);
            var example = new TransferSamplesExample2();
            example.RunExample();

            Console.WriteLine("Done... Hit enter key to continue.");
            Console.ReadLine();
        }

    }
}
