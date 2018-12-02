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

            foreach (var boxId in boxIds)
            {
                var characterCount = new Dictionary<char, int>();
                bool hasTwoRepeatCharacters = false;
                bool hasThreeRepeatCharacters = false;
                Debug.WriteLine(boxId);
                foreach (var c in boxId.Trim())
                {
                    if (characterCount.ContainsKey(c))
                        characterCount[c]++;
                    else
                        characterCount[c] = 1;
                }

                
                foreach (var pair in characterCount)
                {
                    Debug.WriteLine("{0} - {1}", pair.Key, pair.Value);
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

            Debug.WriteLine("Two Character Ids {0}", twoCharacterIds);
            Debug.WriteLine("Three Character Ids {0}", threeCharacterIds);

            Console.Write("Checksum is {0}", twoCharacterIds * threeCharacterIds);

            Console.ReadKey();
        }
    }
}
