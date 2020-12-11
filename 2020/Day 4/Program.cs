using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

class Program
{
    // Reads passport information from a text file into a list of <string, string> dictionaries and checks various conditions
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // We're going to store the passports in a list of dictionaries
        List<Dictionary<string, string>> passports = new List<Dictionary<string, string>>();

        // First we'll dump the text into a single string, then split it based on blank lines
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] entries = s.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

        // Populate a list with dictionaries corresponding to each data entry
        for (int i = 0; i < entries.Length; i++)
        {
            // Initialize a dictionary for the current entry
            Dictionary<string, string> currDict = new Dictionary<string, string>();

            // Split the string into an array with each key:value pair
            string[] currData = entries[i].Split(new char[] { ' ', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            // Split the pairs into keys and values, and load them into the dictionary
            for (int j = 0; j < currData.Length; j++)
            {
                string[] keyValues = currData[j].Split(':');
                currDict.Add(keyValues[0], keyValues[1]);
            }

            // Add the current dictionary to the passports list
            passports.Add(currDict);
        }

        // Traverse the dictionary, and count the number of passports that contain every field (except cid)
        int part1()
        {
            int count = 0;

            for (int i = 0; i < passports.Count; i++)
            {
                Dictionary<string, string> p = passports[i];
                
                // I wish I could do the below statement in a better way
                bool valid = p.ContainsKey("byr") && p.ContainsKey("iyr") && p.ContainsKey("eyr") && p.ContainsKey("hgt") 
                    && p.ContainsKey("hcl") && p.ContainsKey("ecl") && p.ContainsKey("pid");
                
                if (valid)
                    count++;         
            }

            return count;
        }
        answer1 = part1();
        Console.WriteLine("Part 1: The number of valid passports is " + answer1 + "\n");

        // Traverse the dictionary and count the number of valid passports whose fields adhere to certain conditions
        int part2()
        {
            int count = 0;
            Dictionary<string, string> p = new Dictionary<string, string>();

            for (int i = 0; i < passports.Count; i++)
            {
                p = passports[i];

                // I wish I could do the below statement in a better way
                bool fieldsPresent = p.ContainsKey("byr") && p.ContainsKey("iyr") && p.ContainsKey("eyr") && p.ContainsKey("hgt")
                    && p.ContainsKey("hcl") && p.ContainsKey("ecl") && p.ContainsKey("pid");

                if (fieldsPresent) // We now need to verify all the other conditions
                {
                    // sample number for using TryParse
                    int t;

                    // byr is four digits between 1920 and 2002 
                    bool byrValid = false;
                    if (int.TryParse(p["byr"], out t)) // Make sure the string is numeric
                        byrValid = Int32.Parse(p["byr"]) >= 1920 && Int32.Parse(p["byr"]) <= 2002;

                    // iyr is four digits between 2010 and 2020
                    bool iyrValid = false;
                    if (int.TryParse(p["iyr"], out t)) // Make sure the string is numeric
                       iyrValid = Int32.Parse(p["iyr"]) >= 2010 && Int32.Parse(p["iyr"]) <= 2020;

                    // eyr is four digits between 2020 and 2030
                    bool eyrValid = false;
                    if (int.TryParse(p["eyr"], out t)) // Make sure the string is numeric
                        eyrValid = Int32.Parse(p["eyr"]) >= 2020 && Int32.Parse(p["eyr"]) <= 2030;

                    // hgt is either a 3 digit number between 150 and 193 that ends in "cm"
                    // or a 2 digit number between 59 and 76 that ends in "in"
                    bool hgtValid = false;
                    if (p["hgt"].Contains("cm") && p["hgt"].Length == 5 && int.TryParse(p["hgt"].Substring(0, 3), out t))
                        hgtValid = Int32.Parse(p["hgt"].Substring(0, 3)) >= 150 && Int32.Parse(p["hgt"].Substring(0, 3)) <= 193;
                    else if (p["hgt"].Contains("in") && p["hgt"].Length == 4 && int.TryParse(p["hgt"].Substring(0, 2), out t))
                        hgtValid = Int32.Parse(p["hgt"].Substring(0, 2)) >= 59 && Int32.Parse(p["hgt"].Substring(0, 2)) <= 76;

                    // hcl is a '#' followed by six characters 0-9 or a-f
                    bool hclValid = false;
                    if (p["hcl"][0] == '#' && p["hcl"].Length == 7)
                    {
                        hclValid = true;
                        for (int j = 0; j < p["hcl"].Length - 1; j++)
                        {
                            char c = p["hcl"].Substring(1)[j];
                            if (!(c >= 48 && c <= 57) && !(c >= 97 && c <= 102)) // Ascii code range condition
                                hclValid = false;
                        }
                    }

                    // ecl is any of "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
                    bool eclValid = p["ecl"] == "amb" || p["ecl"] == "blu" || p["ecl"] == "brn" || p["ecl"] == "gry" || p["ecl"] == "grn" || p["ecl"] == "hzl" || p["ecl"] == "oth";

                    // pid is a 9 digit number
                    bool pidValid = false;
                    if (p["pid"].Length == 9 && int.TryParse(p["pid"], out t))
                        pidValid = true;

                    if (byrValid && iyrValid && eyrValid && hgtValid && hclValid && eclValid && pidValid)
                        count++;
                }
            }

            return count;
        };
        answer2 = part2();
        Console.WriteLine("Part 2: The number of valid passports is " + answer2 + "\n");

        return;
    }
}
