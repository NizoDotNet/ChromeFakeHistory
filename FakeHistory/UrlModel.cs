namespace FakeHistory;

internal class UrlModel
{
    public string URL { get; }
    public string Title { get; }
    public DateTime? Date { get; }

    public UrlModel(string url, string title)
    {
        URL = url;
        Title = title;
    }

    public UrlModel(string url, string title, DateTime? date) 
        : this(url, title)
    {
        Date = date;
    }
}
