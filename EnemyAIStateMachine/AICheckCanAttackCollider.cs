using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICheckCanAttackCollider : MonoBehaviour
{
	[SerializeField] private AIBase aiBase;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			aiBase.SetPlayerInAttackRange(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			aiBase.SetPlayerInAttackRange(false);
		}
	}
}
