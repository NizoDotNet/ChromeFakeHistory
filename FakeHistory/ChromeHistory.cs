using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;


namespace FakeHistory;

public class ChromeHistory
{
    

    private DatabaseContext _databaseContext;

    public ChromeHistory()
    {
        _databaseContext = new();
    }

    public void AddHistory(string[] urls, string[] titles, DateTime[] dates)
    {
        CloseChrome();
        _databaseContext.AddHistory(urls, titles, dates);
    }

    public void AddHistory(string[] urls, string[] titles)
    {
        CloseChrome();

        _databaseContext.AddHistory(urls, titles);
    }

    public void AddHistory(string url, string title, DateTime? date)
    {

        CloseChrome();

        _databaseContext.AddHistory(url, title, date);

    }
    private void CloseChrome()
    {
        Process[] chromeInstances = Process.GetProcessesByName("chrome");
        foreach (Process p in chromeInstances)
            p.Kill();
    }


}


