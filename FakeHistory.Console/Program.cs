using FakeHistory;

ChromeHistory chromeHistory = new();

chromeHistory.AddHistory("https://www.youtube.com", "old", new(2024, 2, 23));

Console.WriteLine("End");

Console.ReadLine();