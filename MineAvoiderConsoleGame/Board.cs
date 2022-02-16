using System;

public class Board
{
    private Cell[,] gameboard;
    private int turns = 0, guesses = 0, rats = 1, mines, freesquares;
    private bool explosion = false, autoend = true;

    public bool wasExplosion()
    {
        return explosion;
    }

    public bool checkAutoEnd()
    {
        return this.autoend;
    }

    public void setAutoEnd(bool autoend)
    {
        this.autoend = autoend;
    }

    public int getNumMines()
    {
        return mines;
    }

    public void setNumMines(int num)
    {
        this.mines = num;
    }

    public void removeMine()
    {
        this.mines--;
    }

    public int getFreeSquares()
    {
        return freesquares;
    }

    public void removeFreeSquare()
    {
        this.freesquares--;
    }

    public int getRowLength()
    {
        return gameboard.GetLength(0);
    }

    public int getColumnLength()
    {
        return gameboard.GetLength(1);
    }

    public int getTurns()
    {
        return turns;
    }

    public void addTurn()
    {
        this.turns++;
    }

    public int getGuesses()
    {
        return guesses;
    }

    public void addGuess()
    {
        this.guesses++;
    }

    public int getRats()
    {
        return rats;
    }

    public void useRat()
    {
        this.rats--;
    }
    public void setRats(int d)
    {
        this.rats = d;
    }

    public int getStatus(int col, int row)
    {
        return this.gameboard[col, row].getStatus();
    }

    public void setStatus(int col, int row, int status)
    {
        this.gameboard[col, row].setStatus(status);
    }

    public bool checkReveal(int col, int row)
    {
        return this.gameboard[col, row].checkReveal();
    }

    public void toggleReveal(int col, int row, bool guessed)
    {
        this.gameboard[col, row].toggleReveal(guessed);
    }

    public bool checkFlag(int col, int row)
    {
        return this.gameboard[col, row].checkFlag();
    }

