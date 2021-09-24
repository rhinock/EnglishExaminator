using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EnglishExaminator
{
    class Program
    {
        public class TupleList<T1, T2, T3> : List<ValueTuple<T1, T2, T3>>
        {
            public void Add(T1 infinitive, T2 simplePast, T3 pastParticiple)
            {
                Add(new ValueTuple<T1, T2, T3>(infinitive, simplePast, pastParticiple));
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            int Score = 0;
            int Count = 0;
            string name;
            string[] str = { "" };
            TupleList<string, string, string> baseTuple = new TupleList<string, string, string>();

            Console.WriteLine("Write the name of file:");
            Console.WriteLine("Press Enter to use default file IrregularVerbsAll.csv");
            do
            {
                name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                    name = "IrregularVerbsAll.csv";
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
            }
            while
            (
                baseTuple.Count != str.Length
                || baseTuple.All
                (
                    i => string.IsNullOrEmpty(i.Item1)
                    && string.IsNullOrEmpty(i.Item2)
                    && string.IsNullOrEmpty(i.Item3)
                )
            );

            Console.WriteLine($"\nScore: { Score } / { Count }, " + $"Left: { baseTuple.Count }\n");

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
                        if (baseTuple[index].Item3 == "-")
                        {
                            form = new Random().Next(0, 2);
                            if (form == 1)
                                Console.WriteLine($"1. { baseTuple[index].Item1 }");
                            else
                                Console.WriteLine($"2. { baseTuple[index].Item2 }");
                        }
                        break;
                }

                string[] split;
                do
                {
                    string input = Console.ReadLine();
                    split = input.Split(' ');

                } while (split.Length != 3 || split.All(i => string.IsNullOrEmpty(i)));

                var tempTuple = baseTuple.FindAll(x => x.Item1 == split[0]);

                Console.WriteLine();
                if (tempTuple.Count != 0)
                {
                    bool isCorrect = false;
                    var updatedTuple = new TupleList<string, string, string>();
                    updatedTuple.Add(split[0], split[1], split[2]);

                    foreach (var tuple in tempTuple)
                    {
                        if (tuple.Equals(updatedTuple.First()))
                        {
                            isCorrect = true;
                            break;
                        }
                    }

                    if (isCorrect)
                    {
                        Score += tempTuple.Count;
                        Console.WriteLine("Correct");
                    }
                    else
                    {
                        Console.WriteLine("Incorrect");
                    }

                    Count += tempTuple.Count;

                    foreach (var tuple in tempTuple)
                    {
                        Console.WriteLine(tuple);
                    }

                    baseTuple.RemoveAll(x => x.Item1 == split[0]);
                }
                else
                {
                    Count++;
                    Console.WriteLine("Incorrect");
                    Console.WriteLine(baseTuple[index]);
                    baseTuple.RemoveAll(x => x.Item1 == baseTuple[index].Item1);
                }

                Console.WriteLine($"Score: { Score } / { Count }, Left: { baseTuple.Count }");
                Console.WriteLine();
            }

            Console.WriteLine("Enter any key for exit");
            Console.ReadKey();
        }
    }
}