using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AllyAI : AIBase
{
	public LayerMask obstacleLayer;

	// States
	private bool playerInSightRange = false;
	private bool isChasingPlayer = false;

	[Range(0, 360)]
	public float viewAngle;

	private void Start()
	{
		Init();
	}

	public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public void SetPlayerInSightRange(bool inSightRange)
	{
		playerInSightRange = inSightRange;
		if (inSightRange && !isChasingPlayer)
		{
			StartCoroutine(FindPlayerWithDelay());
		}
		else if (!inSightRange && isChasingPlayer)
		{
			StartCoroutine(LookAroundThenLeave());
		}
	}

	private bool CheckCanSeePlayer()
	{
		if (!playerInSightRange)
		{
			return false;
		}

		Vector3 directionToPlayer = (playerCombatScript.transform.position - transform.position).normalized;

		if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
		{
			float distanceToTarget = Vector3.Distance(transform.position, playerCombatScript.transform.position);

			if (!Physics.Raycast(transform.position, directionToPlayer, distanceToTarget, obstacleLayer))
			{
				return true;
			}
		}

		return false;
	}

	IEnumerator FindPlayerWithDelay()
	{
		bool playerSpotted = false;

		while(!playerSpotted && playerInSightRange)
		{
			yield return new WaitForSeconds(0.2f);
			playerSpotted = CheckCanSeePlayer();
		}

		if (playerSpotted)
		{
			transform.LookAt(playerCombatScript.transform.position);
			// Play shocked animation
			isChasingPlayer = true;
			StartCoroutine(ChaseAndAttackPlayer());
		}
	}

	IEnumerator ChaseAndAttackPlayer()
	{
		while (playerInSightRange && isChasingPlayer)
		{
			if (isDead) yield break;
			
			while (isAttacking)
			{
				yield return new WaitForSeconds(0.1f);
				if (isDead) yield break;
			}

			if (playerInAttackRange)
			{
				TryAttackPlayer();
			}
			else
			{
				ChasePlayer();
			}

			yield return null;
		}
	}

	IEnumerator LookAroundThenLeave()
	{
		float timer = 0.0f;
		bool canSeePlayer = CheckCanSeePlayer();
		agent.SetDestination(transform.position);

		ChangeAnimationState(idleHash);

		while (!canSeePlayer && timer < 2.0f)
		{
			yield return new WaitForSeconds(0.2f);
			timer += 0.2f;
			canSeePlayer = CheckCanSeePlayer();
		}

		if (canSeePlayer)
		{
			StartCoroutine(ChaseAndAttackPlayer());
		}
		else
		{
			// move back to station
			isChasingPlayer = false;
		}

	}
}
