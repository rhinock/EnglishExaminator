using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeForms
{
    public static class Out
    {
        public static void Write(string str)
        {
            ConsoleColor background = Console.BackgroundColor;
            ConsoleColor foreground = Console.ForegroundColor;

            for (int i = 0; i < str.Length; i++)
            {
                char current = str[i];
                if (current == '^')
                {
                    int add = 0;
                    if (i + 1 < str.Length)
                    {
                        if (str[i + 1] == '^')
                        {
                            Console.Write('^');
                            continue;
                        }
                        if ("0123456789abcdef".Contains(str[i + 1]))
                        {
                            Console.BackgroundColor = (ConsoleColor)Convert.ToInt32(str[i + 1].ToString(), 16);
                            add++;
                        }
                    }
                    if (i + 2 < str.Length && "0123456789abcdef".Contains(str[i + 2]))
                    {
                        Console.ForegroundColor = (ConsoleColor)Convert.ToInt32(str[i + 2].ToString(), 16);
                        add++;
                    }
                    i += add;
                    continue;
                }
                Console.Write(current);
            }

            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }

        public static void WriteLine(string str)
        {
            Write(str);
            Console.WriteLine();
        }
    }
}
