using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditedLevelScript : MonoBehaviour
{
	private GameObject myObject;
    // Start is called before the first frame update
    void Start()
    {
    	RemoveExtras();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RemoveExtras()
    {
    	for (int i = 0; i < 8; i++)
    	{
    		myObject = GameObject.Find("Barrier_" + i);
    		myObject.SetActive(GlobalInfo.BarriersArray[i]);
    		for (int j = 0; j < 4; j++)
    		{
    			myObject = GameObject.Find("Web_" + i + "" + j);
    			if (!GlobalInfo.BarriersArray[i])
    			{
    				myObject.SetActive(false);
    			}
    			else myObject.SetActive(GlobalInfo.WebsArray[i, j]);
    		}
    	}
    }
}
