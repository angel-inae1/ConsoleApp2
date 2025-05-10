namespace ConsoleApp2;
using System.Text.Json;
using System.IO;
public class Person : PersonBase
{
    public List<PersonHistoryEntry> History { get; set; } = new();
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
    public Person AddNewPerson()
    {
        Person person = new Person();
        
        Console.Write("Имя:");
        person.Name = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(person.Name))
        {
            Console.Write("Введите имя:");
            person.Name = Console.ReadLine();
        }
        Console.Write("Фамилия:");
        person.Surname = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(person.Surname))
        {
            Console.Write("Введите фамилию:");
            person.Surname = Console.ReadLine();
        }
       
        Console.Write("Дата рождения: ");
        DateTime birthday;

        while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy",
                   System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                   out birthday))
        {
            Console.WriteLine("Некорректный ввод, введите данные в порядку день.месяц.год");
        }
        person.Birthday = birthday;
        
        Console.WriteLine("Возраст:" + person.GetAge());
        
        Console.Write("Рост в м: ");
        bool heightResult = double.TryParse(Console.ReadLine(), out double personHeight);
        while (!heightResult || personHeight <= 0)
        {
            Console.Write("Некорректные данные, введите правильный рост: ");
            heightResult = double.TryParse(Console.ReadLine(), out personHeight);
        }
        person.Height = personHeight;
        
        Console.Write("Вес: ");
        bool weightResult = double.TryParse(Console.ReadLine(), out double personWeight);
        while (!weightResult)
        {
            Console.Write("Некорректные данные, введите правильный вес: ");
            weightResult = double.TryParse(Console.ReadLine(), out personWeight);
        }
        person.Weight = personWeight;
        
        Console.Write("Желаемый вес: ");
        bool doneWeightResult = double.TryParse(Console.ReadLine(), out double personDoneWeight);
        while (!doneWeightResult)
        {
            Console.Write("Некорректный ввод: ");
            doneWeightResult = double.TryParse(Console.ReadLine(), out personDoneWeight);
        }
        person.DoneWeight = personDoneWeight;
        
        var (bmi, decoding) = Calculatebmi(person.Weight, person.Height);
        Console.WriteLine($"Индекс массы тела: {bmi:F1} ({decoding})");
        
        Console.WriteLine($"Данные, которые вы ввели: {person.Name} {person.Surname} {person.Birthday.ToString("d")}  ({person.GetAge()} лет)\n Рост: {person.Height}м Вес: {person.Weight}кг \n Желаемый вес: {person.DoneWeight}кг Индекс массы тела: {bmi:F1} ({decoding})");
        
        Console.WriteLine("Хотите сохранить данные? да/нет");
        string answer;
        do
        {
            string? input = Console.ReadLine();
            answer = input?.ToLower()?? "";
        } while (answer != "да" && answer != "д" && answer != "н" && answer != "не" && answer != "нет");

        switch (answer)
        {
            case "д" or "да":
                string directory = "Data";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string datasave = person.Birthday.ToString("dd-MM-yyyy");
                string filename = $"{person.Surname}_{person.Name}_{datasave}.json";
                string path = Path.Combine(directory, filename);
                string json = JsonSerializer.Serialize(person, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
                Console.WriteLine($"Данные сохранены в файл: {filename}");
                break;
            case "н" or "не" or "нет":
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