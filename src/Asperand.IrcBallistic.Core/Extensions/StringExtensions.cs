namespace Asperand.IrcBallistic.Core.Extensions
{
    public static class StringExtensions
    {
        public static int CharLookAhead(this string source, char charType, int startingIndex)
        {
            for (var i = startingIndex + 1; i < source.Length; i++)
            {
                if (source[i] == charType)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}