using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AdventCode_2a
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] boxIds = File.ReadAllLines(@"input.txt");
            int twoCharacterIds = 0;
            int threeCharacterIds = 0;
            var startTime = DateTime.Now;
            var endTime = DateTime.Now;

            foreach (var boxId in boxIds)
            {
                var characterCount = new Dictionary<char, int>();
                bool hasTwoRepeatCharacters = false;
                bool hasThreeRepeatCharacters = false;
                foreach (var c in boxId.Trim())
                {
                    if (characterCount.ContainsKey(c))
                        characterCount[c]++;
                    else
                        characterCount[c] = 1;
                }

                
                foreach (var pair in characterCount)
                {
                    if (pair.Value == 2 && hasTwoRepeatCharacters == false)
                    {
                        twoCharacterIds++;
                        hasTwoRepeatCharacters = true;
                    }
                    if (pair.Value == 3 && hasThreeRepeatCharacters == false)
                    {
                        threeCharacterIds++;
                        hasThreeRepeatCharacters = true;
                    }
                }
            }
            endTime = DateTime.Now;

            Console.WriteLine("Checksum is {0}{1}", twoCharacterIds * threeCharacterIds, Environment.NewLine);
            Console.WriteLine(endTime - startTime);
            Console.ReadKey();
        }
    }
}
