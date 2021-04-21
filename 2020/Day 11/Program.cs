using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

class Program
{
    // Advent of Code 2020 Day 11
    static void Main(string[] args)
    {
        string path = @"input.txt";

        // There are two int answers
        int answer1;
        int answer2;

        // First we'll dump the text into a single string, then split it by new lines
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] lines = s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Returns the number of occupied seats next to a given seat
        int countNeighbors(List<List<char>> state, int row, int col)
        {
            int above = 0;
            if (row > 0)
                above = state[row - 1][col] == '#' ? 1 : 0;

            int aboveLeft = 0;
            if (row > 0 && col > 0)
                aboveLeft = state[row - 1][col - 1] == '#' ? 1 : 0;

            int aboveRight = 0;
            if (row > 0 && col < state[0].Count - 1)
                aboveRight = state[row - 1][col + 1] == '#' ? 1 : 0;

            int below = 0;
            if (row < state.Count - 1)
                below = state[row + 1][col] == '#' ? 1 : 0;

            int belowLeft = 0;
            if (row < state.Count - 1 && col > 0)
                belowLeft = state[row + 1][col - 1] == '#' ? 1 : 0;

            int belowRight = 0;
            if (row < state.Count - 1 && col < state[0].Count - 1)
                belowRight = state[row + 1][col + 1] == '#' ? 1 : 0;

            int left = 0;
            if (col > 0)
                left = state[row][col - 1] == '#' ? 1 : 0;

            int right = 0;
            if (col < state[0].Count - 1)
                right = state[row][col + 1] == '#' ? 1 : 0;

            return above + aboveLeft + aboveRight + below + belowLeft + belowRight + left + right;
        }

        // Looks down an entire direction from a spot, and returns the status of the closest seat
        int lookDirection(List<List<char>> state, int row, int col, string direction)
        {
            if (direction == "above")
            {
                for (int i = 1; i <= row; i++)
                {
                    if (state[row - i][col] == 'L')
                        return 0;
                    else if (state[row - i][col] == '#')
                        return 1;
                    else
                        continue;
                }
                return 0;
            }
                
            if (direction == "aboveLeft")
            {
                for (int i = 1; i <= row && i <= col; i++)
                {
                    if (state[row - i][col - i] == 'L')
                        return 0;
                    else if (state[row - i][col - i] == '#')
                        return 1;
                    else
                        continue;
                }
                return 0;
            }

            if (direction == "aboveRight")
            {
                for (int i = 1; i <= row && (col + i) < state[0].Count; i++)
                {
                    if (state[row - i][col + i] == 'L')
                        return 0;
                    else if (state[row - i][col + i] == '#')
                        return 1;
                    else
                        continue;
                }
                return 0;
            }

            if (direction == "below")
            {
                for (int i = 1; (row + i) < state.Count; i++)
                {
                    if (state[row + i][col] == 'L')
                        return 0;
                    else if (state[row + i][col] == '#')
                        return 1;
                    else
                        continue;
                }
                return 0;
            }

            if (direction == "belowLeft")
            {
                for (int i = 1; (row + i) < state.Count && i <= col; i++)
                {
                    if (state[row + i][col - i] == 'L')
                        return 0;
                    else if (state[row + i][col - i] == '#')
                        return 1;
                    else
                        continue;
                }
                return 0;
            }

            if (direction == "belowRight")
            {
                for (int i = 1; (row + i) < state.Count && (col + i) < state[0].Count; i++)
                {
                    if (state[row + i][col + i] == 'L')
                        return 0;
                    else if (state[row + i][col + i] == '#')
                        return 1;
                    else
                        continue;
                }
                return 0;
            }

            if (direction == "left")
            {
                for (int i = 1; i <= col; i++)
                {
                    if (state[row][col - i] == 'L')
                        return 0;
                    else if (state[row][col - i] == '#')
                        return 1;
                    else
                        continue;
                }
                return 0;
            }

            if (direction == "right")
            {
                for (int i = 1; (col + i) < state[0].Count; i++)
                {
                    if (state[row][col + i] == 'L')
                        return 0;
                    else if (state[row][col + i] == '#')
                        return 1;
                    else
                        continue;
                }
                return 0;
            }

            return -1;
        }

