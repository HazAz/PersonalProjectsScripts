using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeAlgorithm //: MonoBehaviour
{
    protected MazeCell[,] mazeCells;
    protected int mazeRows, mazeColumns;
    // Start is called before the first frame update
    protected MazeAlgorithm(MazeCell[,] mazeCells) //: base())
    {
        this.mazeCells = mazeCells;
        mazeRows = mazeCells.GetLength(0);
        mazeColumns = mazeCells.GetLength(1);
    }
    public abstract void CreateMaze();
}
