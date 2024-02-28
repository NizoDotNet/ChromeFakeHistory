namespace FakeHistory;


public class UrlOrTitleLengthLessThanAnotherException : Exception
{
    
    public UrlOrTitleLengthLessThanAnotherException(string message) 
        : base(message) 
    {
    }

    public static void ValidateUrlAndTitle(string[] url, string[] title)
    {
        if(url.Length !=  title.Length)
        {
            throw new UrlOrTitleLengthLessThanAnotherException("");
        }
    }
}
