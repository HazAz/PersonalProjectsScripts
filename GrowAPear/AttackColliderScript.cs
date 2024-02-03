using UnityEngine;

public class AttackColliderScript : MonoBehaviour
{
	[SerializeField] private BoxCollider boxCollider;

	private int damage = 10;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
		}
	}

	public void IncreaseAttack()
	{
		damage = 15;
	}
	public void SetGrapeCollider()
	{
		damage = 5;
	}

	public void IncreaseSize()
	{
		boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 1.2f);
	}
}
