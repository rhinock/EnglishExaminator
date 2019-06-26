using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Core;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;

namespace ThreeForms
{
    class Program
    {
        // Номера пунктов меню
        const int MENU_CONTINUE = 0;
        const int MENU_NEW_SESSION = 1;
        const int MENU_SETTINGS = 2;
        const int MENU_EXIT = 3;

        static LocalizedStrings localization;

        [STAThread]
        static void Main(string[] args)
        {
            int selected;
            LoadLocalization(Properties.App.Default.LanguageFile);
            do
            {
                selected = Menu();
                switch (selected)
                {
                    case MENU_CONTINUE:
                        {
                            if (File.Exists("save.bin"))
                            {
                                ThreeFormsVerifier verifier;
                                BinaryFormatter formatter = new BinaryFormatter();
                                using (FileStream fs = new FileStream("save.bin", FileMode.OpenOrCreate))
                                {
                                    verifier = (ThreeFormsVerifier)formatter.Deserialize(fs);
                                }
                                Console.Clear();
                                Game(verifier);
                            }
                            break;
                        }
                        
                    case MENU_NEW_SESSION:
                        {
                            ThreeFormsVerifier verifier = OpenFileMenu();
                            Console.Clear();
                            Game(verifier);
                            break;
                        }

                    case MENU_SETTINGS:
                        int language = Settings();
                        if (language == 0)
                        {
                            Properties.App.Default.LanguageFile = null;
                            Properties.App.Default.Save();
                        }
                        else if (language == 1)
                        {
                            Properties.App.Default.LanguageFile = "ru.json";
                            Properties.App.Default.Save();
                        }
                        LoadLocalization(Properties.App.Default.LanguageFile);
                        break;
                }


            }
            while (selected != MENU_EXIT);
        }


