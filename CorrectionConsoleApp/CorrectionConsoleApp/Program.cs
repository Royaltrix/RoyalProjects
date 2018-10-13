using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrectionConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Execute();
        }
        public static void Execute()
        {
            SpellCorrector spellCorrector = new SpellCorrector();
            Console.Write("Ingrese una palabra: ");

            var word = Console.ReadLine();
            List<string> candidates = spellCorrector.Candidates(word);

            candidates = candidates.Skip(Math.Max(0, candidates.Count() - 3)).ToList();
            Console.WriteLine("Posibles Correcciones: ");
            foreach (string item in candidates)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}
