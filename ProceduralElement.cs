using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ChemWriter
{
    internal class ProceduralElement : Element
    {
        private static List<char> Chars = new List<char> { 'n', 'u', 'b', 't', 'q', 'p', 'h', 's', 'o', 'e' };
        private static List<string> Names = new List<string> { "nil", "un", "bi", "tri", "quad", "pent", "hex", "sept", "oct", "enn" };

        public ProceduralElement(string sym)
        {
            AtomicNumber = DeriveNumber(sym);
            Symbol = Char.ToUpper(sym[0]) + sym[1..];
            Name = DeriveName(AtomicNumber);
        }

        public ProceduralElement(UInt64 num)
        {
            AtomicNumber = num;
            Symbol = DeriveSymbol(num);
            Name = DeriveName(num);
        }

        private static string DeriveSymbol(UInt64 num)
        {
            var numStr = num.ToString();
            var sym = "";

            foreach (var c in numStr)
            {
                sym += Chars[int.Parse($"{c}")];
            }

            return Char.ToUpper(sym[0]) + sym[1..];
        }

        private string DeriveName(UInt64 num)
        {
            var name = "";
            var numStr = num.ToString();

            for (var i = 0; i < numStr.Length; i++)
            {
                var dig = int.Parse(numStr[i].ToString());

                if (i == 0)
                {
                    var digName = Char.ToUpper(Names[dig][0]) + Names[dig][1..];
                    name += digName;
                    continue;
                }

                if (dig == 0 && numStr[i-1] == '9')
                {
                    name += "il";
                    continue;
                }

                name += Names[dig];
            }

            if (numStr.Last() == '2' || numStr.Last() == '3')
            {
                name += "um";
            } else
            {
                name += "ium";
            }

            return name;
        }

        public static UInt64 DeriveNumber(string sym)
        {
            UInt64 num = 0;

            for (var i = 0; i < sym.Length; i++)
            {
                num = (UInt64)Chars.IndexOf(sym[i]) + num * 10;
            }

            return num;
        }

        // Returns a list of all procedural elements with symbol "p" that satisfy input.StartsWith(p),
        // except ones with leading zeros
        public static List<ProceduralElement> GetOptions(string input, Config config)
        {
            var options = new List<ProceduralElement>();

            if (input.StartsWith("n"))
            {
                ReportWriter.Debug($"Declining to generate systematic names for input {input} because it has a leading zero.");
                return options;
            }

            var attempt = "";

            foreach (var c in input)
            {
                if (Chars.Contains(c))
                {
                    attempt += c;
                    AddOption(options, new ProceduralElement(attempt), config);
                }
                else break;
            }

            options.Reverse();
            return options.Where(e => !e.HasRedundantSymbol()).ToList();
        }

        private static ProceduralElement AugmentProceduralElement(char c, ProceduralElement prev)
        {
            return new ProceduralElement(prev.AtomicNumber * (UInt64)10 + (UInt64)Chars.IndexOf(c));
        }

        public static List<Element> MinimizeProceduralElements(List<Element> ans)
        {
            return ans;
        }

        private static void AddOption(List<ProceduralElement> options, ProceduralElement e, Config config)
        {
            var i = e.AtomicNumber;

            if (InRange(i, config.minSystematic, config.maxSystematic))
            {
                options.Add(e);
            }
            
        }

        private static bool InRange(UInt64 i, UInt64 min, UInt64 max)
        {
            return min <= i && i <= max;
        }

        private bool HasRedundantSymbol()
        {
            return Program.Do(Symbol, new Config(0, 0), new(), new()).isComplete;
        }
    }
}
