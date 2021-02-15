using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

class Program
{
    // Some basic number counting/manipulation
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two long answers
        long answer1;
        long answer2;

        // First we'll dump the text into a single string, then split it by new lines
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] lines = s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Now into a long list
        List<long> entries = new List<long>();
        foreach (string line in lines)
            entries.Add(Int64.Parse(line));

        // Checks if a number is a sum of any two values in a given range
        bool checkSum(int firstIndex, int targetIndex)
        {
            for (int i = firstIndex; i < targetIndex; i++)
                for (int j = firstIndex; j < targetIndex; j++)
                    if (entries[i] + entries[j] == entries[targetIndex] && i != j)
                        return true;
            return false;
        }

        // Finds the first number that is not a sum of any two of the previous 25 numbers
        long part1()
        {
            // We go through the whole list, starting after the preamble
            for (int i = 25; i < entries.Count; i++)
                if (!checkSum(i - 25, i))
                    return entries[i];

            return -1;
        }
        answer1 = part1();
        Console.WriteLine("Part 1: The first non-sum number is " + answer1 + ".\n");

        // Gets the sum of a long list (there might be a built in function for this, but I don't know.)
        long sumList(List<long> l)
        {
            long sum = 0;
            foreach (long num in l)
                sum += num;
            return sum;
        }

        // Sums the minimum and maximum vales of a long list
        long sumMinMax(List<long> l)
        {
            // Get the maximum
            long max = 0;
            foreach (long num in l)
                if (num > max)
                    max = num;

            // Get the minimum
            long min = max;
            foreach (long num in l)
                if (num < min)
                    min = num;

            return min + max;
        }

        // Sums the first and last values of a contiguous set in entries that sums to the answer of part1
        long part2(long value)
        {
            List<long> sumSet = new List<long>();
            int startIndex = 0;
            int i = startIndex;

            while (sumList(sumSet) != value)
            {
                sumSet.Add(entries[i]);
                i++;

                if (sumList(sumSet) > value)
                {
                    startIndex += 1;
                    i = startIndex;
                    sumSet.Clear();
                    continue;
                }
            }

            return sumMinMax(sumSet);
        }
        answer2 = part2(answer1);
        Console.WriteLine("Part 2: The encryption weakness is " + answer2 + ".\n");

        return;
    }
}
