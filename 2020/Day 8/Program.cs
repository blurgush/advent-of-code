using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

class Program
{
    // This class holds information for each instruction
    class Instruction
    {
        private string op;
        private int arg;
        private int lineNum;
        
        // Constructor
        public Instruction(string o, string a, int l)
        {
            op = o;
            lineNum = l;
            int n = Int32.Parse(a.Substring(1, a.Length - 1));

            if (a[0] == '+')
                arg = n;
            else
                arg = n * -1;
        }

        // Getters and setters
        public string getOp()
        {
            return op;
        }

        public int getArg()
        {
            return arg;
        }

        public int getLineNum()
        {
            return lineNum;
        }

        public void setOp(string s)
        {
            op = s;
        }
    }

    // Makes Instruction objects for all the input data, and navigates through a list of them
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // First we'll dump the text into a single string, then split it by new lines and spaces
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] entries = s.Split(new string[] { "\n", " " }, StringSplitOptions.RemoveEmptyEntries);

        // Now we load the data into a list of Instruction objects
        List<Instruction> instructions = new List<Instruction>();
        for (int i = 0; i < entries.Length; i += 2)
            instructions.Add(new Instruction(entries[i], entries[i + 1], i / 2));

        // This function will return the value of the accumulator right before the first repeat instruction is made
        // It also returns a bool that indicates whether the program will loop infinitely
        Tuple<int, bool> returnAccumulator()
        {
            List<int> usedOps = new List<int>();
            int accum = 0;

            // If this code runs the same amount of times as there is instructions, 
            // that means we haven't found any repeated instructions
            int i = 0;
            while (i < instructions.Count)
            {
                // Gather the instruction information
                string op = instructions[i].getOp();
                int arg = instructions[i].getArg();
                int lineNo = instructions[i].getLineNum();

                // If the instruction has not yet been executed
                if (!usedOps.Contains(lineNo))
                {
                    usedOps.Add(lineNo);    // Add the line number to the used instructions list
                    if (op == "jmp")        // Jump to the next line
                        i += arg - 1;
                    else if (op == "acc")   // Increment the accumulator
                        accum += arg;
                }
                else
                    break;

                i++;
            }
            
            // If there are repeated instructions (the while loop ends early)
            // there is an infinite loop
            // If not, the loop terminates
            bool isInfinite = i < instructions.Count;

            return new Tuple<int, bool>(accum, isInfinite);
        }

        // Finds the value of the accumulator immediately before the first repeated instruction
        int part1()
        {
            return returnAccumulator().Item1;
        }
        answer1 = part1();
        Console.WriteLine("Part 1: The value of the accumulator is " + answer1 + ".\n");

        // Finds the accumulator value after the program successfully terminates
        // Done by finding the jmp or nop instruction that needs to be swapped
        int part2()
        {
            int dangerCount = 0; // The number of instructions that could be the faulty one
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].getOp() == "nop" || instructions[i].getOp() == "jmp")
                    dangerCount++;
            }

            // Try every possible swapping of nop and jmp instructions
            List<Instruction> triedInstructs = new List<Instruction>();
            for (int i = 0; i < dangerCount; i++)
            {
                int changedIndex = 0; // We have to remember which one we changed so we can change it back
                
                // Go in and swap the first jmp/nop instruction that hasn't been tried yet
                for (int j = 0; j < instructions.Count; j++) {
                    changedIndex = j;
                    if (instructions[j].getOp() == "nop" && !triedInstructs.Contains(instructions[j]))
                    {
                        instructions[j].setOp("jmp");
                        triedInstructs.Add(instructions[j]);
                        break;
                    }
                    else if (instructions[j].getOp() == "jmp" && !triedInstructs.Contains(instructions[j]))
                    {
                        instructions[j].setOp("nop");
                        triedInstructs.Add(instructions[j]);
                        break;
                    }
                }

                // Navigate through the program with the changed instruction
                // If an instruction is executed more than once, we're in an infinite loop
                // When that doesn't happen, we made the right change
                if (!returnAccumulator().Item2)
                    return returnAccumulator().Item1;
                else
                {
                    // Change the instruction back and move onto the next one
                    if (instructions[changedIndex].getOp() == "nop")
                        instructions[changedIndex].setOp("jmp");
                    else
                        instructions[changedIndex].setOp("nop");
                }
            }
            return 0;
        }
        answer2 = part2();
        Console.WriteLine("Part 2: The accumulator of the successful program is " + answer2 + ".\n");

        return;
    }
}
