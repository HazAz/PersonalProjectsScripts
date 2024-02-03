using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//THIS IS THE CLASS THE RUNS THE HTN, IT'S A BIT CONFUSING SO I'LL EXPLAIN THINGS HERE
//i CREATED A TREE, AND EACH NODE HAS PRECONDITIONS
//MY EvalNode() FUNCTION CHECKS PRECONDITIONS, AND IF THEY'RE MET, IT EITHER EXECUTES THE TASK OR PUSHES IT IN A STACK AND RETURNS TRUE
//IF CONDITIONS NOT MET, RETURNS FALSE
//THEN I RUN DFS(NODE n) WHICH DOES DEPTH FIRST SEARCH ON THE TREE AND EVALUATES THE NODES, HENCE ADDING SOME TO THE STACK
//IF THE STACK IS NOT EMPTY, I POP THE COMMAND AND EXECUTE IT
//EXECUTE() CARRIES OUT THE FUNCTIONS

public class tree : MonoBehaviour {

	public NavMeshAgent agent;

	//Worldstate
	public GameObject[] hasPickable;
	public Vector3[] alcove;
	public GameObject player;
	public GameObject patrol1;
	public GameObject patrol2;


	//NODES
	private Node moveGrab = new Node("moveGrab");
	private Node moveAvoid = new Node("moveAvoid");
	private Node idle = new Node("idle");
	private Node teleport = new Node("tele");
	private Node win;
	private Node avoid;
	
	//making sure I dont override the stack
	private bool isExecuting;
	
	public GameObject spawnClass;
	
	//number of teleports left by the AI
	private int numTele;
	
	//STACK
	public static Stack<string> stack = new Stack<string>();
	

	// Use this for initialization
	void Start () {
		numTele = 2;

		isExecuting = false;

		//ASSIGNING NODES
		avoid = new Node("avoid", idle, moveAvoid);
		win = new Node("win", teleport, avoid, moveGrab);
		

		//PLACING NODES
		win.SetLeft(teleport);
		win.SetMid(avoid);
		win.SetRight(moveGrab);
		avoid.SetLeft(idle);
		avoid.SetMid(moveAvoid);
		StartCoroutine(FirstStep());
	}

	//waiting half a second before running the first DFS, to give the player a chance to prepare
	IEnumerator FirstStep()
	{
		yield return new WaitForSeconds(0.5f);
		DFS(win);
	}
	
	// Update is called once per frame
	void Update () {
		//while stack is not empty, pop the first command and execute it
		while(stack.Count != 0){
			Execute(stack.Pop());
		}

		if(isExecuting == false){
			//if executing is false, start executing, then run dfs, fill up the stack, pop it and execute.
			isExecuting = true;
			DFS(win);
			while(stack.Count!= 0){
				Debug.Log(stack.Peek());
				Execute(stack.Pop());
			}
		}
		//set isExecuting to false to signal the end of the run, and repeat the process (backtracking)
		isExecuting = false;
	}

	//CHECKS PRECONDITIONS
	public bool EvalNode(Node n){

		//no preconditions for moveAvoid. If it says moveAvoid, get out of there!
		if(n.GetTask().Equals("moveAvoid")){
			stack.Push("moveAvoid");
			return true;
		}

		//if the command is teleport, just do it here. No point in adding in a stack, recomputing which object was the closer and then teleporting it.
		//Safer
		else if(n.GetTask().Equals("tele")){
			if(numTele > 0){
				bool patrol1Close = TooClose(patrol1);
				bool patrol2Close = TooClose(patrol2);
				bool playerClose = TooClose(player);
				
				//if patrol is too close, respawn it, and decrement the number of teleports
				if(patrol1Close){
					patrol1.GetComponent<patrolScript>().Respawn();
					numTele--;
					return true;
				}
				else if(patrol2Close){
					patrol2.GetComponent<patrolScript>().Respawn();
					numTele--;
					return true;
				}
				//if player is too close, make him spawn at a random alcove
				else if(playerClose){
					int r = (int) Random.Range(0f, 9.99f);
					spawnClass.GetComponent<spawn>().Spawn(player, r);
					numTele--;
					return true;
				}
				else{
					return false;
				}
			}
			return false;
		}

		//if the task it idle, and if the AI is already in an alcove, push the idle command into the stack
		else if(n.GetTask().Equals("idle")){
			for(int i = 0; i < 10; i++){
				if(transform.position.x == alcove[i].x && transform.position.z == alcove[i].z){
					stack.Push("idle");
					return true;
				}
			}
			return false;
		}

		//win has no preconditions. Jump to the next node
		else if(n.GetTask().Equals("win")){
			return true;
		}

		//if the task is avoid, check if either patrol is close. If so, check the children of avoid in DFS
		else if(n.GetTask().Equals("avoid")){
			bool patrol1Close = Close(patrol1);
			bool patrol2Close = Close(patrol2);
			if(patrol1Close || patrol2Close){
				return true;
			}
			return false;
		}

		//checks if there is a pickable. If there is, push the grab command into the stack
		else if(n.GetTask().Equals("moveGrab")){
			for(int i = 0; i < 10; i++){
				if(hasPickable[i].activeSelf){
					stack.Push("moveGrab");
					return true;
				}
			}
			return false;
		}

		//if there's an error, return false
		return false;
	}




