using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeForms
{
    /// <summary>
    /// Ячейка/кнопка
    /// </summary>
    class Button
    {
        // Позиция на экране
        public Tuple<int, int> position = new Tuple<int, int>(0, 0);

        // Размер кнопки с учетом рамок
        public Tuple<int, int> size = new Tuple<int, int>(10, 3);

        // Текст кнопки
        public string content = "Button";

        // Цвет кнопки
        public ConsoleColor color = ConsoleColor.White;

        // Является ли кнопка выделенной
        public bool focus = false;

        /// <summary>
        /// Вывод кнопки на экран
        /// </summary>
        public void Print()
        {
            // Настройка цвета
            if (focus) Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = color;

            // Координаты верхней рамки
            Console.CursorLeft = position.Item1;
            Console.CursorTop = position.Item2;

            // "Создание" верхней рамки
            string spaces = "";
            string afterspaces = "";
            for (int i = 0; i < size.Item1 - 2; i++) spaces += '═';
            Console.Write("╒{0}╗", spaces);
            Console.CursorLeft = position.Item1;
            Console.CursorTop += 1;

            // Рисование содержимого кнопки и "стенок"
            for (int j = 0; j < size.Item2 - 2; j++)
            {
                // Содержимое
                if (size.Item2 / 2 == Console.CursorTop - position.Item2)
                {
                    // Определение отступов текста от краев рамки и печать
                    spaces = "";
                    afterspaces = "";
                    for (int i = 0; i < (size.Item1 - content.Length) / 2 - 1; i++) spaces += ' ';
                    for (int i = 0; i < size.Item1 - (size.Item1 - content.Length) / 2 - 1 - content.Length; i++) afterspaces += ' ';
                    Console.Write("│{0}{1}{2}║", spaces, content, afterspaces);
                }
                // Стенки
                else
                {
                    // Определение отступов и печать
                    spaces = "";
                    for (int i = 0; i < size.Item1 - 2; i++) spaces += ' ';
                    Console.Write("│{0}║", spaces);
                }

                // Положение курсора для рисования следующей строки
                Console.CursorLeft = position.Item1;
                Console.CursorTop += 1;
            }

            // Создание "нижней" рамки
            spaces = "";
            for (int i = 0; i < size.Item1 - 2; i++) spaces += '─';
            Console.Write("└{0}╜", spaces);
            Console.CursorLeft = position.Item1;

            // Восстановление цветовых настроек
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
