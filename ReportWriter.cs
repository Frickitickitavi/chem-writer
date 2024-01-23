using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemWriter
{
    internal static class ReportWriter
    {
        private const ConsoleColor REAL = ConsoleColor.Green;
        private const ConsoleColor ATTEMPTED = ConsoleColor.DarkBlue;
        private const ConsoleColor THEORETICAL = ConsoleColor.Blue;
        private const ConsoleColor INFINITE = ConsoleColor.Cyan;
        private const ConsoleColor OLD = ConsoleColor.Yellow;
        private const ConsoleColor TRIVIAL = ConsoleColor.DarkYellow;
        private const ConsoleColor REDUNDANT = ConsoleColor.Red;
        private const ConsoleColor DEFAULT = ConsoleColor.White;

        private const ConsoleColor SUCCESS = ConsoleColor.DarkGreen;
        private const ConsoleColor PARTIAL = ConsoleColor.DarkYellow;
        private const ConsoleColor FAILURE = ConsoleColor.DarkRed;

        private const ConsoleColor DEBUG = ConsoleColor.Magenta;

        public static void PrintSuccess(List<List<Element>> answers)
        {
            PrintAnswers(answers);
            SetColor(SUCCESS);
            Print("");
            Print("################################################################");
            Print("###                                                          ###");
            Print("###  SUCCESS! CREATED THE ENTIRE STRING IN AT LEAST ONE WAY  ###");
            Print("###                                                          ###");
            Print("################################################################");
            Print("");
            Print($"Total Answers: {answers.Count}");
            SetColor(DEFAULT);
        }

        public static void PrintPartial(List<List<Element>> answers, string orig)
        {
            PrintAnswers(answers);

            SetColor(PARTIAL);
            Print("");
            Print("################################################################");
            Print("###                                                          ###");
            Print("###     FAILURE. LISTING SEQUENCES THAT GOT THE FURTHEST     ###");
            Print("###                                                          ###");
            Print("################################################################");
            Print("");

            var inLen = orig.Length;
            var outLen = answers.First().Sum(e => e.Symbol.Length);

            Print($"Input Length:  {inLen} characters");
            Print($"Output Length: {outLen} characters ({orig[..outLen]})");
            Print($"Percentage:    {Decimal.Divide(outLen, inLen) * 100}%");
            Print($"Total Answers: {answers.Count}");

            SetColor(DEFAULT);
        }

        public static void PrintFailure()
        {
            SetColor(FAILURE);
            Print("#####################################################################");
            Print("###                                                               ###");
            Print("###  COMPLETE FAILURE. UNABLE TO SEQUENCE ANY PART OF THE STRING  ###");
            Print("###                                                               ###");
            Print("#####################################################################");
            SetColor(DEFAULT);
        }

        public static void Debug(string message)
        {
            if (Config.DEBUG)
            {
                var oldColor = Console.ForegroundColor;
                SetColor(DEBUG);
                Print(message);
                SetColor(oldColor);
            }
        }

        public static void PrintBadChars(IEnumerable<char> badChars)
        {
            SetColor(FAILURE);
            PrintNoNewline($"FATAL - Invalid character{(badChars.Count() == 1 ? "" : "s")} found in input string: ");
            foreach (var c in badChars)
            {
                PrintNoNewline(c.ToString());
            }
            Print("\n\n");
            SetColor(DEFAULT);
        }

        private static void Print(string message)
        {
            Console.WriteLine(message);
        }

        private static void PrintNoNewline(string message)
        {
            Console.Write(message);
        }

        private static void PrintAnswers(List<List<Element>> answers)
        {
            var conds = new List<Func<Element, bool>>() { e => e is RealElement };
            var remaining = PrintAnswerSet(
                answers, 
                conds, 
                "SOLUTIONS WITH ONLY REAL ELEMENTS", 
                REAL
            );

            conds.Add(e => Config.MAX_REAL < e.AtomicNumber && e.AtomicNumber <= Config.MAX_ATTEMPTED);
            remaining = PrintAnswerSet(
                remaining,
                conds,
                $"IF WE ALSO ALLOW LOW-END THEORETICAL ELEMENTS ({Config.MAX_REAL+1}-{Config.MAX_ATTEMPTED})",
                ATTEMPTED,
                "*"
            );

            conds.Add(e => Config.MAX_ATTEMPTED < e.AtomicNumber && e.AtomicNumber <= Config.MAX_THEORETICAL);
            remaining = PrintAnswerSet(
                remaining,
                conds,
                $"IF WE ALSO ALLOW MID-RANGE THEORETICAL ELEMENTS ({Config.MAX_ATTEMPTED+1}-{Config.MAX_THEORETICAL})",
                THEORETICAL,
                "**"
            );

            conds.Add(e => Config.MAX_THEORETICAL < e.AtomicNumber);
            remaining = PrintAnswerSet(
                remaining,
                conds,
                $"IF WE ALSO ALLOW THEORETICAL ELEMENTS ABOVE {Config.MAX_THEORETICAL}",
                INFINITE
            );

            conds.Add(e => Config.MIN_OLD_PROCEDURAL < e.AtomicNumber);
            remaining = PrintAnswerSet(
                remaining,
                conds,
                $"IF WE ALSO ALLOW OLD SYSTEMATIC NAMES OF ELEMENTS {Config.MIN_OLD_PROCEDURAL}-{Config.MAX_REAL}",
                OLD,
                "***"
            );

            conds.Add(e => Config.MIN_OLD_TRIVIAL < e.AtomicNumber);
            remaining = PrintAnswerSet(
                remaining,
                conds,
                $"IF WE ALSO ALLOW REDUNDANT SYSTEMATIC NAMES FOR ELEMENTS {Config.MIN_OLD_TRIVIAL}-{Config.MIN_OLD_PROCEDURAL-1}",
                TRIVIAL,
                "****"
            );

            remaining = PrintAnswerSet(
                remaining,
                new() { e => true },
                $"IF WE ALSO ALLOW REDUNDANT SYSTEMATIC NAMES FOR ALL ELEMENTS",
                REDUNDANT
            );

            PrintKey();
        }

        private static void PrintKey()
        {
            SetColor(DEFAULT);
            Print("\n\nCOLOR KEY:");
            SetColor(REAL);
            Print($"GREEN ------- Properly-named elements (1-{Config.MAX_REAL})");
            SetColor(ATTEMPTED);
            Console.Write($"DARK BLUE --- Undiscovered elements with relatively low atomic numbers ({Config.MIN_THEORETICAL}-{Config.MAX_ATTEMPTED})");
            SetColor(DEFAULT);
            Print("*");
            SetColor(THEORETICAL);
            Console.Write($"BLUE -------- Undiscovered elements with moderately high atomic numbers ({Config.MAX_ATTEMPTED+1}-{Config.MAX_THEORETICAL})");
            SetColor(DEFAULT);
            Print("**");
            SetColor(INFINITE);
            Print($"CYAN -------- Undiscovered elements with very high atomic numbers (>{Config.MAX_THEORETICAL})");
            SetColor(OLD);
            Console.Write($"YELLOW ------ Old systematic names for elements that have since been properly named ({Config.MIN_OLD_PROCEDURAL}-{Config.MAX_REAL})");
            SetColor(DEFAULT);
            Print("***");
            SetColor(TRIVIAL);
            Console.Write($"ORANGE ------ Systematic names introduced as alternatives for then-recently-named elements ({Config.MIN_OLD_TRIVIAL}-{Config.MIN_OLD_PROCEDURAL-1})");
            SetColor(DEFAULT);
            Print("****");
            SetColor(REDUNDANT);
            Print($"RED --------- Systematic names for elements that have always had proper names (1-{Config.MIN_OLD_TRIVIAL-1})");
            SetColor(DEFAULT);
            Print("\n\nFOOTNOTES:");
            Print($"*    {Config.MAX_ATTEMPTED} is used as the upper bound for the low range of undiscovered elements because it is the heaviest element for which synthesis has been attempted.\n");
            Print($"**   {Config.MAX_THEORETICAL} is used as the upper bound for the mid range of undiscovered elements because it is a common, though unproven, hypothesis for the theoretical limit of the periodic table.\n");
            Print($"***  {Config.MIN_OLD_PROCEDURAL}-{Config.MAX_REAL} is used as the upper range for redundant systematic names because these elements did not originally have proper names and were exclusively referred to by their systematic names.\n");
            Print($"**** {Config.MIN_OLD_TRIVIAL}-{Config.MIN_OLD_PROCEDURAL-1} is used as the middle range for redundant systematic names because these elements were newly-named when systematic names were introduced and their systematic names were offered as alternatives.\n");
        }

        private static void SetColor(ConsoleColor c)
        {
            Console.ForegroundColor = c;
        }

        private static ConsoleColor GetColor(Element e)
        {
            if (e is RealElement)
            {
                return REAL;
            }

            if (e is ProceduralElement)
            {
                var n = e.AtomicNumber;
                if (Config.MAX_REAL < n && n <= Config.MAX_ATTEMPTED)
                {
                    return ATTEMPTED;
                }

                if (Config.MAX_ATTEMPTED < n && n <= Config.MAX_THEORETICAL)
                {
                    return THEORETICAL;
                }

                if (Config.MAX_THEORETICAL < n)
                {
                    return INFINITE;
                }

                if (Config.MIN_OLD_PROCEDURAL <= n && n <= Config.MAX_REAL)
                {
                    return OLD;
                }

                if (Config.MIN_OLD_TRIVIAL <= n && n < Config.MIN_OLD_PROCEDURAL)
                {
                    return TRIVIAL;
                }

                if (n < Config.MIN_OLD_TRIVIAL)
                {
                    return REDUNDANT;
                }
            }

            Console.ForegroundColor = DEFAULT;
            throw new InvalidDataException($"Element {e} in unaccounted state");
        }

        private static List<List<Element>> PrintAnswerSet(List<List<Element>> ans, List<Func<Element, bool>> conds, string intro, ConsoleColor color)
        {
            return PrintAnswerSet(ans, conds, intro, color, "");
        }

        private static List<List<Element>> PrintAnswerSet(List<List<Element>> ans, List<Func<Element, bool>> conds, string intro, ConsoleColor color, string footnote)
        {
            SetColor(color);
            Console.Write($"\n{intro}");
            SetColor(DEFAULT);
            Console.Write(footnote);
            SetColor(color);
            Print(":");

            var matchingAns = ans.Where(an => AnswerMeetsConds(an, conds));

            if (matchingAns.Count() == 0) {
                SetColor(DEFAULT);
                Print("None.");
                return ans;
            }

            
            foreach (var a in matchingAns)
            {
                PrintAnswer(a);
            }

            return ans.Where(an => an.Any(el => !AnswerMeetsConds(an, conds))).ToList();
        }

        private static bool AnswerMeetsConds(List<Element> an, List<Func<Element, bool>> conds)
        {
            foreach (var el in an)
            {
                if (conds.TrueForAll(cond => !cond(el)))
                {
                    return false;
                }
            }
            return true;
        }

        private static void PrintAnswer(List<Element> a)
        {
            foreach (var e in a)
            {
                Console.ForegroundColor = GetColor(e);
                Console.Write($"{e.Symbol}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" - ");
            for (var idx = 0; idx < a.Count; idx++)
            {
                var e = a[idx];

                Console.ForegroundColor = GetColor(e);
                Console.Write($"{e.Name}");

                if (e is ProceduralElement && e.AtomicNumber <= Config.MAX_REAL)
                {
                    Console.Write($" (aka {RealElement.Table.Find(r => e.AtomicNumber == r.AtomicNumber)})");
                }

                if (idx != a.Count - 1)
                {
                    SetColor(DEFAULT);
                    Console.Write(", ");
                }
            }
            Print("");
        }
    }
}
