using System.Collections;
using UnityEngine;

public class AntBossScript : MonoBehaviour
{
	[SerializeField] private float speed = 10f;
	[SerializeField] private float meleeRange = 3f;
	[SerializeField] private int damage = 30;
	[SerializeField] private Animator animator;
	[SerializeField] private EnemyHealth enemyHealth;

	private Transform player;

	private bool inAction = false;

	private PlayerHealth playerHealth;
	private EnemySpawner enemySpawner;

	public void Init(Transform playerTransform, EnemySpawner es)
	{
		player = playerTransform;
		playerHealth = player.GetComponent<PlayerHealth>();
		enemySpawner = es;
	}

	private void Update()
	{
		if (player == null || inAction)
		{
			return;
		}

		float distanceToPlayer = Vector3.Distance(transform.position, player.position);

		if (distanceToPlayer > meleeRange)
		{
			animator.Play("Ant-Walk");
			ChasePlayer();
		}
		else
		{
			StartCoroutine(MeleeAttack());
		}
	}

	private void ChasePlayer()
	{
		transform.rotation = Quaternion.Euler(new Vector3(0f, player.position.x - transform.position.x > 0f ? 90f : -90f, 0f));
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
		transform.position = new Vector3(transform.position.x, 1f, 0f);
	}

	private IEnumerator MeleeAttack()
	{
		inAction = true;
		animator.Play("Ant-Chomp");
		yield return new WaitForSeconds(0.6f);

		var direction = (playerHealth.transform.position - transform.position).normalized;
		if (Physics.Raycast(transform.position, direction, out var hit, meleeRange))
		{
			// Check if the hit object is the player
			if (hit.collider.CompareTag("Player"))
			{
				playerHealth.TakeDamage(damage);

				if (playerHealth.HasChili)
				{
					enemyHealth.TakeDamage(5);
				}
			}
			else if (hit.collider.CompareTag("Enemy") && playerHealth.HasBerry)
			{
				hit.collider.GetComponent<EnemyHealth>()?.TakeDamage(10);
			}
		}

		yield return new WaitForSeconds(0.5f);
		animator.Play("Ant-Walk");
		yield return new WaitForSeconds(0.5f);
		inAction = false;
	}

	public void Die()
	{
		enemySpawner.EnemyDied();
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			InvokeRepeating("DealCollisionDamage", 1f, 1f);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			CancelInvoke("DealCollisionDamage");
		}
	}

	private void DealCollisionDamage()
	{
		playerHealth.TakeDamage(5);

		if (playerHealth.HasChili)
		{
			enemyHealth.TakeDamage(5);
		}
	}
}
