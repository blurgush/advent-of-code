using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

class Program
{
    // This class holds information for bag color and contents
    class Bag
    {
        private string color; // The color
        private List<Tuple<string, int>> contents; // The inner bags

        // Constructor: assigns color and initializes contents
        public Bag(string b, List<Tuple<string, int>> cb)
        {
            color = b;
            contents = cb;
        }

        // Getters
        public string getColor() { return color; }
        public List<Tuple<string, int>> getContents() { return contents; }
    }

    // Load the input into Bag objects to work on them
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // First we'll dump the text into a single string, then split it by new lines
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] entries = s.Split(new string[] { "\n", "\r", ".", "bags contain",}, StringSplitOptions.RemoveEmptyEntries);

        // Put them into a list of new Bag objects
        List<Bag> bags = new List<Bag>();

        // Initialize the bags and put them into the list
        for (int i = 0; i < entries.Length; i += 2)
        {
            string color = entries[i].Trim();
            List<Tuple<string, int>> innerBags = new List<Tuple<string, int>>();
            string[] contentEntries = entries[i + 1].Split(new char[] { ',' });
            for (int j = 0; j < contentEntries.Length; j++)
            {
                contentEntries[j].Trim();
                contentEntries[j] = contentEntries[j].Substring(0, contentEntries[j].IndexOf("bag"));
                int number;
                if (!contentEntries[j].Contains("no other")) {
                    number = Convert.ToInt32(contentEntries[j].Substring(1, 1));
                    innerBags.Add(new Tuple<string, int>(contentEntries[j].Substring(3), number));
                }
                else
                {
                    number = 0;
                    innerBags.Add(new Tuple<string, int>(contentEntries[j].Substring(3), number));
                }
            }
            bags.Add(new Bag(color, innerBags));
        }

        // Find the count of bag types that can directly or indirectly contain "shiny gold bags"
        int part1()
        {
            // Keep track of the bags that can eventually hold a shiny gold one
            List<Bag> goodBags = new List<Bag>();

            // First we'll add the bags that can directly hold a shiny gold one
            foreach (Bag b in bags)
            {
                foreach (Tuple<string, int> t in b.getContents())
                {
                    // If the bag can directly hold a shiny gold bag
                    if (t.Item1.Contains("shiny gold")) {
                        goodBags.Add(b);
                    }
                }
            }

            // Now we want to make a function that we can keep calling until we find all the bags
            // We'll also need something to track how many bags we count per run
            int countedBags = 1; // Set to an arbitrary number > 0

            // An inner function
            // It will return 1 when it finds a match, or 0 if it doesn't find one
            int checkContents(Bag b)
            {
                // For each of the content bags
                foreach (Tuple<string, int> t in b.getContents())
                {
                    // Loop through the list of good bags to see if the content bag can hold any of them
                    foreach (Bag gb in goodBags)
                    {
                        if (t.Item1.Contains(gb.getColor()))
                        {
                            goodBags.Add(b);
                            return 1;
                        }
                    }
                }
                return 0;
            }

            // The outer/main function for counting bags
            void countBags()
            {
                countedBags = 0; // We'll set it to 0 at the start
                foreach (Bag b in bags)
                {
                    // If the bag is already counted, go to the next one
                    if (goodBags.Contains(b))
                        continue;
                    else // If the bag has not yet been counted, check the content bags
                        countedBags += checkContents(b);
                }
            }

            // Keep calling the function until we don't find any more bags
            while (countedBags > 0)
            {
                countBags();
            }

            return goodBags.Count;
        }
        answer1 = part1();
        Console.WriteLine("Part 1: " + answer1 + " bags can eventually contain at least one shiny gold bag.\n");

        // Count how many individual bags are stored inside a shiny gold bag
        // Pretty much the opposite of part 1, except the numbers are important here
        int part2()
        {
            // This function takes a string bag name and returns the bag to which the name belongs
            Bag getBag(string name)
            {
                foreach (Bag b in bags)
                {
                    if (name.Contains(b.getColor()))
                    {
                        return b;
                    }
                }
                // If the name is wrong, this will show up
                return bags[593];
            }

            // This is gonna be like traversing a tree; we'll do it recursively with a function
            // Takes the bag, how many there are, and returns the sum of bags contained within
            int returnInnerBagCount(int n, Bag b)
            {
                int count = 0;

                // Base case is where the current bag contains no other bags
                if (b.getContents()[0].Item1.Contains("other"))
                    return 0;

                // Otherwise, the bag contains other bags
                else
                {
                    List<Bag> children = new List<Bag>(); // List of child bags
                    List<int> counts = new List<int>(); // The amount of each child bag

                    // Loop through and grab the child bags
                    // No bag holds more than (1 digit) of a single type
                    for (int i = 0; i < b.getContents().Count; i++)
                    {
                        string s = b.getContents()[i].Item1;
                        children.Add(getBag(s));                // Add the child bag to the list
                        int m = b.getContents()[i].Item2;
                        counts.Add(m);                          // Add the multiplier to the list
                    }

                    // Now add the counts of each child bag, as well as their inner bag counts
                    for (int i = 0; i < children.Count; i++)
                    {
                        count += counts[i] + returnInnerBagCount(counts[i], children[i]);
                    }

                    // Return the sum, multiplied by the count of bags
                    return n * count;
                }
            }

            return returnInnerBagCount(1, getBag("shiny gold"));
        }
        answer2 = part2();
        Console.WriteLine("Part 2: A single shiny gold bag must contain " + answer2 + " other bags.\n");

        return;
    }
}
