using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

class Program
{
    //
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // First we'll dump the text into a single string, then split it by new lines
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] lines = s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Now into an ordered int list
        List<int> entries = new List<int>();
        foreach (string line in lines)
            entries.Add(Int32.Parse(line));
        entries.Sort();

        // Find the product of 1 and 3 volt differences in a full adapter chain
        int part1()
        {
            // Initialize two counts for the differences
            int oneVoltDiffs = 0;
            int threeVoltDiffs = 0;

            // Add the first difference
            if (entries[0] - 0 == 1)
                oneVoltDiffs++;
            else if (entries[0] - 0 == 3)
                threeVoltDiffs++;

            foreach (int i in entries)
            {
                if (i < entries.Count - 1)
                {
                    if (entries[i + 1] - entries[i] == 1)
                        oneVoltDiffs++;
                    else if (entries[i + 1] - entries[i] == 3)
                        threeVoltDiffs++;
                }
            }

            return oneVoltDiffs * threeVoltDiffs;
        }
        answer1 = part1();
        Console.WriteLine("Part 1: The product is " + answer1 + ".\n");

        //
        int part2()
        {
            return 0;
        }
        answer2 = part2();
        Console.WriteLine("Part 2: The encryption weakness is " + answer2 + ".\n");

        return;
    }
}
