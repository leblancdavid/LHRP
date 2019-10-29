using LHRP.Api.Runtime;
using LHRP.Instrument.SimplePipettor.Runtime;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.IO;

namespace LHRP.Scripting.TestConsole
{
    public class ScriptHost
    {
        public IRuntimeEngine SimulationEngine { get; set; }
        public ProtocolScript Protocol { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {

            var script = File.ReadAllText("MySimpleProtocol.csx");

            //note: we block here, because we are in Main method, normally we could await as scripting APIs are async
            var result = CSharpScript.EvaluateAsync<int>(script, null,
                new ScriptHost
                {
                    SimulationEngine = new SimplePipettorSimulationEngine(),
                    Protocol = new ProtocolScript()
                }).Result;

            Console.ReadLine();
        }
    }
}
