namespace lb1_3;

public class Printer
{
    private readonly PriorityQueue<PrintJob, int> _queue = new();
    private readonly List<PrintStat> _stats = new();


    public void Add(string user, string doc, int priority)
    {
        _queue.Enqueue(new PrintJob(user, doc, priority), priority);
    }
    
    public void ProcessAll()
    {
        while (_queue.TryDequeue(out PrintJob job, out _))
        {
            Console.WriteLine($"Друкується: {job.Doc} (Пріоритет: {job.Priority})");
            _stats.Add(new PrintStat(job.User, job.Doc, DateTime.Now));
        }
    }
    
    public void ShowStats()
    {
        Console.WriteLine("\n--- Статистика ---");
        foreach (var s in _stats) 
            Console.WriteLine($"{s.Time}: {s.User} - {s.Doc}");
    }
    
    public void SaveStats(string path)
    {
        var lines = _stats.Select(s => $"{s.Time}: {s.User} - {s.Doc}");
        File.WriteAllLines(path, lines);
    }
}