    public bool validSquare(int col, int row)
    {
        if ((col >= 0) && (row >= 0) && (col < getRowLength()) && (row < getColumnLength()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Board(int cols, int rows)
    {
        this.gameboard = new Cell[cols, rows];
        for (int col = 0; col < cols; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                this.gameboard[col, row] = new Cell(col, row, '-');
            }
        }

        if (rows == 3 || cols == 3)
        {
            this.mines = 2;
        }
        else if ((3 < rows && rows <= 4) || (3 < cols && cols <= 4))
        {
            this.mines = 4;
        }
        else if ((4 < rows && rows <= 6) || (4 < cols && cols <= 6))
        {
            this.mines = 6;
        }
        else if ((6 < rows && rows <= 8) || (6 < cols && cols <= 8))
        {
            this.mines = 8;
        }
        else if ((8 < rows && rows <= 10) || (8 < cols && cols <= 10))
        {
            this.mines = 10;
        }

        this.freesquares = (rows * cols) - this.mines;
    }

    public void placeMines()
    {
        Random rand = new Random();
        int col, row;

        int toPlace = mines;

        while (toPlace != 0)
        {
            col = rand.Next(0, getRowLength());
            row = rand.Next(0, getColumnLength());            

            if (isSafe(col, row)) 
            {
               setStatus(col, row, -1);
                toPlace--;
            }
        }

        for (row = 0; row < getColumnLength(); row++)
        {
            for (col = 0; col < getRowLength(); col++)
            {
                if (isSafe(col, row))
                {
                    setStatus(col, row, detectMines(col, row));
                }
                else
                {
                    continue;
                }
            }
        }
    }

    public int detectMines(int col, int row)
    {
        int surroundingmines = 0;
        
        for (int i = col - 1; i <= col + 1; i++)
        {
            for (int j = row - 1; j <= row + 1; j++)
            {
                surroundingmines += detectionHelper(i, j);
            }
        }

        return surroundingmines;
    }

    public int detectionHelper(int col, int row)
    {
        if (!validSquare(col, row))
        {
            return 0;
        }
        else if (!isSafe(col, row))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public Boolean isSafe(int col, int row)
    {
        if (getStatus(col, row) < 0) { 
            return false; 
        }
        else { 
            return true; 
        }
    }

    public void guess(int col, int row)
    {
        addGuess();
        if (!validSquare(col, row))
        {
            Console.WriteLine($"\n[{col},{row}] is out of bounds.\nTurn skipped as penalty");
            addTurn();
        }
        else if (checkReveal(col, row))
        {
            Console.WriteLine($"\n[{col},{row}] is a previously selected target.\nTurn skipped as penalty.");
            addTurn();
        }
        else
        {
            toggleReveal(col, row, true);
            if (!isSafe(col, row))
            {
                removeMine();                
                this.explosion = true;
                Console.WriteLine("\nBOOM!");
            }
            else
            {
                removeFreeSquare();
                Console.WriteLine($"\n{getStatus(col, row)} mine(s) in the surrounding area.");
            }
        }
    }

    public void toggleFlag(int col, int row)
    {
        if (!validSquare(col, row))
        {
            Console.WriteLine($"\n[{col},{row}] is out of bounds.\nNo flag placed.");
        }
        else if (checkFlag(col, row)) {
            this.gameboard[col, row].toggleFlag(false);
            Console.WriteLine($"\nFlag removed from square [{col},{row}].");
        }
        else
        {
            this.gameboard[col, row].toggleFlag(true);
            Console.WriteLine($"\nFlag placed on square [{col},{row}].");
        }
    }

    public void rat(int direction, int index)
    {
        useRat();
        int spots = 0;
        string dir = "Unspecified";
        if (direction == 0)
        {
            dir = "row";
            for (int i = 0; i < getRowLength(); i++)
            {
                if (!isSafe(i, index))
                {
                    spots++;
                }
            }
        }
        else if (direction ==1)
        {
            dir = "column";
            for (int i = 0; i < getColumnLength(); i++)
            {
                if (!isSafe(index, i))
                {
                    spots++;
                }
            }
        }
        Console.WriteLine($"\nRat has sniffed {spots} mine(s) in {dir} {index}.");
    }

    public void display()
    {
        string output = "\n  ";
        string square = " ";
        for (int collabel = 0; collabel < getRowLength(); collabel++)
        {
            output += " " + collabel + " ";
        }
        output += "\n";
        for (int row = 0; row < getColumnLength(); row++)
        {
            for (int col = 0; col < getRowLength(); col++)
            {
                if (checkReveal(col, row))
                {
                    if (!isSafe(col, row))
                    {
                        square = "X";
                    }
                    else
                    {
                        square = $"{getStatus(col, row)}";
                    }
                }
                else if (checkFlag(col, row))
                {
                    square = "F";
                }
                else
                {
                    square = " ";
                }
                if (col == 0)
                {
                    output += row + " [" + square + "]";
                }
                else
                {
                    output += "[" + square + "]";
                }

            }
            output += "\n";
        }
        Console.WriteLine(output);
    }

    public void print()
    {
        string output = "\n  ";
        string square = " ";
        for (int collabel = 0; collabel < getRowLength(); collabel++)
        {
            output += " " + collabel + " ";
        }
        output += "\n";
        for (int row = 0; row < getColumnLength(); row++)
        {
            for (int col = 0; col < getRowLength(); col++)
            {
                if (!isSafe(col, row))
                {
                    square = "X";
                }
                else
                {
                    square = $"{getStatus(col, row)}";
                }
                if (col == 0)
                {
                    output += row + " [" + square + "]";
                }
                else
                {
                    output += "[" + square + "]";
                }
            }
            output += "\n";
        }
        Console.WriteLine(output);
    }

    //public static void Main(string[] args)
    //{
    //    Board gameplay = new Board(10, 10);
    //    gameplay.placeMines();
    //    gameplay.print();

    //    Random rand = new Random();
    //    int col, row;
    //    for (int i = 0; i < 25; i++)
    //    {
    //        gameplay.display();
    //        col = rand.Next(0, gameplay.getRowLength());
    //        row = rand.Next(0, gameplay.getColumnLength());
    //        gameplay.toggleFlag(col, row);
    //        col = rand.Next(0, gameplay.getRowLength());
    //        row = rand.Next(0, gameplay.getColumnLength());
    //        gameplay.guess(col, row);
    //    }

    //    gameplay.print();
        
    //}

}
