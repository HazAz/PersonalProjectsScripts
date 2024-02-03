using UnityEngine;
using DamageNumbersPro;

public class EnemyHealth : MonoBehaviour
{
	[SerializeField] private int maxHealth = 20;
	private int currentHealth;
	[SerializeField] private MosquitoScript mosquitoScript;
	[SerializeField] private SkeeterBossScript skeeterBossScript;
	[SerializeField] private AntScript antScript;
	[SerializeField] private AntBossScript antBossScript;
	[SerializeField] private BeeScript beeScript;
	[SerializeField] private BeeBossScript beeBossScript;
	[SerializeField] private DamageNumber damagePrefab;
	[SerializeField] private GameObject bloodPrefab;

	void Start()
	{
		currentHealth = maxHealth;
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;

		if (currentHealth < 0)
		{
			Die();
		}
		
		damagePrefab.Spawn(transform.position, damage);
	}

	private void Die()
	{
		Instantiate(bloodPrefab, transform.position, Quaternion.identity);

		mosquitoScript?.Die();
		skeeterBossScript?.Die();
		antScript?.Die();
		antBossScript?.Die();
		beeScript?.Die();
		beeBossScript?.Die();
	}
}
