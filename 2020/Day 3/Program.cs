using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    // Has a function that counts trees for a given slope, and two functions that compute relevant answers using it
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // Populate a list with strings corresponding to each line, basically a 2d array
        List<string> skiMap = new List<string>();
        using (StreamReader sr = File.OpenText(path))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                skiMap.Add(s);
            }
            sr.Close();
        }

        // We want the both the x and y size here
        int xSize = skiMap[0].Length;
        int ySize = skiMap.Count;

        // Counts the number of trees (#) encountered on the specified right xDelta, down yDelta slope traversal
        int treeCounter(int xDelta, int yDelta)
        {
            int count = 0;

            // Current position (0, 0)
            int[] position = new int[2];
            position[0] = 0; // x
            position[1] = 0; // y

            // While we haven't reached the end on y
            while(position[1] < ySize)
            {
                // If we're on a tree, increment the count
                if (skiMap[position[1]][position[0]] == '#')
                    count++;

                // If we're about to go too far on x, we wrap around to the beginning based on the difference
                if (position[0] + xDelta > (xSize - 1)) // Reminder that the last index of the map is (xSize - 1)
                {
                    int difference = (xSize - 1) - position[0];
                    position[0] = (xDelta - 1) - difference;
                }
                // Otherwise increase x by xDelta as usual
                else
                    position[0] += xDelta;

                // Either way, increment y to keep going down
                position[1] += yDelta;
            }

            return count;
        };

        // Uses the treeCounter to find the answer to the first question
        int part1()
        {
            return treeCounter(3, 1);
        }
        answer1 = part1();
        Console.WriteLine("Part 1: The number of encountered trees is " + answer1 + "\n");

        // Uses the treeCounter to find the answer to the second question
        int part2()
        {
            return treeCounter(1, 1) * treeCounter(3, 1) * treeCounter(5, 1) * treeCounter(7, 1) * treeCounter(1, 2);
        };
        answer2 = part2();
        Console.WriteLine("Part 2: The product is " + answer2 + "\n");

        return;
    }
}
