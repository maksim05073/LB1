using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using lb1_3;

record PrintJob(string User, string Doc, int Priority);
record PrintStat(string User, string Doc, DateTime Time);

class Program
{
    static void Main()
    {
        var printer = new Printer();
        
        printer.Add("Oleg", "Report.pdf", 5);
        printer.Add("Boss", "Important.doc", 1);
        printer.Add("Anna", "Photo.jpg", 10);
        
        printer.ProcessAll();
        
        printer.ShowStats();
        printer.SaveStats("stats.txt");
        Console.ReadKey();
    }
}
