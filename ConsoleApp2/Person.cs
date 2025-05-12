namespace ConsoleApp2;
using System.Text.Json;
using System.IO;
public class Person : PersonBase
{
    public List<PersonHistoryEntry> History { get; set; } = new();

    public Person(string name, string surname, DateTime birthday, double height, double weight, double doneWeight)
    {
        Name = name;
        Surname = surname;
        Birthday = birthday;
        Height = height;
        Weight = weight;
        DoneWeight = doneWeight;
    }
    
    public int GetAge()
    {
        var today = DateTime.Today;
        int age = today.Year - Birthday.Year;
        if (Birthday.Date > today.AddYears(-age))
            age--;
        return age;
    }

    public double Index()
    {
       var (bmi, decoding) = Calculatebmi(Weight, Height);
       Console.WriteLine(decoding);
       return bmi;
    }
    public static Person AddNewPerson()
    {
        Console.Write("Имя:");
        string? name;
        do
        {
            name = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(name));
        
        Console.Write("Фамилия:");
        string? surname;
        do
        {
            surname = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(surname));
       
       
        Console.Write("Дата рождения (дд.ММ.гггг): ");
        DateTime birthday;
        while (!DateTime.TryParseExact(Console.ReadLine(), 
                   "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, 
                   System.Globalization.DateTimeStyles.None, 
                   out birthday))
        {
            Console.Write("Неверный формат. Повторите ввод (дд.ММ.гггг): ");
        }
        
        
        Console.WriteLine("Возраст:" + new Person(name,surname,birthday,0,0,0).GetAge());
        
        Console.Write("Рост в м: ");
        double height;
        bool heightResult;
        do
        {
            string? input = Console.ReadLine();
            heightResult = double.TryParse(input, out height);
            if (!heightResult || height <= 0)
            {
                Console.Write("Неверный ввод. Повторите ввод роста в метрах: ");
            }
        }
        while (!heightResult || height <= 0);
        
        
        Console.Write("Вес: ");
        double weight;
        bool weightResult;
        do
        {
            string? input = Console.ReadLine();
            weightResult = double.TryParse(input, out weight);
            if (!weightResult || weight <= 0)
            {
                Console.Write("Некорректные данные, введите правильный вес: ");
            }
        } while (!weightResult || weight <= 0);
        
        Console.Write("Желаемый вес: ");
        double doneWeight;
        bool doneWeightResult;
        do
        {
            string? input = Console.ReadLine();
            doneWeightResult = double.TryParse(input, out doneWeight);
            if (!doneWeightResult || doneWeight <= 0)
            {
                Console.Write("Некорректный ввод: ");
            }
        } while (!doneWeightResult || doneWeight <= 0);
        
        Person person = new Person(name, surname, birthday, height, weight, doneWeight);
        
        var (bmi, decoding) = person.Calculatebmi(weight, height);
        Console.WriteLine($"Индекс массы тела: {bmi:F1} ({decoding})");
        
        Console.WriteLine($"Данные, которые вы ввели: {name} {surname} {birthday.ToString("d")} " +
                          $" ({person.GetAge()} лет)\n " +
                          $"Рост: {height}м Вес: {weight}кг \n" +
                          $" Желаемый вес: {doneWeight}кг Индекс массы тела: {bmi:F1} ({decoding})");
        
        Console.WriteLine("Хотите сохранить данные? да/нет");
        string answer;
        do
        {
            string? input = Console.ReadLine();
            answer = input?.ToLower()?? "";
        } while (answer != "да" && answer != "д" && answer != "н" && answer != "не" && answer != "нет");

        switch (answer)
        {
            case "д":
            case "да":
                string directory = "Data";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string datasave = birthday.ToString("dd-MM-yyyy");
                string filename = $"{surname}_{name}_{datasave}.json";
                string path = Path.Combine(directory, filename);
                string json = JsonSerializer.Serialize(person, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
                Console.WriteLine($"Данные сохранены в файл: {filename}");
                break;
            case "н":
            case "не":
            case "нет":
                Console.WriteLine("Данные не сохранены!");
                break;
        }
        return person;
    }

    public override string ToString()
    {
        var (bmi, decoding) = Calculatebmi(Weight, Height);
        return $"Имя: {Name}\n" +
               $"Фамилия: {Surname}\n" +
               $"Дата рождения: {Birthday}\n" +
               $"Возраст: {GetAge()}\n" +
               $"Рост: {Height} м\n" +
               $"Вес: {Weight} кг\n" +
               $"Желаемый вес: {DoneWeight} кг \n" +
               $"Индекс массы тела: {bmi:F1} ({decoding})";
    }
}