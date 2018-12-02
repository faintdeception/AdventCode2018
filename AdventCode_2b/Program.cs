using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventCode_2a {
    class Program {
        static void Main(string[] args)
        {
            string[] boxIds = File.ReadAllLines(@"input.txt");
            List<string> closeMatches = new List<string>();
            StringBuilder matchedCharacters = new StringBuilder();
            int nonMatchingCharacterIndex = -1;

            foreach (var boxId in boxIds)
            {
                foreach (var innerBoxId in boxIds)
                {
                    int matchingCharacterCount = 0;

                    if (innerBoxId != boxId)
                    {
                        for (int i = 0; i < innerBoxId.Length; i++)
                        {
                            if (innerBoxId[i] == boxId[i])
                            {
                                matchingCharacterCount++;
                            }
                        }


                    }

                    if (matchingCharacterCount == boxId.Length - 1)
                    {
                        Console.WriteLine(innerBoxId);
                        closeMatches.Add(innerBoxId);
                    }
                }
            }

            if (closeMatches.Count > 1)
            {
                for (int i = 0; i < closeMatches[0].Length; i++)
                {
                    if (closeMatches[0][i] != closeMatches[1][i])
                        nonMatchingCharacterIndex = i;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(closeMatches[0].Remove(nonMatchingCharacterIndex, 1).Insert(nonMatchingCharacterIndex, " "));
            }

            Console.ReadKey();
        }
    }
}
