using FakeHistory;

ChromeHistory chromeHistory = new();

string url = Console.ReadLine();
string title = Console.ReadLine();
DateTime date = DateTime.Parse(Console.ReadLine());

chromeHistory.AddHistory($"https://{url}", title, date);

Console.WriteLine("End");

Console.ReadLine();