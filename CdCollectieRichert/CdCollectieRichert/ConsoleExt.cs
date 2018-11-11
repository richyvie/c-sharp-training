using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdCollectieRichert
{
    public static class ConsoleExt
    {
        public static ConsoleColor KleurInfo { get; set; } 
            = ConsoleColor.Cyan;
        public static ConsoleColor KleurError { get; set; }
            = ConsoleColor.Red;
        public static ConsoleColor KleurSucces { get; set; }
            = ConsoleColor.Green;
        public static ConsoleColor KleurWarning { get; set; }
            = ConsoleColor.Yellow;

        public static void WriteLineInfo(string boodschap)
        {
            var oudekleur = Console.ForegroundColor;
            Console.ForegroundColor = KleurInfo;
            Console.WriteLine(boodschap);
            Console.ForegroundColor = oudekleur;
        }

        public static void WriteLineError(string boodschap)
        {
            var oudekleur = Console.ForegroundColor;
            Console.ForegroundColor = KleurError;
            Console.WriteLine(boodschap);
            Console.ForegroundColor = oudekleur;
        }

        public static void WriteLineSucces(string boodschap)
        {
            var oudekleur = Console.ForegroundColor;
            Console.ForegroundColor = KleurSucces;
            Console.WriteLine(boodschap);
            Console.ForegroundColor = oudekleur;
        }

        public static void WriteLineWarning(string boodschap)
        {
            var oudekleur = Console.ForegroundColor;
            Console.ForegroundColor = KleurWarning;
            Console.WriteLine(boodschap);
            Console.ForegroundColor = oudekleur;
        }

        public static void WriteTitle(string boodschap)
        {
            int LengteBoodschap = boodschap.Length;
            string Lijn = new string('*', LengteBoodschap+4);
            Console.WriteLine(Lijn);
            Console.WriteLine("* {0} *", boodschap);
            Console.WriteLine(Lijn);
        }
    }
}
