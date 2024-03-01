using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;


namespace FakeHistory;

public class ChromeHistory
{
    private static string localAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Google\\Chrome\\User Data\\Default\\History");
    private static string connectionString = $"Data Source={localAppDataPath}";


    public void AddHistory(string[] urls, string[] titles, DateTime[] dates)
    {
        CloseChrome();

        using SQLiteConnection sqlConnection = CreateConnection();
        sqlConnection.Open();

        UrlOrTitleLengthLessThanAnotherException.ValidateUrlAndTitle(urls, titles);

        for (int i = 0; i < urls.Length; i++)
        {
            AddHistory(sqlConnection, new UrlModel(urls[i], titles[i], dates[i]));
        }
    }

    public void AddHistory(string[] urls, string[] titles)
    {
        CloseChrome();

        using SQLiteConnection sqlConnection = CreateConnection();
        sqlConnection.Open();

        UrlOrTitleLengthLessThanAnotherException.ValidateUrlAndTitle(urls, titles);

        for (int i = 0; i < urls.Length; i++)
        {
            AddHistory(sqlConnection, new UrlModel(urls[i], titles[i]));
        }
    }

    public void AddHistory(string url, string title, DateTime? date)
    {

        CloseChrome();

        using SQLiteConnection sqlConnection = CreateConnection();
        sqlConnection.Open();

        if (date is null)
            AddHistory(sqlConnection, new UrlModel(url, title));
        
        else
            AddHistory(sqlConnection, new UrlModel(url, title, (DateTime)date));

    }

    private int AddHistory(SQLiteConnection sqlConnection, UrlModel model)
    { 
        
        var lastVisitTime = AddUrl(sqlConnection, model);

        int id = GetId(sqlConnection, model);

        AddToVisit(sqlConnection, id, lastVisitTime);

        return id;
    }


    private SQLiteConnection CreateConnection()
    {
        SQLiteConnection sqlConnection = new(connectionString);
        return sqlConnection;
    }

    private long AddUrl(SQLiteConnection sqlConnection, UrlModel model)
    {
        using var addUrl = sqlConnection.CreateCommand();
        addUrl.CommandText = 
            """
            INSERT INTO urls(url, title, visit_count, last_visit_time) 
            VALUES (@url, @title, 32, @lastVisitTime);
            """;

        var lastVisitTime = GetMilliSeconds(model.Date);

        addUrl.Parameters.AddWithValue("url", model.URL);
        addUrl.Parameters.AddWithValue("title", model.Title);
        addUrl.Parameters.AddWithValue("lastVisitTime", lastVisitTime);
        addUrl.ExecuteNonQuery();

        return lastVisitTime;
    }

    private long GetMilliSeconds(DateTime date)
    {
        var m = new DateTime(1601, 1, 1);
        var dif = date - m;
        var lastVisitTime = dif.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
        return lastVisitTime;
    }

    private int GetId(SQLiteConnection sqlConnection, UrlModel model)
    {
        using var getUrlId = sqlConnection.CreateCommand();
        getUrlId.CommandText = "SELECT id FROM urls WHERE title == @title AND url == @url";
        getUrlId.Parameters.AddWithValue("title", model.Title);
        getUrlId.Parameters.AddWithValue("url", model.URL);
        using var idReader = getUrlId.ExecuteReader();
        idReader.Read();
        int id = idReader.GetInt32(0);

        return id;
    }

    private void AddToVisit(SQLiteConnection sqlConnection, int id, long lastVisitTime)
    {
        using var addToVisits = sqlConnection.CreateCommand();
        addToVisits.CommandText = 
                        """
                        INSERT INTO visits(url, visit_time, transition, visit_duration, is_known_to_sync) 
                        VALUES(@id, @lastVisitTime, 805306368, 41096701, 0);
                        """;
        addToVisits.Parameters.AddWithValue("id", id);
        addToVisits.Parameters.AddWithValue("lastVisitTime", lastVisitTime);
        addToVisits.ExecuteNonQuery();
    }
    private void CloseChrome()
    {
        Process[] chromeInstances = Process.GetProcessesByName("chrome");
        foreach (Process p in chromeInstances)
            p.Kill();
    }


}


