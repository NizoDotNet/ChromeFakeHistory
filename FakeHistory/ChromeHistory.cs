using System.Diagnostics;
using System.Data.SQLite;
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

        using SQLiteConnection sqlConnection = new(connectionString);
        sqlConnection.Open();

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

        using var getUrlId = sqlConnection.CreateCommand();
        getUrlId.CommandText = "SELECT id FROM urls WHERE title == @title AND url == @url";
        getUrlId.Parameters.AddWithValue("title", title);
        getUrlId.Parameters.AddWithValue("url", url);
        using var idReader = getUrlId.ExecuteReader();
        idReader.Read();
        int id = idReader.GetInt32(0);

        using var addToVisits = sqlConnection.CreateCommand();
        addToVisits.CommandText = """
                        INSERT INTO visits(url, visit_time, transition, visit_duration, is_known_to_sync) 
                        VALUES(@id, @lastVisitTime, 805306368, 41096701, 1);
                        """;
        addToVisits.Parameters.AddWithValue("id", id);
        addToVisits.Parameters.AddWithValue("lastVisitTime", lastVisitTime);

        addToVisits.ExecuteNonQuery();
        return id;
    }


    private void CloseChrome()
    {
        Process[] chromeInstances = Process.GetProcessesByName("chrome");
        foreach (Process p in chromeInstances)
            p.Kill();
    }


}
