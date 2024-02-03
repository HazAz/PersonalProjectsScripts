using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {
	//cell has a cell number, 4 walls, floor, and a boolean isVisited
	//only run eller on a maze if the cell isn't visited
	public int cellNumber = -1;
	public GameObject NorthWall;
	public GameObject SouthWall;
	public GameObject EastWall;
	public GameObject WestWall;
	public GameObject floor;
	public bool isVisited = false;
}