        // Returns the number of occupied seats (not considering floors)
        int countVisibleNeighbors(List<List<char>> state, int row, int col)
        {
            int above = lookDirection(state, row, col, "above");
            int aboveLeft = lookDirection(state, row, col, "aboveLeft");
            int aboveRight = lookDirection(state, row, col, "aboveRight");
            int below = lookDirection(state, row, col, "below");
            int belowLeft = lookDirection(state, row, col, "belowLeft");
            int belowRight = lookDirection(state, row, col, "belowRight");
            int left = lookDirection(state, row, col, "left");
            int right = lookDirection(state, row, col, "right");
            return above + aboveLeft + aboveRight + below + belowLeft + belowRight + left + right;
        }

        // Returns the number of occupied seats in a given state
        int countOccupiedSeats(List<List<char>> state)
        {
            int occupied = 0;
            for (int i = 0; i < state.Count; i++)
                for (int j = 0; j < state[0].Count; j++)
                    if (state[i][j] == '#')
                        occupied++;
            return occupied;
        }

        // Counts the number of seats that remain occupied after the system stabilizes
        int stableSeats(int ruleset)
        {

            // Create a 2d char list (mutable) to keep track of the floor's state
            List<List<char>> state = new List<List<char>>();
            foreach (string row in lines)
            {
                List<char> currRow = new List<char>();

                foreach (char col in row)
                    currRow.Add(col);

                state.Add(currRow);
            }

            // Repeatedly advance the state until the system has stabilized
            bool done = false;
            while (!done)
            {
                // Represents the map after a state change
                char[,] map = new char[state.Count, state[0].Count];

                // Calculate the new map
                for (int i = 0; i < state.Count; i++)
                {
                    for (int j = 0; j < state[0].Count; j++)
                    {
                        // For the first question
                        if (ruleset == 1)
                        {
                            // First rule
                            if (state[i][j] == 'L' && countNeighbors(state, i, j) == 0)
                                map[i, j] = '#';
                            // Second rule
                            else if (state[i][j] == '#' && countNeighbors(state, i, j) >= 4)
                                map[i, j] = 'L';
                            else
                                map[i, j] = state[i][j];
                        }
                        // For the second question
                        else if (ruleset == 2)
                        {
                            // First rule
                            if (state[i][j] == 'L' && countVisibleNeighbors(state, i, j) == 0)
                                map[i, j] = '#';
                            // Second rule
                            else if (state[i][j] == '#' && countVisibleNeighbors(state, i, j) >= 5)
                                map[i, j] = 'L';
                            else
                                map[i, j] = state[i][j];
                        }
                    }
                }

                // See if the system has stabilized
                bool unstable = false;
                for (int i = 0; i < state.Count; i++)
                    for (int j = 0; j < state[0].Count; j++)
                        if (state[i][j] != map[i, j])
                            unstable = true;
                if (!unstable)
                    return countOccupiedSeats(state);

                /* testing to view states 
                int count1 = 0;
                int count2 = 0;
                foreach (List<char> row in state)
                {
                    count2 = 0;
                    foreach (char col in row)
                    {
                        Console.Write(col);
                        Console.Write(map[count1, count2] + " ");
                        count2++;
                    }
                    Console.WriteLine();
                    count1++;
                }
                Console.WriteLine();

                foreach (List<char> row in state)
                {
                    foreach (char col in row)
                    {
                        Console.Write(col);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                */

                // If the system is still unstable, copy it over to the state
                for (int i = 0; i < state.Count; i++)
                    for (int j = 0; j < state[0].Count; j++)
                        state[i][j] = map[i, j];
            }

            return -1;
        }

        answer1 = stableSeats(1);
        Console.WriteLine("Part 1: The number of occupied seats is " + answer1 + ".\n");
        answer2 = stableSeats(2);
        Console.WriteLine("Part 2: The number of occupied seats is " + answer2 + ".\n");

        return;
    }
}
