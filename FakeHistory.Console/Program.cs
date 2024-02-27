using FakeHistory;

ChromeHistory chromeHistory = new();

chromeHistory.AddHistory("https://x.com", "new", DateTime.UtcNow);

Console.WriteLine("End");

Console.ReadLine();