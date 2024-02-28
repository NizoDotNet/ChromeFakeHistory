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
}
