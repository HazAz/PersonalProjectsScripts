using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//circle class
public class YellowCircle {

    private float radius = 2f;
    private int nextSpot = 0;
    public Vector3 center;
    List<YellowSocial> peopleInCircle;

    private int count;

    public int Count() {
        return peopleInCircle.Count;
    }


    public YellowCircle(Vector3 cen, YellowSocial person) {
        this.peopleInCircle = new List<YellowSocial>();
        this.peopleInCircle.Add(person);
        this.center = cen;
    }

    public void Enter(YellowSocial person) {
        peopleInCircle.Add(person);
    }

    //finds good spot for socializer to stand in
    public Vector3 GetNextSpot() {
        float angle;
        float newXPos;
        float newZPos;

        angle = 1/3 * Mathf.PI;
        newXPos = this.radius * Mathf.Cos(angle * nextSpot) + this.center.x;
        newZPos = this.radius * Mathf.Sin(angle * nextSpot) + this.center.z;
        nextSpot++;

        return new Vector3(newXPos, 1, newZPos);
    }

    public void Leave(YellowSocial person) {
       peopleInCircle.Remove(person);
    }

    
}