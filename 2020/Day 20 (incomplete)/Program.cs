using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

class Program
{
    // Tile objects hold a number and char[][]
    class Tile
    {
        private int num;
        private char[,] grid;
        private char[,] originalGrid;
        public Tile(int n, char[,] cg)
        {
            num = n;
            grid = cg;
            originalGrid = cg;
        }

        public int getNum()
        {
            return num;
        }

        public char[,] getGrid()
        {
            return grid;
        }

        public void resetGrid()
        {
            grid = originalGrid;
        }

        public void rotateRight() {
            int l = 10;

            // Making a copy of grid
            char[,] copyGrid = new char[l, l];
            for (int i = 0; i < l; i++)
                for (int j = 0; j < l; j++)
                    copyGrid[i, j] = grid[i, j];

            for (int i = 0; i < l; ++i)
                for (int j = 0; j < l; ++j)
                    grid[i, j] = copyGrid[l - j - 1, i];
        }

        public void flipHorizontal() {
            int l = 10;

            // Making a copy of grid
            char[,] copyGrid = new char[l, l];
            for (int i = 0; i < l; i++)
                for (int j = 0; j < l; j++)
                    copyGrid[i, j] = grid[i, j];

            for (int i = 0; i < l; ++i)
                for (int j = 0; j < l; ++j)
                    grid[i, j] = copyGrid[i, l - j - 1];
        }
        public void flipVertical() {
            int l = 10;

            // Making a copy of grid
            char[,] copyGrid = new char[l, l];
            for (int i = 0; i < l; i++)
                for (int j = 0; j < l; j++)
                    copyGrid[i, j] = grid[i, j];

            for (int i = 0; i < l; ++i)
                for (int j = 0; j < l; ++j)
                    grid[i, j] = copyGrid[l - i - 1, j];
        }

        // for testing
        public void printTile() {
            Console.WriteLine(num);
            int l = 10;
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    Console.Write(grid[i, j]);
                    Console.Write(" ");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }

        public bool checkRightFit(Tile second)
        {
            for (int i = 0; i < grid.Length; i++)
                if (grid[i, grid.Length] == second.getGrid()[i, 0])
                    return false;
            return true;
        }

        public bool checkLeftFit(Tile second)
        {
            for (int i = 0; i < grid.Length; i++)
                if (grid[i, 0] == second.getGrid()[i, grid.Length])
                    return false;
            return true;
        }

        public bool checkTopFit(Tile second)
        {
            for (int i = 0; i < grid.Length; i++)
                if (grid[0, i] == second.getGrid()[grid.Length, i])
                    return false;
            return true;
        }

        public bool checkBottomFit(Tile second)
        {
            for (int i = 0; i < grid.Length; i++)
                if (grid[grid.Length, i] == second.getGrid()[0, i])
                    return false;
            return true;
        }

        public bool checkSurroundingFits(List<Tuple<Tile, bool>> surroundings)
        {
            bool leftFit = true;
            bool rightFit = true;
            bool topFit = true;
            bool bottomFit = true;

            if (surroundings[0].Item2)
                leftFit = checkLeftFit(surroundings[0].Item1);
            if (surroundings[1].Item2)
                rightFit = checkRightFit(surroundings[1].Item1);
            if (surroundings[2].Item2)
                topFit = checkTopFit(surroundings[2].Item1);
            if (surroundings[3].Item2)
                bottomFit = checkBottomFit(surroundings[3].Item1);

            return (leftFit && rightFit && topFit && bottomFit);
        }

        public void tryConfigurations(int n)
        {
            switch(n) // Hopefully these cover all the cases
            {
                case 1:
                    flipHorizontal(); break;
                case 2:
                    flipVertical(); break;
                case 3:
                    rotateRight(); break;
                case 4:
                    rotateRight(); flipHorizontal(); break;
                case 5:
                    rotateRight(); flipVertical(); break;
                case 6:
                    rotateRight(); rotateRight(); break;
                case 7:
                    rotateRight(); rotateRight(); flipHorizontal(); break;
                case 8:
                    rotateRight(); rotateRight(); flipVertical(); break;
                case 9:
                    rotateRight(); rotateRight(); rotateRight(); break;
                case 10:
                    rotateRight(); rotateRight(); rotateRight(); flipHorizontal(); break;
                case 11:
                    rotateRight(); rotateRight(); rotateRight(); flipVertical(); break;
            }
        }
    }

    // 
    static void Main(string[] args)
    {
        // input.txt is in my debug\net5.0 folder
        string path = @"input.txt";

        // There are two integer answers
        int answer1;
        int answer2;

        // First we'll dump the text into a single string, then split it by blank lines
        string s = File.ReadAllText(path, Encoding.UTF8);
        string[] entries = s.Split(new string[] { "\n\n", ":" }, StringSplitOptions.RemoveEmptyEntries);

        // Now we'll load them into a list of Tile objects
        List<Tile> tileList = new List<Tile>();
        for (int i = 0; i < entries.Length; i += 2)
        {
            int n = Int32.Parse(entries[i].Substring(5, 4));
            string[] sa = entries[i + 1].Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            char[,] tileMap = new char[sa.Length, sa.Length];
            for (int j = 0; j < sa.Length; j++)
            {
                string line = sa[j];
                for (int k = 0; k < line.Length; k++)
                {
                    tileMap[j, k] = line[k];
                }
            }
            tileList.Add(new Tile(n, tileMap));
        }

        // Find the product of the four corner pieces of a completed puzzle
        int part1()
        {
            // Initialize the puzzle tile array
            int dim = tileList.Count / tileList.Count;
            Tile[,] puzzle = new Tile[dim, dim];

            // Create an empty tile to use for blank spots and beyond the boundary
            char[,] emptyGrid = new char[0, 0];
            Tile empty = new Tile(0, emptyGrid);

            bool completePuzzle = false;

            while (!completePuzzle)
            {
                // We're gonna start with the top left piece
                // We'll have to try every single configuration
                foreach (Tile tile in tileList)
                {
                    // Place the piece
                    puzzle[0, 0] = tile;

                    // Continually place the next one
                }
            }

            

        }
        answer1 = part1();
        Console.WriteLine("Part 1: The value of the accumulator is " + answer1 + ".\n");

        // 
        int part2()
        {
            return 0;
        }
        answer2 = part2();
        Console.WriteLine("Part 2: The accumulator of the successful program is " + answer2 + ".\n");

        return;
    }
}
