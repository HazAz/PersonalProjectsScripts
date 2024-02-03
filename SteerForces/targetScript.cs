using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetScript : MonoBehaviour {
	//if traveller collides with target, make him respawn

	private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Traveller"))
        {
            other.GetComponent<RedTraveller>().Respawn();
        }
    }
}
