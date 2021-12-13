using System;
using System.Text.RegularExpressions;

namespace SWI.SoftStock.Client.ProcessWatchers
{
    internal static class FindFilesPatternToRegex
    {
        private const string NonDotCharacters = @"[^.]*";
        private static readonly Regex HasQuestionMarkRegEx = new Regex(@"\?", RegexOptions.Compiled);
        private static readonly Regex IlegalCharactersRegex = new Regex("[" + @"\/:<>|" + "\"]", RegexOptions.Compiled);
        private static readonly Regex CatchExtentionRegex = new Regex(@"^\s*.+\.([^\.]+)\s*$", RegexOptions.Compiled);

        public static Regex Convert(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException();
            }
            pattern = pattern.Trim();
            if (pattern.Length == 0)
            {
                throw new ArgumentException("Pattern is empty.");
            }
            if (IlegalCharactersRegex.IsMatch(pattern))
            {
                throw new ArgumentException("Patterns contains ilegal characters.");
            }
            var hasExtension = CatchExtentionRegex.IsMatch(pattern);
            var matchExact = false;
            if (HasQuestionMarkRegEx.IsMatch(pattern))
            {
                matchExact = true;
            }
            else if (hasExtension)
            {
                matchExact = CatchExtentionRegex.Match(pattern).Groups[1].Length != 3;
            }
            var regexString = Regex.Escape(pattern);
            regexString = "^" + Regex.Replace(regexString, @"\\\*", ".*");
            regexString = Regex.Replace(regexString, @"\\\?", ".");
            if (!matchExact && hasExtension)
            {
                regexString += NonDotCharacters;
            }
            regexString += "$";
            var regex = new Regex(regexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex;
        }
    }
}