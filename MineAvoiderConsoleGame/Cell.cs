using System;

public class Cell
{
	private int col;
	private int row;		
	private int status;
	// '-1' mine
	// '0' - '8' surrounding mines
	private bool reveal;
	private bool flag;

	public Cell(int col, int row, int status)
	{
		this.col = col;
		this.row = row;				
		this.status = status;
		this.reveal = false;
		flag = false;
	}
	public int getCol()
	{
		return col;
	}

	public void setCol(int c)
	{
		this.row = c;
	}

	public int getRow()
    {
		return row;
    }

	public void setRow(int r)
	{
		this.row = r;
	}

	public int getStatus()
    {
		return status;
    }

	public void setStatus(int s)
    {
		this.status = s;
    }

	public bool checkReveal()
    {
		return reveal;
    }

	public void toggleReveal(bool guessed)
    {
		this.reveal = guessed;
    }
	public bool checkFlag()
	{
		return flag;
	}

	public void toggleFlag(bool flagged)
	{
		this.flag = flagged;
	}
}
