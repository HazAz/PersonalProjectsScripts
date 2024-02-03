using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoBulletScript : MonoBehaviour
{
	[SerializeField] private float minSpeed = 3f;
	[SerializeField] private float maxSpeed = 5f;
	[SerializeField] private int damage = 10;
	[SerializeField] private float destroyTime = 3f;

	private Vector3 targetPosition;
	private float speed = 0f;
	private bool isBoss = false;
	private PlayerHealth playerHealth;
	private Vector3 shooterPos = Vector3.zero;

	public void Init(PlayerHealth health, Transform shooterTransform = null, bool boss = false)
	{
		playerHealth = health;
		targetPosition = (playerHealth.transform.position - transform.position).normalized;
		transform.rotation = Quaternion.LookRotation(targetPosition, Vector3.up);

		speed = Random.Range(minSpeed, maxSpeed);
		shooterPos = shooterTransform != null ? shooterTransform.position : Vector3.zero;
		isBoss = boss;
		Invoke("Destroy", destroyTime);
	}

	private void Update()
	{
		transform.position += targetPosition * speed * Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			TryHitPlayer();
		}
		else if (other.CompareTag("Enemy"))
		{
			if (playerHealth.HasOrange)
			{
				other.GetComponent<EnemyHealth>()?.TakeDamage(5);
			}

			return;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void TryHitPlayer()
	{
		if (playerHealth.HasPepper)
		{
			if (!isBoss && Random.value < 0.25f)
			{
				targetPosition = (shooterPos - transform.position).normalized;
				CancelInvoke("Destroy");
				Invoke("Destroy", destroyTime);
				return;
			}
		}

		playerHealth.TakeDamage(damage);
		Destroy(gameObject);
	}

	private void Destroy()
	{
		Destroy(gameObject);
	}
}