using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public bool isVisited = false;
    public GameObject northWall, southWall, eastWall, westWall, floor;
}
