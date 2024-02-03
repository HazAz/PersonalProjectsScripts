using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//function that spawns obstacles and AI
public class SpawnScript : MonoBehaviour {
	public GameObject traveller;
	public GameObject social;
	public GameObject wanderer;

	public float minSpeed;
	public float maxSpeed;

	public int numObstacles;

	public int numTraveller;
	public int numSocial;
	public int numWanderer;

	 public GameObject area;

	public static SpawnScript Sp = null;
	private SpawnScript() {}

	//creats instance
	private void Awake()
    {
        if (Sp == null) Sp = this;
        else if (Sp != this) Destroy(this); 
    }

    //create obstacles, spawn wanderers and socials and travellers
	void Start () {
		Obstacles.Ob.CreateAllObstacles();
        Spawn(wanderer, numWanderer);
        Spawn(social, numSocial);
        StartCoroutine(SpawnTraveller(traveller, numTraveller));
	}

	//function that spanws wanderers and socials
	void Spawn(GameObject person, int num) {
        Bounds personBounds = person.GetComponent<Renderer>().bounds;
        Bounds areaBounds = area.GetComponent<Renderer>().bounds;
   
        for (int i = 0; i < num; i++){
            Vector3 rand = new Vector3(Random.Range(areaBounds.min.x, areaBounds.max.x), 1.8f, Random.Range(areaBounds.min.z, areaBounds.max.z));
           
            var character = Instantiate(person, rand, Quaternion.identity);
            if(character.CompareTag("Wanderer")){
               character.GetComponent<greenWanderer>().maxSpeed = Random.Range(minSpeed, maxSpeed);
            }
            else if(character.CompareTag("Social")){
               character.GetComponent<YellowSocial>().maxSpeed = Random.Range(minSpeed, maxSpeed);
            }
        }
    }
	//function that spawns travellers
    public IEnumerator SpawnTraveller(GameObject travel, int num){
        Vector3 source = new Vector3(20f, 1.8f, 0f);
        for (int i = 0; i < num; i++){
            var character = Instantiate(travel, source, Quaternion.identity);
            travel.GetComponent<RedTraveller>().maxSpeed = Random.Range(minSpeed, maxSpeed);
            yield return new WaitForSeconds(0.1f);
        }
    }

	
}
