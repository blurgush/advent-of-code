using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;

// We're going to make a class for seats
public class Seat
{
    private int row, col, id;
    public Seat(int r, int c, int i)
    {
        row = r;
        col = c;
        id = i;
    }

    public int getRow() { return row; }
    public int getCol() { return col; }
    public int getId() { return id; }
}

class Program
{
    // Read the input strings and create Seat objects based on their composition
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // First we'll dump the text into a single string, then split it by lines
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] entries = s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Initialize a list of seat objects
        List<Seat> seats = new List<Seat>();

        // Make Seat objects for each member of entries
        for (int i = 0; i < entries.Length; i++)
        {
            int row;
            int col;
            int id;

            // Seat rows are between 0 and 127
            List<int> rowVals = new List<int>();
            for (int j = 0; j < 128; j++)
                rowVals.Add(j);

            // Reduce the set of valid seat rows based on the first seven chars
            for (int j = 0; j < 7; j++)
            {
                int half = rowVals.Count / 2;
                if (entries[i][j] == 'F')   
                    rowVals.RemoveRange(half, half);    // Keep the first half
                else                        
                    rowVals.RemoveRange(0, half);       // Keep the second half
            }
            row = rowVals[0];

            // Seat columns are between 0 and 7
            List<int> colVals = new List<int>();
            for (int j = 0; j < 8; j++)
                colVals.Add(j);

            // Reduce the set of valid seat cols based on the last three chars
            for (int j = 7; j < 10; j++)
            {
                int half = colVals.Count / 2;
                if (entries[i][j] == 'R')
                    colVals.RemoveRange(0, half);       // Keep the second half
                else
                    colVals.RemoveRange(half, half);    // Keep the first half
            }
            col = colVals[0];

            // The seat ID is calculated using the row and column
            id = row * 8 + col;

            // Make the seat object and put it in the list
            Seat seat = new Seat(row, col, id);
            seats.Add(seat);
        }

        // Find the highest seat ID on a boarding pass
        int part1()
        {
            // Make an int array of the seat IDs
            int[] seatIds = new int[seats.Count];
            for (int i = 0; i < seatIds.Length; i++)
            {
                seatIds[i] = seats[i].getId();
            }

            return seatIds.Max();
        }
        answer1 = part1();
        Console.WriteLine("Part 1: The highest seat ID is " + answer1 + "\n");

        // Find the missing ID of your seat, with the given conditions (not at the front or back)
        int part2()
        {
            // First we want a sorted list of seat IDs
            List<int> seatIds = new List<int>();
            foreach(Seat seat in seats)
            {
                seatIds.Add(seat.getId());
            }
            seatIds.Sort();

            // Traverse the list of seats IDs and make a list of seat IDs that are "missing"
            List<int> missingIds = new List<int>();
            int currId = seatIds[0];
            for (int i = 1; i < seatIds.Count; i++)
            {
                // If the next seatId is more than +1 there's a missing ID
                if (seatIds[i] - 1 != currId)
                    missingIds.Add(currId + 1);

                currId = seatIds[i];
            }

            // One of the missing IDs is ours
            // There should be one ID that is not at the front or back (AKA row is 0 or 127)
            // So we need to do some backwards math on the IDs

            // If the row is 0 or 127, the ID calculation will look like 
            // (0) * 8 + col        = 0 + col           = a number in between 0 and 7
            // (127) * 8 + col      = 1016 + col        = a number in between 1016 and 1023

            // So if we have a number that isn't in these ranges, it's ours
            // Here goes nothing
            foreach (int id in missingIds)
            {
                if ((id > 7 && id < 1016) || id > 1023)
                        return id;
            }

            return 0;
        }
        answer2 = part2();
        Console.WriteLine("Part 2: Your seat ID is " + answer2 + "\n");

        return;
    }
}
