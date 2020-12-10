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
        // I would have liked to just have the whole thing as a 2d array, but since the map is
        // of theoretically infinite length, I'll just have a third parameter that describes which
        // 'sector' the position is in.
        List<List<string>> fullMap = new List<List<string>>();

        using (StreamReader sr = File.OpenText(path))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                skiMap.Add(s);
            }
            sr.Close();
        }
        // Populate the full map
        fullMap.Add(skiMap);

        // We want the both the x and y size here
        int xSize = skiMap[0].Length;
        int ySize = skiMap.Count;

        // Counts the number of trees (#) encountered on the specified right xDelta, down yDelta slope traversal
        int treeCounter(int xDelta, int yDelta)
        {
            int count = 0;

            // Current position (0, 0, 0)
            int[] position = new int[3];
            position[0] = 0; // sector
            position[1] = 0; // x
            position[2] = 0; // y

            // While we haven't reached the end on y
            while(position[2] < ySize)
            {

                // If we're on a tree, increment the count
                if (fullMap[position[0]][position[2]][position[1]] == '#')
                    count++;

                // If we're about to go too far on x, we add on a new sector and move into it
                if (position[1] + xDelta > (xSize - 1)) // Reminder that the last index of the map is (xSize - 1)
                {
                    fullMap.Add(skiMap);
                    position[0]++;
                    // Careful that we move onto the right x position
                    int difference = (xSize - 1) - position[1];
                    position[1] = (xDelta - 1) - difference;
                }
                // Otherwise increase x by xDelta as usual
                else
                    position[1] += xDelta;

                // Either way, increment y to keep going down
                position[2] += yDelta;
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
