using System.Text.RegularExpressions;

namespace SigaClasses.Utils
{
    public static class Remover
    {
        public static string ReplaceMultipleSpacesByPipe(string value)
        {
            return Regex.Replace(value, " {2,}", "|");
        }

        public static string RemoveSeparators(string value)
        {
            return value.Replace("\t"," ").Replace("\r", " ").Replace("\n", " ");
        }

    }
}
