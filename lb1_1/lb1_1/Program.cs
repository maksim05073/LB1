using System.Text.Json;
using System.Xml.Serialization;

namespace lb1_1;

class Program
{
    static List<RouteTaxi> routeTaxis = new();

    static void Main(string[] args)
    {
        routeTaxis = InitializeData();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Показати всі маршрути");
            Console.WriteLine("2. Додати маршрут");
            Console.WriteLine("3. Редагувати маршрут");
            Console.WriteLine("4. Видалити маршрут");
            Console.WriteLine("5. Знайти маршрут за зупинкою");
            Console.WriteLine("6. Зберегти у JSON та XML");
            Console.WriteLine("0. Вийти");
            Console.Write("Ваш вибір: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowAll();
                    break;
                case "2":
                    AddRoute();
                    break;
                case "3":
                    EditRoute();
                    break;
                case "4":
                    DeleteRoute();
                    break;
                case "5":
                    SearchByStop();
                    break;
                case "6":
                    SerializeAll();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Невірний вибір!");
                    break;
            }
        }
    }

    static List<RouteTaxi> InitializeData()
    {
        return new List<RouteTaxi>
        {
            new RouteTaxi { RouteNumber = "101", Fare = 10m, IntervalBetweenStopsMin = 4,
                Stops = new[] { "Центр", "Університет", "Базар", "Автовокзал" } },
            new RouteTaxi { RouteNumber = "202", Fare = 12.5m, IntervalBetweenStopsMin = 3,
                Stops = new[] { "Центр", "Магазин", "Лікарня", "Автовокзал" } },
            new RouteTaxi { RouteNumber = "303", Fare = 9.5m, IntervalBetweenStopsMin = 5,
                Stops = new[] { "Школа", "Парк", "Ринок" } }
        };
    }

    static void ShowAll()
    {
        Console.WriteLine("\nСписок маршрутів:");
        foreach (var r in routeTaxis)
            Console.WriteLine(r);
    }

    static void AddRoute()
    {
        Console.Write("Номер маршруту: ");
        string num = Console.ReadLine();

        Console.Write("Вартість (грн): ");
        decimal fare = decimal.Parse(Console.ReadLine());

        Console.Write("Інтервал між зупинками (хв): ");
        int interval = int.Parse(Console.ReadLine());

        Console.Write("Введіть зупинки через кому: ");
        string[] stops = Console.ReadLine().Split(',', StringSplitOptions.TrimEntries);

        routeTaxis.Add(new RouteTaxi { RouteNumber = num, Fare = fare, IntervalBetweenStopsMin = interval, Stops = stops });
        Console.WriteLine("Маршрут додано.");
    }

    static void EditRoute()
    {
        Console.Write("Введіть номер маршруту для редагування: ");
        string num = Console.ReadLine();

        var route = routeTaxis.FirstOrDefault(r => r.RouteNumber == num);
        if (route == null)
        {
            Console.WriteLine("Маршрут не знайдено.");
            return;
        }

        Console.Write("Нова ціна (або Enter щоб залишити): ");
        string priceInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(priceInput))
            route.Fare = decimal.Parse(priceInput);

        Console.Write("Новий інтервал (або Enter щоб залишити): ");
        string intInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(intInput))
            route.IntervalBetweenStopsMin = int.Parse(intInput);

        Console.Write("Нові зупинки (через кому, або Enter щоб залишити): ");
        string stopsInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(stopsInput))
            route.Stops = stopsInput.Split(',', StringSplitOptions.TrimEntries);

        Console.WriteLine("Дані оновлено.");
    }

    static void DeleteRoute()
    {
        Console.Write("Введіть номер маршруту для видалення: ");
        string num = Console.ReadLine();

        var route = routeTaxis.FirstOrDefault(r => r.RouteNumber == num);
        if (route != null)
        {
            routeTaxis.Remove(route);
            Console.WriteLine("Маршрут видалено.");
        }
        else
        {
            Console.WriteLine("Маршрут не знайдено.");
        }
    }

    static void SearchByStop()
    {
        Console.Write("Введіть назву зупинки: ");
        string stop = Console.ReadLine();

        var result = routeTaxis.Where(r => r.Stops.Any(s => s.Equals(stop, StringComparison.OrdinalIgnoreCase))).ToList();

        if (result.Count == 0)
        {
            Console.WriteLine("Жоден маршрут не містить цієї зупинки.");
            return;
        }

        int minTime = result.Min(r => r.TotalTravelTime);

        Console.WriteLine("\nНайшвидші маршрути до пункту:");
        foreach (var r in result.Where(r => r.TotalTravelTime == minTime))
            Console.WriteLine(r);
    }

    static void SerializeAll()
    {
        SerializeToJson("routes.json", routeTaxis);
        SerializeToXml("routes.xml", routeTaxis);
        Console.WriteLine("Дані збережено у JSON та XML.");
    }

    static void SerializeToJson(string path, List<RouteTaxi> data)
    {
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    static void SerializeToXml(string path, List<RouteTaxi> data)
    {
        XmlSerializer serializer = new(typeof(List<RouteTaxi>));
        using FileStream fs = new(path, FileMode.Create);
        serializer.Serialize(fs, data);
    }
}
