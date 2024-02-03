using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScanner : MonoBehaviour
{
    private int collisionCount;
    // Start is called before the first frame update
    void Start()
    {
        collisionCount = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            collisionCount++;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            collisionCount--;
        }
    }

    public bool IsClear()
    {
        return collisionCount == 0;
    }
}
