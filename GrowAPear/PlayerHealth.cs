using UnityEngine;
using UnityEngine.SceneManagement;
using DamageNumbersPro;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private int maxHealth = 100;
	private int currentHealth;
	[SerializeField] private HealthBar healthBar;
	[SerializeField] private PlayerSFXScript playerSFXScript;
	[SerializeField] private PlayerMovement playerMovement;

	[SerializeField] private DamageNumber healPrefab;
	[SerializeField] private DamageNumber dodgePrefab;
	[SerializeField] private DamageNumber damagePrefab;
	[SerializeField] private DamageNumber deflectPrefab;

	[SerializeField] private GameObject stainGO;

	private bool isDead = false;

	private bool hasChili = false;
	private bool hasPepper = false;
	private bool hasBerry = false;
	private bool hasOrange = false;
	private bool hasKiwi = false;
	public bool HasChili { get { return hasChili; } set { hasChili = value; } }
	public bool HasPepper { get { return hasPepper; } set { hasPepper = value; } }
	public bool HasBerry { get { return hasBerry; } set { hasBerry = value; } }
	public bool HasOrange { get {  return hasOrange; } set { hasOrange = value; } }
	public bool HasKiwi { get {  return hasKiwi; } set { hasKiwi = value; } }
	
	void Start()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}

	public void TakeDamage(int damage)
	{
		if (isDead) return;

		if (hasKiwi && Random.value < 0.15f)
		{
			dodgePrefab.Spawn(transform.position, "DODGE");
			return;
		}

		currentHealth -= damage;

		if (currentHealth < 0)
		{
			currentHealth = 0;
			isDead = true;
			Die();
		}
		else
		{
			playerSFXScript.PlayDamageTakenSFX();
		}

		damagePrefab.Spawn(transform.position, damage);
		healthBar.SetHealth(currentHealth);
	}

	private void Die()
	{
		StaticPowerupScript.OnDeath();
		stainGO.SetActive(true);
		playerMovement.IsDead = true;
		CancelInvoke();
		playerSFXScript.PlayDeathSFX();
		Invoke("GameOver", 1f);
	}

	public void SetHasApple()
	{
		InvokeRepeating("HealFromApple", 0f, 10f);
	}

	private void HealFromApple()
	{
		currentHealth = Mathf.Min(maxHealth, currentHealth + 10);
		healPrefab.Spawn(transform.position, 10);

		healthBar.SetHealth(currentHealth);
	}

	public void HealFromWater()
	{
		currentHealth = Mathf.Min(maxHealth, currentHealth + 10);
		healPrefab.Spawn(transform.position, 10);

		healthBar.SetHealth(currentHealth);
	}

	private void GameOver()
	{
		SceneManager.LoadScene("GameOver");
	}

	public void Deflect()
	{
		deflectPrefab.Spawn(transform.position, "DEFLECT");
	}
}

