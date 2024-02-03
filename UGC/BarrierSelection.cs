using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSelection : MonoBehaviour
{
    uint currentObjectNumber = 0;
	
	uint maxObjectNumber = 7;

	public GameObject barrierSelection;
	private GameObject myObject;
	public GameObject possibleWebs;
	private bool inBarrierSelection;
	
	// Start is called before the first frame update
    void Start()
    {
    	inBarrierSelection = false;
    }

    // Update is called once per frame
    void Update()
    {
    	if (!inBarrierSelection) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentObjectNumber++;
			
			if (currentObjectNumber >= maxObjectNumber+1)
			{
				currentObjectNumber = 0;
			}
			
			myObject = GameObject.Find("Barrier_" + currentObjectNumber);
			
			barrierSelection.transform.position = myObject.transform.position;
        }
		
		if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
        	if (currentObjectNumber == 0) currentObjectNumber = maxObjectNumber;
			else currentObjectNumber--;
			
			myObject = GameObject.Find("Barrier_" + currentObjectNumber);
			
			barrierSelection.transform.position = myObject.transform.position;
        }
		
		if (Input.GetKeyDown(KeyCode.Space))
        {
			//when go to playLevel, make sure if mesh renderer is off, disable the object because collider is still there
			RemoveObject();
		}
    }

    void RemoveObject()
    {
    	myObject.GetComponent<Renderer>().enabled = !myObject.GetComponent<Renderer>().enabled;
        myObject.GetComponent<BoxCollider2D>().enabled = !myObject.GetComponent<BoxCollider2D>().enabled;
        GlobalInfo.BarriersArray[currentObjectNumber] = myObject.GetComponent<BoxCollider2D>().enabled;
    }

    public void StartBarrierSelection()
    {
    	possibleWebs.SetActive(false);
    	myObject = GameObject.Find("Barrier_" + currentObjectNumber);
    	barrierSelection.transform.position = myObject.transform.position;
    	inBarrierSelection = true;
    }

    public void EndBarrierSelection()
    {
    	inBarrierSelection = false;
    	barrierSelection.transform.position = new Vector3(100f, 100f, 100f);
    }
}
