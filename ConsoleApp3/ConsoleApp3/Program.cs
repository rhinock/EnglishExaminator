using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Core;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace ThreeForms
{
    class Program
    {
        const int MENU_CONTINUE = 0;
        const int MENU_NEW_SESSION = 1;
        const int MENU_SETTINGS = 2;
        const int MENU_EXIT = 3;

        [STAThread]
        static void Main(string[] args)
        {
            bool result = false;
            int selected;
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
                        break;
                }


            }
            while (selected != MENU_EXIT);


            Console.WriteLine("Enter any key for exit");
            Console.ReadKey();
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
            string title = "Неправильные формы глаголов";

            // Кнопка "Продолжить текущую сессию"
            Button play = new Button();
            play.color = ConsoleColor.Red;
            play.content = "Продолжить";
            play.size = new Tuple<int, int>(play.content.Length + 2, 3);
            play.position = new Tuple<int, int>(width / 2 - play.size.Item1 / 2, 10);

            // Кнопка "Новый файл"
            Button newGame = new Button();
            newGame.content = "Новая сессия";
            newGame.size = new Tuple<int, int>(newGame.content.Length + 2, 3);
            newGame.position = new Tuple<int, int>(width / 2 - newGame.size.Item1 / 2, 15);

            // Кнопка "Новый файл"
            Button settings = new Button();
            settings.content = "Настройки";
            settings.size = new Tuple<int, int>(newGame.content.Length + 2, 3);
            settings.position = new Tuple<int, int>(width / 2 - settings.size.Item1 / 2, 20);

            // Кнопка "Выход"
            Button exit = new Button();
            exit.content = "Выход";
            exit.size = new Tuple<int, int>(exit.content.Length + 2, 3);
            exit.position = new Tuple<int, int>(width / 2 - exit.size.Item1 / 2, 25);

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
                Console.Write("СТРЕЛКИ - для навигации, ENTER - для выбора, ESC - выход");

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
            Console.WriteLine("Choose the file:");
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
                    Out.Write($"[^0cException^07] {ex.Message}\n");
                }
            }
            return verifier;
        }

        static void Game(ThreeFormsVerifier verifier)
        {
            Console.WriteLine($"\nScore: { verifier.Score } / { verifier.Forms.Count }, " + $"Left: { verifier.UpdatedForms.Count }\n");

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
                    Out.WriteLine("^0aCorrect");
                }
                else
                    Out.WriteLine("^0cIncorrect");

                Out.WriteLine(correctness.text);

                Console.WriteLine($"Score: { verifier.Score } / { verifier.Forms.Count }, " + $"Left: { verifier.UpdatedForms.Count }");
                Console.WriteLine();
            }
        }

    }
}