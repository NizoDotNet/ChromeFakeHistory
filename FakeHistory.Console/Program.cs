using FakeHistory;

ChromeHistory chromeHistory = new();

string[] urls = new string[5];
string[] titles = new string[5];
for (int i = 0; i < 5; i++)
{
    Console.WriteLine("URL: ");
    urls[i] = $"https://{Console.ReadLine()}";
    Console.WriteLine("Title: ");
    titles[i] = Console.ReadLine();
}

chromeHistory.AddHistory(urls, titles);


Console.WriteLine("End");

Console.ReadLine();