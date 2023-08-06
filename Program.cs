using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace BirthdayReminderApp
{
    class Program
    {
        // создание списка birthdayList
        static List<BirthdayEntry> birthdayList = new List<BirthdayEntry>();

        static void Main(string[] args)
        {
            DateTime currentTime = DateTime.Now; //сохранение текущего времени
            string formattedDate = currentTime.ToString("dd.MM.yyyy HH:mm");
            bool exit = false;
            LoadListFromFile("birthdayList.txt");
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать в Поздравлятор!");
                Console.WriteLine("Сегодня: " + formattedDate + "\n");
                NextBirthdayDate();
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Показать список дней рождения");
                Console.WriteLine("2. Добавить запись в список");
                Console.WriteLine("3. Удалить запись из списка");
                Console.WriteLine("4. Сохранить список в файл");
                Console.WriteLine("4. Изменить информацию по индексу");
                Console.WriteLine("6. Выход");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ShowBirthdayList();
                        break;
                    case "2":
                        AddBirthdayEntry();
                        break;
                    case "3":
                        DeleteBirthdayEntry();
                        break;
                    case "4":
                        SaveListToFile("birthdayList.txt");
                        break;
                    case "5":
                        ChangeBirthdayInf();
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        private static void ShowBirthdayList()
        {
            Console.Clear();
            if (birthdayList.Count == 0)
            {
                Console.WriteLine("Список дней рождения пуст.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Список дней рождения:");
                foreach (var entry in birthdayList)
                {
                    Console.WriteLine($"{entry.Name} - {entry.Date:dd.MM.yyyy}");
                }
                Console.ReadKey();
            }
        }

        private static void AddBirthdayEntry()
        {
            Console.Clear();
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите дату рождения (дд.мм.гггг): ");
            string dateString = Console.ReadLine();
            if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                birthdayList.Add(new BirthdayEntry { Name = name, Date = date });
                Console.WriteLine("Запись добавлена.");
            }
            else
            {
                Console.WriteLine("Ошибка ввода даты. Запись не добавлена.");
            }
        }

        private static void DeleteBirthdayEntry()
        {
            if (birthdayList.Count == 0)
            {
                Console.WriteLine("Список дней рождения пуст.");
            }
            else
            {
                Console.WriteLine("Выберите номер записи для удаления:");
                for (int i = 0; i < birthdayList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {birthdayList[i].Name} - {birthdayList[i].Date:dd.MM.yyyy}");
                }

                string input = Console.ReadLine();
                if (int.TryParse(input, out int index) && index >= 1 && index <= birthdayList.Count)
                {
                    birthdayList.RemoveAt(index - 1);
                    Console.WriteLine("Запись удалена.");
                }
                else
                {
                    Console.WriteLine("Неверный выбор. Запись не удалена.");
                }
            }
        }

        private static void NextBirthdayDate()
        {
            DateTime currentTime = DateTime.Now;
            string formattedDate = currentTime.ToString("dd.MM.yyyy HH:mm");
            if (birthdayList.Count == 0)
            {
                Console.WriteLine("Список дней рождения пуст.");
            }
            else
            {
                Console.WriteLine("Ближайшие дни рождения(менее месяца): ");
                for (int i = 0; i < birthdayList.Count; i++)
                {
                    if ((currentTime.Month - birthdayList[i].Date.Month == -1) && (currentTime.Day - birthdayList[i].Date.Day < 31))
                    {
                        Console.WriteLine($"{i + 1}. {birthdayList[i].Name} - {birthdayList[i].Date:dd.MM.yyyy}");
                    }
                }
            }
        }

        private static void SaveListToFile(string filename)
        {
            try
            {
                List<string> line = new List<string>();
                foreach (var x in birthdayList)
                {
                    line.Add($"{x.Name} - {x.Date:dd.MM.yyyy}");
                }
                string content = string.Join(Environment.NewLine, line);
                System.IO.File.WriteAllText(filename, content); // Сохранить данные в файл
                Console.Clear();
                Console.WriteLine("Данные сохранены успешно.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении данных: " + ex.Message);
            }
        }

        private static void LoadListFromFile(string fileName)
        {
            Console.Clear();
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    Console.WriteLine("Файл данных не существует. Создан новый список дней рождения.");
                    return;
                }

                string content = System.IO.File.ReadAllText(fileName); // Прочитать содержимое файла
                string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); // Разбить на отдельные строки

                birthdayList.Clear();
                foreach (var line in lines)
                {
                    string[] parts = line.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && DateTime.TryParseExact(parts[1], "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                    {
                        birthdayList.Add(new BirthdayEntry { Name = parts[0], Date = date }); // Восстановить данные из файла в список
                    }
                }

                Console.WriteLine("Данные загружены успешно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private static void ChangeBirthdayInf()
        {
            Console.Clear();
            if (birthdayList.Count == 0)
            {
                Console.WriteLine("Список дней рождения пуст.");
            }
            else
            {
                Console.WriteLine("Выберите номер записи для редактирования:");
                for (int i = 0; i < birthdayList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {birthdayList[i].Name} - {birthdayList[i].Date:dd.MM.yyyy}");
                }

                string input = Console.ReadLine();
                if (int.TryParse(input, out int index) && index >= 1 && index <= birthdayList.Count)
                {
                    Console.WriteLine("Введите новое имя: ");
                    string newName = Console.ReadLine();

                    Console.WriteLine("Введите новую дату рождения (дд.мм.гггг): ");
                    string newDateString = Console.ReadLine();
                    if (DateTime.TryParseExact(newDateString, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime newDate))
                    {
                        birthdayList[index - 1].Name = newName; // Изменить имя в выбранной записи
                        birthdayList[index - 1].Date = newDate; // Изменить дату рождения в выбранной записи
                        Console.WriteLine("Запись успешно изменена.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода новой даты. Запись не изменена.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный выбор. Запись не удалена.");
                }
            }
        }
    }

    public class BirthdayEntry
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}

