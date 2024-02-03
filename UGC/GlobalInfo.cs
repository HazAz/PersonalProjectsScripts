using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalInfo
{
	public static bool[] BarriersArray = new bool[8];
	public static bool[,] WebsArray = new bool[8, 4];

	public static void ResetArrays()
	{
		for (int i = 0; i < 8; i++)
    	{
    		BarriersArray[i] = true;
    		for (int j = 0; j < 4; j++)
    		{
    			WebsArray[i, j] = true;
    		}
    	}
	}
}
