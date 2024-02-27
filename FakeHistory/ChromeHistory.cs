using System.Data.SQLite;
using System.Diagnostics;
namespace FakeHistory;

public class ChromeHistory
{
    private static string localAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Google\\Chrome\\User Data\\Default\\History");
    private static string connectionString = $"Data Source={localAppDataPath}";

    public ChromeHistory()
    {

    }

    public int AddHistory(string url, string title, DateTime date)
    {
        CloseChrome();

        using SQLiteConnection sqlConnection = CreateConnection();
        sqlConnection.Open();

        var lastVisitTime = AddUrl(sqlConnection, url, title, date);

        int id = GetId(sqlConnection, url, title);

        AddToVisit(sqlConnection, id, lastVisitTime);

        return id;
    }

    private SQLiteConnection CreateConnection()
    {
        SQLiteConnection sqlConnection = new(connectionString);
        return sqlConnection;
    }

    private long AddUrl(SQLiteConnection sqlConnection, string url, string title, DateTime date)
    {
        using var addUrl = sqlConnection.CreateCommand();
        addUrl.CommandText = """
            INSERT INTO urls(url, title, visit_count, last_visit_time) 
            VALUES (@url, @title, 32, @lastVisitTime);
            """;

        var m = new DateTime(1601, 1, 1);
        var dif = date - m;
        var lastVisitTime = dif.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
        addUrl.Parameters.AddWithValue("url", url);
        addUrl.Parameters.AddWithValue("title", title);
        addUrl.Parameters.AddWithValue("lastVisitTime", lastVisitTime);
        addUrl.ExecuteNonQuery();

        return lastVisitTime;
    }

    private int GetId(SQLiteConnection sqlConnection, string url, string title)
    {
        using var getUrlId = sqlConnection.CreateCommand();
        getUrlId.CommandText = "SELECT id FROM urls WHERE title == @title AND url == @url";
        getUrlId.Parameters.AddWithValue("title", title);
        getUrlId.Parameters.AddWithValue("url", url);
        using var idReader = getUrlId.ExecuteReader();
        idReader.Read();
        int id = idReader.GetInt32(0);

        return id;
    }

    private void AddToVisit(SQLiteConnection sqlConnection, int id, long lastVisitTime)
    {
        using var addToVisits = sqlConnection.CreateCommand();
        addToVisits.CommandText = """
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
