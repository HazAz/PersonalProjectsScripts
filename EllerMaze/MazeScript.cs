using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MazeScript : MonoBehaviour {
	//variables I use later
	private int start = 0;
	public GameObject cellPrefab;
	private Vector3 initialPos;
	private GameObject playerObject;
	private Vector3 playerPos;
	public int end;
	public Text winText;
	private int counter;

	public int rowCounter;
	public GameObject[] cells = new GameObject[8];

	// Use this for initialization
	void Start () {
		playerObject = GameObject.Find("Player");
		rowCounter = 0;
		start = 0;
		end = 0;
		winText.text = "";
		counter = 8;

		//instantiated it here because it helped fixed errors later
		cells[0] = Instantiate(cellPrefab, new Vector3(-100, -100, -100), new Quaternion());
		cells[0].GetComponent<MazeCell>().isVisited = true;
	}
	
	//creates the first row
	void CreateFirstRow(){
		initialPos = new Vector3(14.5f, -7.6f, -27f);
		cells = new GameObject[8];

		//Creating row
		for(int i = 0; i < 8; i++){
			cells[i] = Instantiate(cellPrefab, new Vector3(initialPos.x + 10*rowCounter, -7.6f, initialPos.z+i*10), new Quaternion());
			cells[i].GetComponent<MazeCell>().cellNumber = i;
			cells[i].GetComponent<MazeCell>().isVisited = false;
		}

		//setting the walls on the sides
		cells[7].GetComponent<MazeCell>().WestWall.SetActive(false);
	
		//assigning numbers
		for(int i = 1; i< 8; i++){
			if(Random.value < 0.5){
				cells[i].GetComponent<MazeCell>().cellNumber = cells[i-1].GetComponent<MazeCell>().cellNumber;
				cells[i-1].GetComponent<MazeCell>().WestWall.SetActive(false);
			}
			else{
				cells[i-1].GetComponent<MazeCell>().WestWall.SetActive(true);
			}
		}

		//opening the walls to enter the maze
		cells[2].GetComponent<MazeCell>().SouthWall.SetActive(false);
		cells[5].GetComponent<MazeCell>().SouthWall.SetActive(false);


		//taking care of north/south walls
		if(cells[0].GetComponent<MazeCell>().cellNumber != cells[1].GetComponent<MazeCell>().cellNumber)
		{
			cells[0].GetComponent<MazeCell>().NorthWall.SetActive(false);
		}
		if(cells[7].GetComponent<MazeCell>().cellNumber != cells[6].GetComponent<MazeCell>().cellNumber)
		{
			cells[7].GetComponent<MazeCell>().NorthWall.SetActive(false);
		}

		//randomly opening northwalls
		for(int i = 1; i <7; i++){
			if(cells[i].GetComponent<MazeCell>().cellNumber != cells[i+1].GetComponent<MazeCell>().cellNumber && cells[i].GetComponent<MazeCell>().cellNumber != cells[i-1].GetComponent<MazeCell>().cellNumber){
				cells[i].GetComponent<MazeCell>().NorthWall.SetActive(false);
			}
		}
		
		for(int i = 0; i <7; i++){
			if(cells[i].GetComponent<MazeCell>().cellNumber == cells[i+1].GetComponent<MazeCell>().cellNumber){
				if(Random.value>0.4){
					cells[i].GetComponent<MazeCell>().NorthWall.SetActive(false);
					cells[i+1].GetComponent<MazeCell>().NorthWall.SetActive(true);
				}
				else{
					cells[i+1].GetComponent<MazeCell>().NorthWall.SetActive(false);
					cells[i].GetComponent<MazeCell>().NorthWall.SetActive(true);
				}
			}
		}
	}

	//the function the runs Eller's algorithm
	void Eller(GameObject[] current){
		rowCounter++;
		GameObject[] newCells = new GameObject[8];

		//initiate cell. If previous cell has no north wall, then this cell should have no south wall and give them same cellnumber
		for(int i = 0; i < 8; i++){
			newCells[i] = Instantiate(cellPrefab, new Vector3(initialPos.x + 10*rowCounter, -7.6f, initialPos.z+i*10), new Quaternion());
			newCells[i].GetComponent<MazeCell>().isVisited = false;
			newCells[i].GetComponent<MazeCell>().SouthWall.SetActive(false);
			if(current[i].GetComponent<MazeCell>().NorthWall.activeSelf == false){
				newCells[i].GetComponent<MazeCell>().cellNumber = current[i].GetComponent<MazeCell>().cellNumber;
			}
		}
		//assign cell numbers to the rest
		for(int i = 0; i < 8; i++){
			if(newCells[i].GetComponent<MazeCell>().cellNumber==-1){
				newCells[i].GetComponent<MazeCell>().cellNumber = i+counter;
			}
		}
		counter = counter+8;

		newCells[7].GetComponent<MazeCell>().WestWall.SetActive(false);

		//open some together randomly
		for(int i = 0; i< 7; i++){
			//if they have the same set number, create a wall between them (or else this will create loops)
			if(newCells[i].GetComponent<MazeCell>().cellNumber == newCells[i+1].GetComponent<MazeCell>().cellNumber){
				newCells[i].GetComponent<MazeCell>().WestWall.SetActive(true);
			}
			else {
				if(Random.value < 0.5){
					//either add to set and remove wall between them
					newCells[i+1].GetComponent<MazeCell>().cellNumber = newCells[i].GetComponent<MazeCell>().cellNumber;
					newCells[i].GetComponent<MazeCell>().WestWall.SetActive(false);
				}
				else{
					//or add a wall between them
					newCells[i].GetComponent<MazeCell>().WestWall.SetActive(true);
				}
			}
		}

		//taking care of north/south walls
		if(newCells[0].GetComponent<MazeCell>().cellNumber != newCells[1].GetComponent<MazeCell>().cellNumber)
		{
			newCells[0].GetComponent<MazeCell>().NorthWall.SetActive(false);
		}
		if(newCells[7].GetComponent<MazeCell>().cellNumber != newCells[6].GetComponent<MazeCell>().cellNumber)
		{
			newCells[7].GetComponent<MazeCell>().NorthWall.SetActive(false);
		}
		for(int i = 1; i <7; i++){
			if(newCells[i].GetComponent<MazeCell>().cellNumber != newCells[i+1].GetComponent<MazeCell>().cellNumber && newCells[i].GetComponent<MazeCell>().cellNumber != newCells[i-1].GetComponent<MazeCell>().cellNumber){
				newCells[i].GetComponent<MazeCell>().NorthWall.SetActive(false);
			}
		}

		for(int i = 0; i <7; i++){
			if(newCells[i].GetComponent<MazeCell>().cellNumber == newCells[i+1].GetComponent<MazeCell>().cellNumber){
				if(Random.value > 0.4){
					newCells[i].GetComponent<MazeCell>().NorthWall.SetActive(false);
					newCells[i+1].GetComponent<MazeCell>().NorthWall.SetActive(true);
				}
				else{
					newCells[i+1].GetComponent<MazeCell>().NorthWall.SetActive(false);
					newCells[i].GetComponent<MazeCell>().NorthWall.SetActive(true);
				}
			}
		}
		
		//setting cells to the newCells
		for(int i =0; i < 8; i++){
			cells[i] = newCells[i];
		}
	}

	// Update is called once per frame
	void Update(){
		//create first row once only when player reaches certain position
		if(start == 0 && playerObject.transform.position.x > 48 && playerObject.transform.position.z > 40 && playerObject.transform.position.z < 50)
		{ 	
			start = 1;
			CreateFirstRow();
		}
		//if it's not the end and player reaches end of current maze, run Eller
		if(end ==0 && cells[0] && !cells[0].GetComponent<MazeCell>().isVisited && playerObject.transform.position.x > 48+10*(rowCounter+1))
		{
			cells[0].GetComponent<MazeCell>().isVisited = true;
			Eller(cells);
		}
		//if it is the end, set north wall to true, change it's health so that it takes a while to destroy
		if(end!=0){
			for(int i=0; i<7; i++){
				cells[i].GetComponent<MazeCell>().NorthWall.SetActive(true);
				if(cells[i].GetComponent<MazeCell>().cellNumber != cells[i+1].GetComponent<MazeCell>().cellNumber){
					cells[i].GetComponent<MazeCell>().WestWall.SetActive(false);
				}
				cells[i].GetComponent<MazeCell>().NorthWall.GetComponent<wallScript>().damage = -10000;
			}
			cells[7].GetComponent<MazeCell>().NorthWall.SetActive(true);
			cells[7].GetComponent<MazeCell>().NorthWall.GetComponent<wallScript>().damage = -10000;
		}

		//if player reaches the end
		if(playerObject.transform.position.x < 48 && playerObject.transform.position.z > 10 && playerObject.transform.position.z < 20)
		{ 	
			winText.text = "Y  O  U   W  I  N  !  !  !";
			playerObject.GetComponent<Player>().canMove = false; 
		}
	}
}
