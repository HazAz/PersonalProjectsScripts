using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeLoader : MonoBehaviour
{
    public int mazeRows, mazeColumns;
    public GameObject wall;
    public float size = 5f;
    private float offset;
    public bool isFar;
    private MazeCell[,] mazeCells;

    // Start is called before the first frame update
    void Start()
    {
        offset = (isFar) ? 100f : 0;
        InitializeMaze();
        MazeAlgorithm ma = new HuntAndKillAlgorithm(mazeCells);
        ma.CreateMaze();
    }

    // Update is called once per frame

    private void InitializeMaze()
    {
        mazeCells = new MazeCell[mazeRows, mazeColumns];
        for (int r =0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                mazeCells[r, c] = new MazeCell();

               
                mazeCells[r, c].floor = Instantiate(wall, new Vector3(r * size + offset, -(size / 2f), c * size + offset), Quaternion.identity);
                mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);

                if (c==0)
                {
                    mazeCells[r, c].westWall = Instantiate(wall, new Vector3(r * size + offset, 0, (c * size) - (size / 2f) + offset), Quaternion.identity);
                }
                mazeCells[r, c].eastWall = Instantiate(wall, new Vector3(r * size + offset, 0, (c * size) + (size / 2f) + offset), Quaternion.identity);
                var eastRenderer = mazeCells[r, c].eastWall.GetComponent<Renderer>();
                eastRenderer.material.SetColor("_Color", Color.blue);

                if (r == 0)
                {
                    mazeCells[r, c].northWall = Instantiate(wall, new Vector3((r*size) - (size/2f) + offset, 0, c * size + offset), Quaternion.identity);
                    mazeCells[r, c].northWall.transform.Rotate(Vector3.up * 90f);
                }
                mazeCells[r, c].southWall = Instantiate(wall, new Vector3((r * size) + (size / 2f) + offset, 0, c * size + offset), Quaternion.identity);
                mazeCells[r, c].southWall.transform.Rotate(Vector3.up * 90f);
                var southRenderer = mazeCells[r, c].southWall.GetComponent<Renderer>();
                southRenderer.material.SetColor("_Color", Color.blue);
            }
        }

    }
}
