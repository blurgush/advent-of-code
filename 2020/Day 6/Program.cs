using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

class Program
{
    // Gather a grouped list of questionnaire data into strings that represent groups
    // And sum up queries for unique and unanimous responses
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // First we'll dump the text into a single string, then split it by blank lines
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] entries = s.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        // We're left with an array of strings, each representing a group

        // Sum the amount of unique Yes answers in each group
        // Each character in the group strings represents a single Yes answer
        // In other words, count the unique characters in each string
        int part1()
        {
            int count = 0; // The count of total unique "Yes" answers

            // Group by group
            foreach (string group in entries)
            {
                int groupCount = 0; // The count of unique "Yes" answers per group

                // We need to keep track of the unique characters per group so we don't overcount
                List<char> countedChars = new List<char>();

                // Go through the group character by character
                for (int i = 0; i < group.Length; i++)
                {
                    char currChar = group[i];

                    // If we haven't already seen this character (or it's a newline), put it in the unique chars list and increment the count
                    if (!countedChars.Contains(currChar) && currChar != '\n')
                    {
                        countedChars.Add(currChar);
                        groupCount++;
                    }
                }
  
                count += groupCount; // Add the group count to the total count
            }

            return count;
        }
        answer1 = part1();
        Console.WriteLine("Part 1: The sum of Yes counts is " + answer1 + "\n");

        // Sum the number of questions for which everyone in a group answered yes
        int part2()
        {
            int count = 0; // The count of total unanimous "Yes" answers

            // We can count in a similar way to part 1
            foreach (string group in entries)
            {
                int groupCount = 0; // The count of unanimous "Yes" answers per group
                List<char> countedChars = new List<char>(); // List of unique characters per group

                // First we get a count of people in the group (# of newlines + 1)
                int peopleCount = 1;
                foreach (char c in group)
                    if (c == '\n') peopleCount++;
                // There's a special case here, being that the last group has a newline at the end
                // I can't think of any reasonable way to deal with that in the if statement
                // So we'll do a separate check below 
                if (group == entries[entries.Length - 1])
                    peopleCount--;

                // Go through the group character by character
                for (int i = 0; i < group.Length; i++)
                {
                    // Track the character and its count
                    char currChar = group[i];
                    int currCharCount = 0;

                    // If we haven't already seen this character (or it's a newline),
                    if (!countedChars.Contains(currChar) && currChar != '\n')
                    {
                        // Put it in the unique chars list
                        countedChars.Add(currChar);

                        // Count the occurrences of the character
                        foreach (char c in group)
                            if (c == currChar)
                                currCharCount++;

                        // If the count is equal to the number of people in the group, it's a unanimous yes
                        if (currCharCount == peopleCount)
                            groupCount++;
                    }
                }

                count += groupCount; // Add the group count to the total count
            }

            return count;
        }
        answer2 = part2();
        Console.WriteLine("Part 2: The sum of unanimous Yes counts is " + answer2 + "\n");

        return;
    }
}
