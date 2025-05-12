namespace ConsoleApp2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            bool nextStep = true;
            while (nextStep)
            {
                Console.WriteLine("Выберите номер функции программы, которую хотите использовать:");
                Console.WriteLine("1. Добавить нового человека.\n" +
                                  "2. Добавить новые данные к уже существующему человеку\n" +
                                  "3. Информация о человеке.\n" +
                                  "4. Удалить данные о человеке." +
                                  "5. Выйти из программы." +
                                  "6. Посмотреть все файлы.");
                string? input = Console.ReadLine();
                string answer = input ?? "";
                switch (answer)
                {
                    case "1":
                        Person.AddNewPerson();
                        break;
                    case "2":
                        NewData.AddNewData();
                        break;
                    case "3":
                        PersonInfo.ShowPersonInfo();
                        break;
                    case "4":
                        DeletePerson.Delete();
                        break;
                    case "5":
                        nextStep = false;
                        Console.WriteLine("Выход из программы...");
                        break;
                    case "6":
                        ListFiles.ListAllFiles();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова...");
                        break;
                }
            }
        }
    }
}

