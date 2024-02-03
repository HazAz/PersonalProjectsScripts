using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaColliderScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.GetComponent<EnemyHealth>()?.TakeDamage(5);
		}
	}
}
