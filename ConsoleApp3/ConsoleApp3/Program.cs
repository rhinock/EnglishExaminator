using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp3
{
    class Program
    {
        public class TupleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
        {
            public void Add(T1 item, T2 item2, T3 item3)
            {
                Add(new Tuple<T1, T2, T3>(item, item2, item3));
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            int Score = 0;
            string name;
            string[] str = { "" };
            TupleList<string, string, string> baseTuple = new TupleList<string, string, string>();
            TupleList<string, string, string> updatedTuple = new TupleList<string, string, string>();

            Console.WriteLine("Write the name of file:");
            do
            {
                name = Console.ReadLine();
                try
                {
                    str = File.ReadAllLines($@"{ name }");
                    foreach (string s in str)
                        baseTuple.Add(s.Split(';')[0], s.Split(';')[1], s.Split(';')[2]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    baseTuple.Clear();
                }
            } while
            (
                baseTuple.Count != str.Length
                ||
                baseTuple.All
                (
                    i => string.IsNullOrEmpty(i.Item1) 
                    && string.IsNullOrEmpty(i.Item2) 
                    && string.IsNullOrEmpty(i.Item3)
                )
            );

            Console.WriteLine($"\nScore: { Score } / { updatedTuple.Count }, " + $"Left: { baseTuple.Count }\n");

            while (baseTuple.Count != 0)
            {
                var form = new Random().Next(0, 3);
                var index = new Random().Next(0, baseTuple.Count);

                switch (form)
                {
                    case 0:
                        Console.WriteLine($"1. { baseTuple[index].Item1 }");
                        break;
                    case 1:
                        Console.WriteLine($"2. { baseTuple[index].Item2 }");
                        break;
                    case 2:
                        Console.WriteLine($"3. { baseTuple[index].Item3 }");
                        break;
                }

                string[] split;
                do
                {
                    string input = Console.ReadLine();
                    split = input.Split(' ');

                } while (split.Length != 3 || split.All(i => string.IsNullOrEmpty(i)));

                updatedTuple.Add(split[0], split[1], split[2]);

                Console.WriteLine();
                if (baseTuple[index].Equals(updatedTuple.Last()))
                {
                    Console.WriteLine("Correct");
                    Score++;
                }
                else
                    Console.WriteLine("Incorrect");

                Console.WriteLine(baseTuple[index]);
                baseTuple.RemoveAt(index);
                Console.WriteLine($"Score: { Score } / { updatedTuple.Count }, Left: { baseTuple.Count }");
                Console.WriteLine();
            }

            Console.WriteLine("Enter any kay for exit");
            Console.ReadKey();
        }
    }
}