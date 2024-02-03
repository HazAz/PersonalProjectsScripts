using System.Collections;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour
{
	// Combat
	// [Header("Combat Variables")]
	// public List<PlayerAttackRangeScript> LightAttackColliders;
	//[SerializeField] private Weapon weapon;

	private PlayerUIElements playerUIElements;

	private PlayerStateMachine _ctx;

	private float playerCurrentHealth = 100f;
	private float playerMaxHealth = 100f;
	private float playerPosture = 100f;
	private float playerPostureMax = 100f;
	private Coroutine postureHealingCoroutine = null;

	private bool isDebuffApplied = false;
	private DebuffType currentDebuffType;

	private float attackLevel = 10;
	private float defenseLevel = 10;

	private const float MIN_POSTURE_DAMAGE_FLOAT = 30f;

	// Public references
	public float PlayerCurrentHealth { get { return playerCurrentHealth; } set { playerCurrentHealth = value; } }
	public float PlayerHealthMax { get {  return playerMaxHealth; }}
	public float AttackLevel { get { return attackLevel; } set {  attackLevel = value; } }
	public float DefenseLevel { get { return defenseLevel; } set { defenseLevel = value; } }
	//public Weapon Weapon { get { return weapon; } }


	private void Awake()
	{
		_ctx = GetComponent<PlayerStateMachine>();

		/*foreach (var lightAttackCollider in LightAttackColliders)
		{
			lightAttackCollider.gameObject.SetActive(false);       
		}*/
	}

	private void Start()
	{
		playerUIElements = HUD.instance.GetPlayerUIElements();
	}

	public void TakeDamage(AIBase aiBase)
	{
		playerCurrentHealth -= AMDTools.CalculateDamage(aiBase.AttackLevel, defenseLevel, 5.0f);
		if (playerCurrentHealth <= 0)
		{
			Die();
		}
		else
		{
			playerUIElements.SetCurrentHealth(playerCurrentHealth, playerMaxHealth);
		}
	}

	public void TakePostureDamage(float damage)
	{
		playerPosture -= damage;

		if (playerPosture <= 0)
		{
			if (!_ctx.IsDead)
			{
				_ctx.CurrentState = _ctx.StateFactory.StaggerState();
				playerPosture = playerPostureMax;
			}
		}
		else
		{
			if (postureHealingCoroutine != null)
			{
				StopCoroutine(HealPosture());
			}

			postureHealingCoroutine = StartCoroutine(HealPosture());
		}
	}

	private IEnumerator HealPosture()
	{
		yield return new WaitForSeconds(2f);

		while (playerPosture < playerPostureMax)
		{
			playerPosture += 5f;
			yield return new WaitForEndOfFrame();
		}

		playerPosture = playerPostureMax;
	}

	private void Die()
	{
		playerCurrentHealth = 0;
		playerUIElements.SetCurrentHealth(playerCurrentHealth, playerMaxHealth);
		_ctx.IsDead = true;
		_ctx.Animator.Play(_ctx.DeathHash);
		_ctx.DisableCharacterControls();
		PlayerInputController.instance.DisablePlayerController();
	}
}
