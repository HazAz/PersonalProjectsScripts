using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T, U> {

	private T firstP;
    private U secondP;
	public T FirstPoint{
    	set{firstP = value;}
        get{return firstP;}       
    }

    public U SecondPoint{
    	set{secondP = value;}
    	get{ return secondP;}   
    }
    

     public Pair (T first, U second) {
        this.FirstPoint = firstP;
        this.SecondPoint = secondP;
    }
  
    public bool pairEquals(object o) {   
        if (GetType() != o.GetType() || o == null){
            return false;
        }
        
        Pair<T, U> pair = (Pair<T, U>) o; 
        if (firstP.Equals(pair.FirstPoint) && secondP.Equals(pair.SecondPoint)) {
            return true;
        }
        return false;
    }
}