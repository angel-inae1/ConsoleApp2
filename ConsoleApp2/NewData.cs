namespace ConsoleApp2;
using System.Text.Json;
using System.IO;

public class NewData : PersonBase
{
    public NewData(DateTime dateofrecording, double actualweight)
    {
        DateOfRecording = dateofrecording;
        ActualWeight = actualweight;
    }
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

    public static Person? AddNewData()
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
            DateTime dateofrecording;
            while (!DateTime.TryParseExact(Console.ReadLine(), 
                       "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, 
                       System.Globalization.DateTimeStyles.None, 
                       out dateofrecording))
            {
                Console.Write("Неверный формат. Повторите ввод (дд.ММ.гггг): ");
            }
            
            Console.Write("Актуальный вес:");
            double actualweight;
            bool weightResult;
            do
            {
                string? input = Console.ReadLine();
                weightResult = double.TryParse(input, out actualweight);
                if (!weightResult || actualweight <= 0)
                {
                    Console.Write("Некорректные данные, введите правильный вес: ");
                }
            } while (!weightResult || actualweight <= 0);
            
            var (bmi, decoding) = person.Calculatebmi(actualweight, person.Height);
            Console.WriteLine($"Ваш актуальный ИМТ: {bmi:F1} ({decoding})");
            
            double progress = person.Weight - actualweight;
            Console.WriteLine($"Ваш прогресс: {progress} кг");

            double progress2 = actualweight - person.DoneWeight;
            Console.WriteLine($"Осталось {progress2} кг");

            var entry = new PersonHistoryEntry
            {
                Date = dateofrecording,
                ActualWeight = actualweight,
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