	//EXECUTING NODE
	public void Execute(string task){

		//if the task is avoid, find closest alcove or sides of the wall and move there
		if(task.Equals("moveAvoid")){
			Vector3 pos = FindClosestAlcove();
			MoveAvoid(pos);
		}

		//if the task is idle, find the alcove the AI is in and remain there.
		if(task.Equals("idle")){
			for(int i = 0; i < 10; i++){
				if(transform.position.x == alcove[i].x && transform.position.z == alcove[i].z){
					transform.position = alcove[i];
				}
			}
		}

		//if the task is moveGrab, find the closest alcove with a pickable and move there
		if(task.Equals("moveGrab")){
			float min = Mathf.Infinity;
			float distance = 0f;
			int x = -1;
			for(int i=0; i < 10; i++){
				distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - alcove[i].x,2) + Mathf.Pow(transform.position.z - alcove[i].z,2));
				if(hasPickable[i].activeSelf && min > distance){
					min = distance;
					x = i;
				}
			}
			if(x != -1){
				agent.SetDestination(alcove[x]);
			}
		}
	}

	//helper method that finds closet alcove for the avoid function, which takes into account the sides of the walls
	public Vector3 FindClosestAlcove(){
		float min = 99;
		float distance = 0f;
		int x = -1;
		for(int i=0; i < 10; i++){
			distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - alcove[i].x,2) + Mathf.Pow(transform.position.z - alcove[i].z,2));
			if(min > distance){ 
				min = distance;
				x = i;
			}
		}

		float distance1 = Mathf.Sqrt(Mathf.Pow(transform.position.x + 11.4f,2) + Mathf.Pow(transform.position.z - 0.31f,2));
		if(min > distance1) min = distance1;

		float distance2 = Mathf.Sqrt(Mathf.Pow(transform.position.x - 11.4f,2) + Mathf.Pow(transform.position.z - 0.31f,2));
		if(min > distance2) return new Vector3(11.4f, 0f, 0.31f);
		if(min == distance1) return new Vector3(-11.4f, 0f, 0.31f);
		return alcove[x];
	}

	//helper function that moves the AI to a given location
	public void MoveAvoid(Vector3 pos){
		agent.SetDestination(pos);
	}

	//checks the distance from another gameObject (player or patrol) and returns a boolean indicating if they are too close or not
	public bool TooClose(GameObject p){
		Vector3 pPos = p.transform.position;
		float distance =  Mathf.Sqrt(Mathf.Pow(transform.position.x - pPos.x,2) + Mathf.Pow(transform.position.z - pPos.z,2));
		if (distance < 3) return true;
		return false;
	}

	//checks the distance from another gameObject (player or patrol) and returns a boolean indicating if they are close or not
	public bool Close(GameObject p){
		Vector3 pPos = p.transform.position;
		float distance =  Mathf.Sqrt(Mathf.Pow(transform.position.x - pPos.x,2) + Mathf.Pow(transform.position.z - pPos.z,2));
		if (distance < 5.5f) return true;
		return false;
	}

	//for the getMove, always make sure that the pickable is still avaible for pickup, and if the player picked it up and the world state is changed, exit
	public void GetMove(Vector3 pos, int x){
		while(hasPickable[x].activeSelf == true){
			agent.SetDestination(pos);
		}
	}

	//The DFS function. Evaluate node first, the evaluate the left then middle then right child if they exist and if the evaluation of the first node was successful
	public void DFS(Node n){
		bool proceed = EvalNode(n);
		if(proceed){
			if(n.GetLeft() != null) DFS(n.GetLeft());
			if(n.GetMid() != null) DFS(n.GetMid());
			if(n.GetRight() != null) DFS(n.GetRight());
		}
	}
}