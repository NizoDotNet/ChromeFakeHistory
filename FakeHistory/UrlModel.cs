namespace FakeHistory;

internal record UrlModel
{
    public string URL { get; }
    public string Title { get; }
    public DateTime Date { get; }

    public UrlModel(string url, string title)
    {
        URL = url;
        Title = title;
        Date = DateTime.UtcNow;
    }

    public UrlModel(string url, string title, DateTime date)
        : this(url, title)
    {
        Date = date;
    }
}
