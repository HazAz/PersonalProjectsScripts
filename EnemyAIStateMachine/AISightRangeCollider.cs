using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISightRangeCollider : MonoBehaviour
{
	[SerializeField] private AllyAI allyAI;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			allyAI.SetPlayerInSightRange(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			allyAI.SetPlayerInSightRange(false);
		}
	}
}
