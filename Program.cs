using ChemWriter;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace ChemWriter {
    internal class Program
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        
        public static void Main()
        {
            Console.Write("Enter a phrase (/q to quit): ");

            while (true)
            {

                var raw = Console.ReadLine();

                if (raw == "/q") return;

                Console.Write(
                    "How would you like the script to handle systematic element names?\n" +
                    "\n" +
                    "0 - Do not use any systematic names\n" +
                    $"1 - Allow undiscovered systematic elements that have had synthesis attempts ({Config.MAX_REAL + 1}-{Config.MAX_ATTEMPTED})\n" +
                    $"2 - Allow discovered systematic elements under the theoretical maximum of {Config.MAX_THEORETICAL} ({Config.MAX_REAL + 1}-{Config.MAX_THEORETICAL})\n" +
                    $"3 - Allow any undiscovered systematic element ({Config.MAX_REAL + 1}+)\n" +
                    $"4 - Allow any undiscovered systematic element, and old systematic names for elements that have since been properly named ({Config.MIN_OLD_PROCEDURAL}+)\n" +
                    $"5 - Allow any undiscovered systematic element, and any systematic names that have ever been formally used, even if only as alternatives for already-named elements ({Config.MIN_OLD_TRIVIAL}+)\n" +
                    $"6 - Allow any systematic element name whatsoever, even if completely redundant with a named element (1+)\n" +
                    $"7 - Specify a custom range of acceptable systematic elements (not yet implemented)\n" +
                    $"\n" +
                    $"Enter the number corresponding to your choice (default 6): "
                );
                Config? config = null;

                while (config == null)
                {
                    var lenience = Console.ReadLine();
                    config = lenience switch
                    {
                        "0" => new(0, 0),
                        "1" => new(Config.MAX_REAL + 1, Config.MAX_ATTEMPTED),
                        "2" => new(Config.MAX_REAL + 1, Config.MAX_THEORETICAL),
                        "3" => new(Config.MAX_REAL + 1, UInt64.MaxValue),
                        "4" => new(Config.MIN_OLD_PROCEDURAL, UInt64.MaxValue),
                        "5" => new(Config.MIN_OLD_TRIVIAL, UInt64.MaxValue),
                        "6" or "" => new(1, UInt64.MaxValue),
                        _ => config
                    };

                    if (lenience == "7")
                    {
                        throw new NotImplementedException();
                    }

                    if (lenience == null)
                    {
                        Console.Write("Invalid input. Try again: ");
                    }
                }

                var completeAnswers = new List<List<Element>>();
                var bestAnswers = new List<List<Element>>();

                var result = Do(raw, config, completeAnswers, bestAnswers);

                if (result.isComplete)
                {
                    ReportWriter.PrintSuccess(result.answers);
                }
                else if (result.answers.Count() > 0)
                {
                    ReportWriter.PrintPartial(result.answers, result.input);
                }
                else
                {
                    ReportWriter.PrintFailure();
                }

                Console.Write("\nIf you would like to go again, enter another phrase (/q to quit): ");
            }
        }

        public static Result Do(string raw, Config config, List<List<Element>> completeAnswers, List<List<Element>> bestAnswers)
        {
            var input = SanitizeInput(raw);

            GetAnswer(input, config, new(), completeAnswers, bestAnswers);

            if (completeAnswers.Count > 0)
            {
                return new Result() { answers = completeAnswers, isComplete = true };
                
            }
            else if (bestAnswers.Count > 0)
            {
                return new Result() { answers = bestAnswers, isComplete = false, input = input };
            }
            else
            {
                return new Result() { answers = bestAnswers, isComplete = false };
            }
        }

        private static string SanitizeInput(string raw)
        {
            var lower = raw.ToLower();

            foreach (var c in lower.Where(c => !alphabet.Contains(c)))
            {
                ReportWriter.Debug($"Removing nonalphabetical character \"{c}\" from input");
            }

            var input = String.Concat(lower.Where(c => alphabet.Contains(c)));

            ReportWriter.Debug($"Sanitized input: {input}");
            return input;
        }

        private static List<Element> GetAnswer(string input, Config config, List<Element> soFar, List<List<Element>> completeAnswers, List<List<Element>> bestAnswers)
        {
            if (input.Length == 0)
            {
                return soFar;
            }

            foreach (var e in RealElement.Table)
            {
                if (input.StartsWith(e.Symbol.ToLower())) {
                    var newList = new List<Element>(soFar)
                    {
                        e
                    };
                    var ans = GetAnswer(input[e.Symbol.Length..], config, newList, completeAnswers, bestAnswers);
                    if (ans != null)
                    {
                        completeAnswers.Add(ans);
                    }
                }
            }

            var options = ProceduralElement.GetOptions(input, config);

            foreach (var p in options)
            {
                var ans = GetAnswer(input[p.Symbol.Length..], config, new List<Element>(soFar) { p }, completeAnswers, bestAnswers);
                if (ans != null)
                {
                    completeAnswers.Add(ans);
                }
            }

            if (GetAnswerLength(soFar) > 0)
            {
                if (bestAnswers.Count() == 0 || GetAnswerLength(bestAnswers[0]) == GetAnswerLength(soFar))
                {
                    bestAnswers.Add(soFar);
                }
                else if (GetAnswerLength(bestAnswers[0]) < GetAnswerLength(soFar))
                {
                    bestAnswers.Clear();
                    bestAnswers.Add(soFar);
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        private static int GetAnswerLength(List<Element> list)
        {
            return list.Sum(e => e.Symbol.Length);
        }
    }
}


