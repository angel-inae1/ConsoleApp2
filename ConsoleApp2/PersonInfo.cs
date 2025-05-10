namespace ConsoleApp2;

using System;
using System.IO;
using System.Linq;
using System.Text.Json;

public class PersonInfo
{
    public static void ShowPersonInfo()
    {
        Console.WriteLine("Введите Фамилию, Имя и дату рождения в формате Фамилия_Имя_день-месяц-год.json");
        string? filename = Console.ReadLine();
        string filepath = Path.Combine("Data", filename??"");

        if (!File.Exists(filepath))
        {
            Console.WriteLine("Файл не найден");
        }
        
        string json = File.ReadAllText(filepath);
        Person? person = JsonSerializer.Deserialize<Person>(json);

        if (person == null)
        {
            Console.WriteLine("Данных не существует");
        }
        
        Console.WriteLine(person);
        foreach (var entry in person.History.OrderBy(h => h.Date))
        {
            Console.WriteLine($"{entry.Date:dd/MM/yyyy}: Вес {entry.ActualWeight}, ИМТ: {entry.Bmi:F1} {entry.Decoding}");
        }
    }
}