        static void LoadLocalization(string localizationFile)
        {
            if (localizationFile != null && File.Exists(localizationFile))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(LocalizedStrings));
                using (FileStream fs = new FileStream(localizationFile, FileMode.OpenOrCreate))
                {
                    localization = (LocalizedStrings)jsonFormatter.ReadObject(fs);
                }
            }
            else
            {
                localization = new LocalizedStrings();
                return;
            }
        }

        /// <summary>
        /// Главное меню
        /// </summary>
        /// <returns>Номер нажатой кнопки</returns>
        static int Menu()
        {
            // Размеры консоли
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            // Положение фокуса на кнопке
            int cursor = 0;

            // Информация о нажатой клавише
            ConsoleKeyInfo keyInfo;

            // Список кнопок
            List<Button> buttons = new List<Button>();

            // Заголовок
            string title = localization.title;

            // Кнопка "Продолжить текущую сессию"
            Button play = new Button();
            play.color = ConsoleColor.Red;
            play.content = localization.continueGame;
            play.size = new Tuple<int, int>(play.content.Length + 2, 3);
            play.position = new Tuple<int, int>(width / 2 - play.size.Item1 / 2, 7);

            // Кнопка "Новый файл"
            Button newGame = new Button();
            newGame.content = localization.newSession;
            newGame.size = new Tuple<int, int>(newGame.content.Length + 2, 3);
            newGame.position = new Tuple<int, int>(width / 2 - newGame.size.Item1 / 2, 12);

            // Кнопка "Новый файл"
            Button settings = new Button();
            settings.content = localization.settings;
            settings.size = new Tuple<int, int>(newGame.content.Length + 2, 3);
            settings.position = new Tuple<int, int>(width / 2 - settings.size.Item1 / 2, 17);

            // Кнопка "Выход"
            Button exit = new Button();
            exit.content = localization.exit;
            exit.size = new Tuple<int, int>(exit.content.Length + 2, 3);
            exit.position = new Tuple<int, int>(width / 2 - exit.size.Item1 / 2, 22);

            // Добавление кнопок в общий список
            buttons.Add(play);
            buttons.Add(newGame);
            buttons.Add(settings);
            buttons.Add(exit);

            // Главный цикл меню
            do
            {
                // Очистка экрана
                Console.Clear();

                // Печать заголовка
                Console.CursorTop = 3;
                Console.CursorLeft = width / 2 - title.Length / 2;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(title);

                // Отображение кнопок
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (i == cursor)
                        buttons[i].focus = true;
                    else
                        buttons[i].focus = false;

                    buttons[i].Print();
                }

                // Отображение подсказки
                Console.CursorLeft = 0;
                Console.CursorTop = height - 1;
                Console.Write(localization.helpString);

                // Считывание нажатой клавиши
                keyInfo = Console.ReadKey(true);

                // Обработка нажатых кнопок
                if (keyInfo.Key == ConsoleKey.Escape) return 1;
                else if (keyInfo.Key == ConsoleKey.UpArrow && cursor > 0) cursor--;
                else if (keyInfo.Key == ConsoleKey.DownArrow && cursor < buttons.Count - 1) cursor++;

            }
            while (keyInfo.Key != ConsoleKey.Enter);
            return cursor;
        }

        /// <summary>
        /// Меню настроек
        /// </summary>
        /// <returns>Номер нажатой кнопки</returns>
        static int Settings()
        {
            // Размеры консоли
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            // Положение фокуса на кнопке
            int cursor = 0;

            // Информация о нажатой клавише
            ConsoleKeyInfo keyInfo;

            // Список кнопок
            List<Button> buttons = new List<Button>();

            // Заголовок
            string title = localization.settings;

            // Кнопка "English"
            Button english = new Button();
            english.color = ConsoleColor.Red;
            english.content = "English";
            english.size = new Tuple<int, int>(english.content.Length + 2, 3);
            english.position = new Tuple<int, int>(width / 2 - english.size.Item1 / 2, 10);

            // Кнопка "Русский"
            Button russian = new Button();
            russian.content = "Русский";
            russian.size = new Tuple<int, int>(russian.content.Length + 2, 3);
            russian.position = new Tuple<int, int>(width / 2 - russian.size.Item1 / 2, 15);

            // Добавление кнопок в общий список
            buttons.Add(english);
            buttons.Add(russian);

            // Главный цикл меню
            do
            {
                // Очистка экрана
                Console.Clear();

                // Печать заголовка
                Console.CursorTop = 3;
                Console.CursorLeft = width / 2 - title.Length / 2;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(title);

                // Отображение кнопок
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (i == cursor)
                        buttons[i].focus = true;
                    else
                        buttons[i].focus = false;

                    buttons[i].Print();
                }

                // Отображение подсказки
                Console.CursorLeft = 0;
                Console.CursorTop = height - 1;
                Console.Write(localization.helpString);

                // Считывание нажатой клавиши
                keyInfo = Console.ReadKey(true);

                // Обработка нажатых кнопок
                if (keyInfo.Key == ConsoleKey.Escape) return 1;
                else if (keyInfo.Key == ConsoleKey.UpArrow && cursor > 0) cursor--;
                else if (keyInfo.Key == ConsoleKey.DownArrow && cursor < buttons.Count - 1) cursor++;

            }
            while (keyInfo.Key != ConsoleKey.Enter);
            return cursor;
        }

        static ThreeFormsVerifier OpenFileMenu()
        {
            ThreeFormsVerifier verifier = null;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    verifier = new ThreeFormsVerifier(ThreeFormsParser.ParseFile(dialog.FileName));
                }
                catch (Exception ex)
                {
                    Out.Write($"[^0c{localization.exception}^07] {ex.Message}\n");
                }
            }
            return verifier;
        }

        static void Game(ThreeFormsVerifier verifier)
        {
            Console.WriteLine($"\n{localization.score}: { verifier.Score } / { verifier.Forms.Count }, " + $"{localization.left}: { verifier.UpdatedForms.Count }\n");

            while (!verifier.GameEnd)
            {
                Console.WriteLine(verifier.GetRandomWord());
                (bool rezult, string text) correctness = verifier.CheckRezults(Console.ReadLine());

                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream("save.bin", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, verifier);
                }

                Console.WriteLine();
                if (correctness.rezult)
                {
                    Out.WriteLine($"^0a{localization.correct}");
                }
                else
                    Out.WriteLine($"^0c{localization.incorrect}");

                Out.WriteLine(correctness.text);

                Console.WriteLine($"{localization.score}: { verifier.Score } / { verifier.Forms.Count }, " + $"{localization.left}: { verifier.UpdatedForms.Count }");
                Console.WriteLine();
            }
        }

    }
}