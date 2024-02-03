using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {

	//the cube we use
	public GameObject cubePrefab;

	//the area we can play in
	public GameObject area;

	public int min;
	public int max;

	public int numObstacles = 4;

	public static Obstacles Ob = null;
	
	private Obstacles() { }

	//creates an instance
	private void Awake(){
		if (Ob == null) Ob = this;
		else if (Ob != this) Destroy(this);  
	}

	//creates all the obstacles
	public void CreateAllObstacles() {
		GameObject par = Instantiate(new GameObject());
		for (int i = 0; i < numObstacles; i++){
			GameObject obstacle = CreateOneObstacle();
			SpawnObstacles(obstacle);
			obstacle.transform.parent = par.transform;
		}
	}

	// creates one obstacle by joining 1 to 4 cubes together
	GameObject CreateOneObstacle(){
		int numCubes = (int) Random.Range(0f, 4.99f);
		GameObject parentObstacle = Instantiate(cubePrefab);
		parentObstacle.transform.localScale = new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
		Bounds border = parentObstacle.GetComponent<Renderer>().bounds;

		for (int i = 0; i < numCubes; i++){
			GameObject childObstacle = Instantiate(cubePrefab);
			childObstacle.transform.localScale = Vector3.one * Random.Range(min, max);
			Vector3 rand = new Vector3(Random.Range(border.min.x, border.max.x), 2f, Random.Range(border.min.z, border.max.z));
			childObstacle.transform.localPosition = rand;
			childObstacle.transform.parent = parentObstacle.transform;
		}
		//give it a random rotation
		parentObstacle.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
		return parentObstacle;
	}

   
	void SpawnObstacles(GameObject ob){
		Bounds obstacleBounds = ob.GetComponent<Renderer>().bounds;
		Bounds areaBounds = area.GetComponent<Renderer>().bounds;
		Vector3 rand = new Vector3(Random.Range(areaBounds.min.x, areaBounds.max.x), obstacleBounds.size.y, Random.Range(areaBounds.min.z, areaBounds.max.z));
		ob.transform.position = rand;
	}
}























	/*public float sharpness;
	public float spread;
	public int precision;
	
	public int numVertices;
	
	private Vector3 mid;
	public PathFinder grid;
	private Mesh mesh;
	public Material material;

	// Use this for initialization
	void Start () {
		mid = new Vector3(0, 1, 0); //CHANGE NUMBERS

		List<Vector3> verticesList = CreateVertices();
		verticesList.Add(mid);

		int[] trianglesArray = GetTrianglesFromPolygon(verticesList.Count);

		mesh = new Mesh();

		mesh.vertices = verticesList.ToArray();
		mesh.triangles = trianglesArray;

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshRenderer>().material = material;

		updateGrid(verticesList);
	}

	public List<Vector3> CreateVertices() {

		int edge;
		Vector3 vertex1;
		Vector3 vertex2;

		float width = Random.Range(1.0f, spread);

		List<Vector3> vertices = new List<Vector3>();
		vertices.Add(new Vector3(mid.x - width*0.5f, mid.y, mid.z));
		vertices.Add(new Vector3(mid.x + width*0.5f, mid.y, mid.z));

		for (int i = 2; i < numVertices; i++) {
			edge = Random.Range(0, i);

			if(edge != i-1){
				vertex1 = vertices[edge];
				vertex2 = vertices[edge + 1];
			}
			else{
				vertex1 = vertices[edge];
				vertex2 = vertices[0];
			}
			
			float mag = Random.Range(1.0f, sharpness);
			float split = Random.Range(0f, 1.0f);

			Vector3 newVertex = (1 - split) * vertex1 + split * vertex2;

			Vector2 perpenducular = Vector2.Perpendicular(new Vector2(vertex2.x - vertex1.x, vertex2.z - vertex1.z)).normalized;
			
			newVertex += new Vector3(perpenducular.x * mag, 0, perpenducular.y * mag);
			
			vertices.Insert(edge + 1, newVertex);
		}

		return vertices;
	}

	public int[] GetTrianglesFromPolygon(int numV) {
		int center = numV - 1;
		int[] triangleNodes = new int[center * 3];

		for (int i = 0; i < center; i++) {
			triangleNodes[i * 3] = i;
			triangleNodes[i * 3 + 2] = center;
			if (i != center-1) {
				triangleNodes[i * 3 + 1] = i + 1;	
			} 
			else {
				triangleNodes[i * 3 + 1] = 0;
			}
		}
		return triangleNodes;
	}

	// going with edge midpoints and vertices for now, it seems reasonable but not perfect
	private void updateGrid(List<Vector3> vertices) {

		Vector3 gridsToFill;
		// place object at center and halfway point between center and all other vertices
		Vector3 center = vertices[vertices.Count - 1] + this.transform.position;
		grid.FillGrid(center);
		for (int i = 0; i < vertices.Count - 1; i++) {
			for (float j = 1; j < precision; j++) {
				gridsToFill = (1-(j/precision))*center + (j/precision)*(vertices[i] + this.transform.position);
				grid.FillGrid(gridsToFill);
			}
		}

		Vector3 pos;
		Vector3 mid;

		// center vertex is at the last index
		for (int i = 0; i < vertices.Count - 1; i++) {
			// place object at each vertex
			pos = vertices[i] + this.transform.position;
			grid.FillGrid(pos);

			for (float j = 1.0f; j <= precision; j++) {
				if (i != vertices.Count - 2) {
					mid = this.transform.position + vertices[i+1] * (1f-j/precision) + vertices[i] * j/precision;
				}
				else {
					mid = this.transform.position + vertices[0] * (1f-j/precision) + vertices[i] * j/precision;

				}
				grid.FillGrid(mid);
			}
			// place object at midpoints of each edge
		}
	}
	*/
