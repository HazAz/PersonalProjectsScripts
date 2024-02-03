using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCustomAction : MonoBehaviour
{
    [SerializeField] private CustomAction customAction;
    // Start is called before the first frame update
    void Start()
    {
        customAction.ExecuteAction();
    }
}
