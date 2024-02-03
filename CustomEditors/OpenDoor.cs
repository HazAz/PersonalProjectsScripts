using UnityEngine;
using System;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;
    private float timer;
    private Action action;
   
    public void OpenDoors(float time, Action nextAction = null)
    {
        timer = time;
        action = nextAction;
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
        StartCoroutine(WaitForTime());
    }

    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timer);
        if (action != null) action();
    }
}
