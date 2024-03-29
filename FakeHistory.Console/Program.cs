﻿using FakeHistory;

Console.WriteLine("Start");

try
{
    ChromeHistory chromeHistory = new();

    string[] urls = new string[5];
    string[] titles = new string[5];
    DateTime[] dates = new DateTime[5];


    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine("URL: ");
        urls[i] = $"https://{Console.ReadLine()}";
        Console.WriteLine("Title: ");
        titles[i] = Console.ReadLine();
        Console.WriteLine("Date: ");
        if (DateTime.TryParse(Console.ReadLine(), out var date))
        {
            dates[i] = date;
        }
    }

    chromeHistory.AddHistory(urls, titles, dates);
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}



Console.WriteLine("End");

Console.ReadLine();