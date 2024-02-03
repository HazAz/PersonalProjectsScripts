﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntAndKillAlgorithm : MazeAlgorithm
{
	private int currentRow = 0;
	private int currentColumn = 0;
	private bool courseComplete = false;
	
	public HuntAndKillAlgorithm(MazeCell[,] mazeCells) : base(mazeCells)
	{
	}

	public override void CreateMaze()
	{
		HuntAndKill();
	}

	private void HuntAndKill()
	{
		mazeCells[currentRow, currentColumn].isVisited = true;
		while (!courseComplete)
		{
			Kill();
			Hunt();
		}
	}

	private bool RouteStillAvailable(int row, int column)
	{
		int availableRoutes = 0;
		if (row > 0 && !mazeCells[row-1, column].isVisited)
		{
			availableRoutes++;
		}
		if (row < mazeRows - 1 && !mazeCells[row+1, column].isVisited)
		{
			availableRoutes++;
		}
		if (column > 0 && !mazeCells[row,column-1].isVisited)
		{
			availableRoutes++;
		}
		if (column < mazeColumns-1 && !mazeCells[row, column + 1].isVisited)
		{
			availableRoutes++;
		}
		return availableRoutes > 1;
	}

	private void Kill()
	{
		while (RouteStillAvailable(currentRow, currentColumn))
		{
			int direction = (int)Random.Range(1, 4);
			if (direction == 1 && CellIsAvailable(currentRow-1, currentColumn))
			{
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].northWall);
				DestroyWallIfItExists(mazeCells[currentRow - 1, currentColumn].southWall);
				currentRow--;
			}
			else if (direction == 2 && CellIsAvailable(currentRow + 1, currentColumn))
			{
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].southWall);
				DestroyWallIfItExists(mazeCells[currentRow + 1, currentColumn].northWall);
				currentRow++;
			}
			else if (direction == 3 && CellIsAvailable(currentRow, currentColumn + 1))
			{
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].eastWall);
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn + 1].westWall);
				currentColumn++;
			}
			else if (direction == 4 && CellIsAvailable(currentRow, currentColumn - 1))
			{
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].westWall);
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn - 1].eastWall);
				currentColumn--;
			}
			mazeCells[currentRow, currentColumn].isVisited = true;
		}
	}

	private void Hunt()
	{
		courseComplete = true;

		for (int r = 0; r < mazeRows; r++)
		{
			for (int c = 0; c < mazeColumns; c++)
			{
				if (!mazeCells [r,c].isVisited && CellHasAnAdjacentVisitedCell(r,c))
				{
					courseComplete = false;
					currentRow = r;
					currentColumn = c;
					DestroyAdjacentWall(currentRow, currentColumn);
					mazeCells[currentRow, currentColumn].isVisited = true;
					return;
				}
			}
		}

	}

	private bool CellIsAvailable(int row, int column)
	{
		if (row >=0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells[row, column].isVisited)
		{
			return true;
		}
		return false;
	}

	private void DestroyWallIfItExists(GameObject wall)
	{
		if (wall != null)
		{
			GameObject.Destroy(wall);
		}
	}

	private bool CellHasAnAdjacentVisitedCell(int row, int column)
	{
		int visitedCells = 0;
		if (row > 0 && mazeCells[row - 1, column].isVisited)
		{
			visitedCells++;
		}
		if (row < (mazeRows-2) && mazeCells[row + 1, column].isVisited)
		{
			visitedCells++;
		}
		if (column > 0 && mazeCells[row , column - 1].isVisited)
		{
			visitedCells++;
		}
		if (column < (mazeRows - 2) && mazeCells[row, column + 1].isVisited)
		{
			visitedCells++;
		}
		return visitedCells > 0;
	}

	private void DestroyAdjacentWall(int row, int column)
	{
		bool wallDestroyed = false;
		while (!wallDestroyed)
		{
			int direction = (int)Random.Range(1, 4);
			if (direction == 1 && row > 0 && mazeCells[row -1, column].isVisited)
			{
				DestroyWallIfItExists(mazeCells[row, column].northWall);
				DestroyWallIfItExists(mazeCells[row - 1, column].southWall);
				wallDestroyed = true;
			}
			else if (direction == 2 && row < (mazeRows-2) && mazeCells[row + 1, column].isVisited)
			{
				DestroyWallIfItExists(mazeCells[row, column].southWall);
				DestroyWallIfItExists(mazeCells[row + 1, column].northWall);
				wallDestroyed = true;
			}
			else if (direction == 3 && column > 0 && mazeCells[row, column -1 ].isVisited)
			{
				DestroyWallIfItExists(mazeCells[row, column].westWall);
				DestroyWallIfItExists(mazeCells[row, column-1].eastWall);
				wallDestroyed = true;
			}
			else if (direction == 4 && column < (mazeColumns - 2) && mazeCells[row, column+1].isVisited)
			{
				DestroyWallIfItExists(mazeCells[row, column].eastWall);
				DestroyWallIfItExists(mazeCells[row, column+1].westWall);
				wallDestroyed = true;
			}
		}
	}
}
