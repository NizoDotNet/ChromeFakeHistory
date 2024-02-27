using FakeHistory;

ChromeHistory chromeHistory = new();

chromeHistory.AddHistory("https://www.youtube.com", "qq", new(2024, 2, 27, 19, 34, 32));

Console.WriteLine("End");

Console.ReadLine();