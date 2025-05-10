namespace ConsoleApp2;
using System.Text.Json;
using System.IO;

public class NewData : PersonBase
{
    public static Person? LoadFromJson(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("файл не найден");
            return null;
        }

        string json = File.ReadAllText(filePath);
        Person? person = JsonSerializer.Deserialize<Person>(json);
        if (person == null)
        {
            Console.WriteLine("Ошибка");
        }

        return person;
    }

    public Person? AddNewData()
    {
        Console.WriteLine("Введите Фамилию, Имя и дату рождения в формате Фамилия_Имя_день-месяц-год.json");
        string? filename = Console.ReadLine();
        if (string.IsNullOrEmpty(filename))
        {
            Console.WriteLine("Файл не может быть без названия");
            return null;
        }
        string filepath = Path.Combine("Data", filename);
        Person? person = LoadFromJson(filepath);
        if (person != null)
        {
            Console.WriteLine(person);
            
            Console.WriteLine("Введите новые данные:");
            Console.Write("Дата записи:");
            bool dateResult = DateTime.TryParse(Console.ReadLine(), out DateTime date);
            while (!dateResult)
            {
                Console.Write("Некорректные ввод:");
                dateResult = DateTime.TryParse(Console.ReadLine(), out date);
            }
            person.DateOfRecording = date;
            
            Console.Write("Актуальный вес:");
            bool weightResult = double.TryParse(Console.ReadLine(), out double personWeight);
            while (!weightResult)
            {
                Console.Write("Некорректные данные, введите правильный вес: ");
                weightResult = double.TryParse(Console.ReadLine(), out personWeight);
            }
            person.ActualWeight = personWeight;
            
            var (bmi, decoding) = person.Calculatebmi(person.ActualWeight,person.Height);
            Console.WriteLine($"Ваш актуальный ИМТ: {bmi:F1} ({decoding})");
            
            double progress = person.Weight - person.ActualWeight;
            Console.WriteLine($"Ваш прогресс: {progress} кг");

            double progress2 = person.ActualWeight - person.DoneWeight;
            Console.WriteLine($"Осталось {progress2} кг");

            var entry = new PersonHistoryEntry
            {
                Date = person.DateOfRecording,
                ActualWeight = person.ActualWeight,
                Bmi = bmi,
                Decoding = decoding
            };
            person.History.Add(entry);
            string json = JsonSerializer.Serialize(person, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filepath, json);
            Console.WriteLine("Новые данные сохранены");
        }
        return person;
    }
}