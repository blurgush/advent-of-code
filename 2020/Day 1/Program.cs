using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    // Returns the product of the two (or three) values in a text file that sum to 2020
    static int Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // Populate a list with integers corresponding to each line of input.txt
        List<int> years = new List<int>();
        using (StreamReader sr = File.OpenText(path))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                years.Add(Int32.Parse(s));
            }
            sr.Close();
        }
        int size = years.Count;

        // Doubly loop through the list, looking for the two values that sum to 2020
        int part1()
        {
            for (int i = 0; i < size; i++)
            {
                int year1 = years[i];
                for (int j = 0; j < size; j++)
                {
                    int year2 = years[j];
                    if (year1 + year2 == 2020)
                        return year1 * year2;
                }
            }
            return -1;
        };
        answer1 = part1();
        Console.WriteLine("Part 1: The product is " + answer1 + "\n");

        // Triply loop through the list, looking for the three values that sum to 2020
        int part2()
        {
            for (int i = 0; i < size; i++)
            {
                int year1 = years[i];
                for (int j = 0; j < size; j++)
                {
                    int year2 = years[j];
                    for (int k = 0; k < size; k++)
                    {
                        int year3 = years[k];
                        if (year1 + year2 + year3 == 2020)
                            return year1 * year2 * year3;
                    }
                }
            }
            return -1;
        };
        answer2 = part2();
        Console.WriteLine("Part 2: The product is " + answer2 + "\n");

        return 0;
    }
}
