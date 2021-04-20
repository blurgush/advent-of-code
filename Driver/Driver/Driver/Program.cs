using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

class Program
{
    // Advent of Code 2020 Day 10
    static void Main(string[] args)
    {
        string path = @"input.txt";

        // There is an integer and long answer
        int answer1;
        long answer2;

        // First we'll dump the text into a single string, then split it by new lines
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] lines = s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Now into an ordered int list of adapter output joltages
        List<int> joltages = new List<int>();
        joltages.Add(0); // This represents the wall's adapter
        foreach (string line in lines)
            joltages.Add(Int32.Parse(line));
        joltages.Sort();

        // Including your device's built in joltage
        int builtInJoltage = joltages[^1] + 3;
        joltages.Add(builtInJoltage);

        // Finds the product of 1 and 3 jolt differences in a full adapter chain
        int part1()
        {
            // Initialize two counts for the differences and one for your personal adapter's joltage
            int oneJoltDiffs = 0;
            int threeJoltDiffs = 0;

            // Iterate the list of sorted joltages
            for (int i = 0; i < joltages.Count; i++)
            {
                // Except for the last value, compare the adapter with the next one
                if (i < joltages.Count - 1)
                {
                    if (joltages[i + 1] - joltages[i] == 1)
                        oneJoltDiffs++;
                    else if (joltages[i + 1] - joltages[i] == 3)
                        threeJoltDiffs++;
                }
            }

            return oneJoltDiffs * threeJoltDiffs;
        }
        answer1 = part1();
        Console.WriteLine("Part 1: The product is " + answer1 + ".\n");

        // Counts the number of possible adapter chains from the wall to the device
        // Using a dynamic programming algorithm 
        long part2()
        {
            // This list will represent the number of ways to get to each adapter
            // The problem's solution is numChains[^1]
            List<long> numChains = new List<long>();
            for (int i = 0; i < joltages.Count; i++)
                numChains.Add(1);

            // Do the first three adapters manually (base case)
            numChains[0] = 1; // The base adapter
            numChains[1] = 1;
            if (joltages[2] - joltages[0] <= 3) // Can the second adapter be skipped?
                numChains[2] = 2;
            else
                numChains[2] = 1;

            // Iterate through each adapter (from the fourth) and count the possibilities
            for (int i = 3; i < joltages.Count; i++)
            {
                int currAdapter = joltages[i]; // Joltage of the current adapter
                long validJumps = 0; // How many ways there are to get to this adapter

                // We only need to check the previous three
                // Since there are no duplicates in the input
                if (currAdapter - joltages[i - 1] <= 3)
                    validJumps += numChains[i - 1];
                if (currAdapter - joltages[i - 2] <= 3)
                    validJumps += numChains[i - 2];
                if (currAdapter - joltages[i - 3] <= 3)
                    validJumps += numChains[i - 3];

                numChains[i] *= validJumps;
            }

            // Return the number of ways to get to the final adapter (the device)
            return numChains[^1];
        }
        answer2 = part2();
        Console.WriteLine("Part 2: There are " + answer2 + " possible adapter chains.\n");

        return;
    }
}
