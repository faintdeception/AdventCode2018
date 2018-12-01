using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AdventCode_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int> visitedFrequencies = new Dictionary<int, int>();
            int currentFrequency = 0;
            bool foundDuplicateFrequency = false;

            string[] frequencyDeltas = File.ReadAllLines(@"input.txt");

            while (foundDuplicateFrequency == false)
            {
                foreach (string inputDelta in frequencyDeltas)
                {

                    bool isValidInput = int.TryParse(inputDelta.Trim(), out int currentFrequencyDelta);

                    if (!isValidInput)
                    {
                        Debug.WriteLine(string.Format("Invalid input detected: '{0}' is not a number.", inputDelta.Trim()));
                    }
                    else
                    {
                        currentFrequency += currentFrequencyDelta;

                        if (visitedFrequencies.ContainsKey(currentFrequency))
                        {
                            visitedFrequencies[currentFrequency]++;
                            Console.WriteLine("Duplicate Frequency Detected at: {0}!", currentFrequency);
                            foundDuplicateFrequency = true;
                            break;
                        }

                        visitedFrequencies.Add(currentFrequency, 1);
                    }
                }
            }
            
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
