using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    // Checks a list of passwords with given policies and obtains the number of valid passwords
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // Populate 3 lists with strings corresponding to each line of input.txt, delineated by spaces
        List<string> policies = new List<string>();
        List<string> letters = new List<string>();
        List<string> passwords = new List<string>();
        using (StreamReader sr = File.OpenText(path))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                string[] lineContent = s.Split(' ');
                policies.Add(lineContent[0]);
                letters.Add(lineContent[1]);
                passwords.Add(lineContent[2]);
            }
            sr.Close();
        }
        int size = policies.Count;

        // Doing some extra work on policies to get rid of the dashes (-)
        // Making it into a list of tuples
        List<Tuple<int, int>> policyTuples = new List<Tuple<int, int>>();
        for (int i = 0; i < size; i++)
        {
            string[] sArr = policies[i].Split('-');
            Tuple<int, int> policy = new Tuple<int, int>(Int32.Parse(sArr[0]), Int32.Parse(sArr[1]));
            policyTuples.Add(policy);
        }

        // Return the number of valid passwords, where the given character appears n times, where n is between two policy numbers
        int part1()
        {
            int count = 0;
            for (int i = 0; i < size; i++)
            {
                // Gather current password data
                Tuple<int, int> currPolicy = policyTuples[i];
                char currLetter = char.Parse(letters[i].Substring(0, 1));
                string currPassword = passwords[i];

                // Count the number of times currLetter is in the password
                int currCount = 0;
                for (int j = 0; j < currPassword.Length; j++)
                {
                    if (currPassword[j] == currLetter)
                        currCount++;
                }

                // Increment count if the password conforms to its policy
                if (currPolicy.Item1 <= currCount)
                    if (currCount <= currPolicy.Item2)
                        count++;
            }
            return count;
        };
        answer1 = part1();
        Console.WriteLine("Part 1: The number of valid passwords is " + answer1 + "\n");

        // Return the number of valid passwords, where the given character must appear in exactly one of two defined policy slots
        int part2()
        {
            int count = 0;

            for (int i = 0; i < size; i++)
            {
                // Gather current password data
                Tuple<int, int> currPolicy = policyTuples[i];
                char currLetter = char.Parse(letters[i].Substring(0, 1));
                string currPassword = passwords[i];

                // Check both policy slots for the specified letter
                bool slot1 = (currPassword[currPolicy.Item1 - 1] == currLetter);
                bool slot2 = (currPassword[currPolicy.Item2 - 1] == currLetter);

                // Increment count if either bools are true, but not if both are true
                if (slot1 && !slot2)
                    count++;
                else if (!slot1 && slot2)
                    count++;
            }

            return count;
        };
        answer2 = part2();
        Console.WriteLine("Part 2: The number of valid passwords is " + answer2 + "\n");

        return;
    }
}
