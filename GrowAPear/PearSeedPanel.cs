using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearSeedPanel : MonoBehaviour
{
    private Action action;
    public void Init(Action onComplete)
    {
        action = onComplete;
    }

    public void OnClickOkay()
    {
        Invoke(nameof(DoAction), 1.5f);
    }

	private void DoAction()
	{
		gameObject.SetActive(false);
		action?.Invoke();
	}
}
