using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemyAI : AIBase
{
	private void Start()
	{
		Init();
		StartCoroutine(EnemyAICoroutine());
	}

	IEnumerator EnemyAICoroutine()
	{
		yield return new WaitForSeconds(Random.Range(1f, 2f));

		while (!isDead)
		{
			if(!isAttacking)
			{
				if (playerInAttackRange)
				{
					if (!TryAttackPlayer())
					{
						ChangeAnimationState(idleHash);
					}
				}
				else
				{
					ChasePlayer();
				}
			}

			yield return new WaitForSeconds(0.2f);
		}
	}
}
