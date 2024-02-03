using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
	private GameObject level;
    // Start is called before the first frame update
    void Start()
    {
        level = this.transform.parent.gameObject;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        level.GetComponent<Level>().Victory();
    }
}
