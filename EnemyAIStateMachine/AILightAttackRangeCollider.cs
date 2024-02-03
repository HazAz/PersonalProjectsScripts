using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AILightAttackRangeCollider : MonoBehaviour
{
	[SerializeField] AIBase aiBase;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<PlayerCombatScript>(out var playerScript))
		{
			playerScript.TakeDamage(aiBase);
			// playerScript.TakePostureDamage(postureDamage);
		}
	}
}
