namespace FakeHistory;


internal class UrlOrTitleLengthLessThanAnotherException : Exception
{

    internal UrlOrTitleLengthLessThanAnotherException(string message) 
        : base(message) 
    {
    }

    internal static void ValidateUrlAndTitle(string[] url, string[] title)
    {
        if(url.Length !=  title.Length)
        {
            throw new UrlOrTitleLengthLessThanAnotherException("");
        }
    }

    internal static void ValidateUrlAndTitle(string[] url, string[] title, DateTime[] dates)
    {
        if (url.Length != title.Length && dates.Length != url.Length)
        {
            throw new UrlOrTitleLengthLessThanAnotherException("");
        }
    }
}
