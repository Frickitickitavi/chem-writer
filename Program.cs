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
            var raw = "woohoo";
            var config = new Config();
            var completeAnswers = new List<List<Element>>();
            var bestAnswers = new List<List<Element>>();

            var result = Do(raw, config, completeAnswers, bestAnswers);

            if (result.isComplete)
            {
                ReportWriter.PrintSuccess(result.answers);
            } else if (result.answers.Count() > 0)
            {
                ReportWriter.PrintPartial(result.answers, result.input);
            } else
            {
                ReportWriter.PrintFailure();
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


