using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSelection : MonoBehaviour
{
    int currentObjectNumber = 0;
    int currentBarrierNumber = 0;
	
	int maxBarrierNumber = 7;
	int maxWebNumber = 3;

	public GameObject webSelection;
	private GameObject myObject;
	public GameObject possibleWebs;
	private bool cont;
	private bool inWebSelection;
	
	// Start is called before the first frame update
    void Start()
    {
    	inWebSelection = false;
    }

    // Update is called once per frame
    void Update()
    {
    	if (!inWebSelection || !cont) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FindNextAvailableWeb();
			
			myObject = GameObject.Find("Web_" + currentBarrierNumber + "" + currentObjectNumber);
			webSelection.transform.position = myObject.transform.position;
        }
		
		if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
        	FindPrevAvailableWeb();
			
			myObject = GameObject.Find("Web_" + currentBarrierNumber + "" + currentObjectNumber);
			webSelection.transform.position = myObject.transform.position;
        }
		
		if (Input.GetKeyDown(KeyCode.Space))
        {
			RemoveObject();
		}
    }

    void RemoveObject()
    {
    	myObject.GetComponent<Renderer>().enabled = !myObject.GetComponent<Renderer>().enabled;
        myObject.GetComponent<BoxCollider2D>().enabled = !myObject.GetComponent<BoxCollider2D>().enabled;
        GlobalInfo.WebsArray[currentBarrierNumber, currentObjectNumber] = myObject.GetComponent<Renderer>().enabled;

    }

    void ChangeVisibility()
    {
    	for (int i = 0; i < maxBarrierNumber + 1; i++)
    	{
    		GameObject barrier = GameObject.Find("Barrier_" + i);
    		for (int j = 0; j < maxWebNumber + 1; j++)
    		{
    			myObject = GameObject.Find("Web_" + i + "" + j);
    			myObject.GetComponent<Renderer>().enabled = barrier.GetComponent<Renderer>().enabled;
                myObject.GetComponent<BoxCollider2D>().enabled = barrier.GetComponent<BoxCollider2D>().enabled;
                GlobalInfo.WebsArray[currentBarrierNumber, currentObjectNumber] = myObject.GetComponent<Renderer>().enabled;
    		}
    	}
    	
    }

    void FindNextAvailableWeb()
    {
    	currentObjectNumber++;
    	if (currentObjectNumber < maxWebNumber + 1) return;
    	currentObjectNumber = 0;
    	for (int i = currentBarrierNumber + 1; i < maxBarrierNumber + 1; i++)
    	{
    		GameObject barrier = GameObject.Find("Barrier_" + i);
    		if (barrier.GetComponent<Renderer>().enabled)
    		{
    			currentBarrierNumber = i;
				return;
    		}
    	}
    	//if it didnt return, then it couldnt find next available web. So we find first available web
    	for (int i = 0; i < currentBarrierNumber; i++)
    	{
    		GameObject barrier = GameObject.Find("Barrier_" + i);
    		if (barrier.GetComponent<Renderer>().enabled)
    		{
    			currentBarrierNumber = i;
				return;
    		}
    	}
    }

    bool FindFirstAvailableWeb()
    {
    	for (int i = 0; i < maxBarrierNumber + 1; i++)
    	{
    		GameObject barrier = GameObject.Find("Barrier_" + i);
    		if (barrier.GetComponent<Renderer>().enabled)
    		{
    			currentBarrierNumber = i;
				return true;
    		}
    	}
    	return false;
    }


	void FindPrevAvailableWeb()
    {
    	if (currentObjectNumber > 0)
    	{
    		currentObjectNumber--;
    		return;
    	}
    	currentObjectNumber = maxWebNumber;
    	for (int i = currentBarrierNumber - 1; i > 0; i--)
    	{
    		GameObject barrier = GameObject.Find("Barrier_" + i);
    		if (barrier.GetComponent<Renderer>().enabled)
    		{
    			currentBarrierNumber = i;
				return;
    		}
    	}
    	//if it didnt return, then it couldnt find next available web. So we find last available web
    	for (int i = maxBarrierNumber; i > currentBarrierNumber; i--)
    	{
    		GameObject barrier = GameObject.Find("Barrier_" + i);
    		if (barrier.GetComponent<Renderer>().enabled)
    		{
    			currentBarrierNumber = i;
				return;
    		}
    	}
    }

    public void StartWebSelection()
    {
    	possibleWebs.SetActive(true);
    	inWebSelection = true;
    	cont = true;
    	ChangeVisibility();
    	if (FindFirstAvailableWeb())
    	{
    		myObject = GameObject.Find("Web_" + currentBarrierNumber + "" + currentObjectNumber);
    		webSelection.transform.position = myObject.transform.position;
    	}
    	else
    	{
    		cont = false;
    	}
    }

    public void EndWebSelection()
    {
    	inWebSelection = false;
    	webSelection.transform.position = new Vector3(100f, 100f, 100f);
    }
}
