namespace lb1_1;

public class RouteTaxi
{
    public string RouteNumber { get; set; }
    public decimal Fare { get; set; }
    public string[] Stops { get; set; }
    public int IntervalBetweenStopsMin { get; set; }

    public int TotalTravelTime => Stops.Length * IntervalBetweenStopsMin;

    public override string ToString()
    {
        return $"Маршрут №{RouteNumber}, Ціна: {Fare} грн, " +
               $"Час: {TotalTravelTime} хв, Зупинки: {string.Join(" -> ", Stops)}";
    }
}