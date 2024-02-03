using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIBase : MonoBehaviour
{
	private CharacterController characterController;
	[SerializeField] private EnemyHealthBar healthBar;

	// Level Data
	protected float attackLevel = 5f;
	protected float defenseLevel = 5f;

	// Health and Posture
	protected float maxHealth = 100f;
	protected float currentHealth = 100f;
	protected float maxPosture = 100f;
	protected float currentPosture = 100f;

	// Navmesh and Transform
	protected NavMeshAgent agent;
	protected PlayerCombatScript playerCombatScript;
	[SerializeField] protected Animator animator;
	[SerializeField] protected AILightAttackRangeCollider lightAttackRangeCollider;
	// [SerializeField] protected HeavyAttackRangeCollider HeavyAttackRangeCollider;

	// Attack Data
	protected bool playerInAttackRange;
	protected float timeBetweenAttacks = 1f;
	protected bool isAttacking = false;
	protected bool canAttack = true;

	protected bool isDead = false;

	private int deathHash;
	protected int attack1Hash;
	protected int idleHash;
	protected int moveHash;

	public float AttackLevel { get { return attackLevel; } }

	public bool CanInflictDebuff = false;
	public DebuffType DebuffType;

	private void Awake()
	{
		deathHash = Animator.StringToHash("Death");
		attack1Hash = Animator.StringToHash("Attack1");
		idleHash = Animator.StringToHash("Idle");
		moveHash = Animator.StringToHash("Move");

		characterController = GetComponent<CharacterController>();
		agent = GetComponent<NavMeshAgent>();
	}

	protected void Init()
	{
		playerCombatScript = GameObject.FindObjectOfType<PlayerCombatScript>();
		
		lightAttackRangeCollider.gameObject.SetActive(false);
		healthBar.SetCurrentHealth(currentHealth, maxHealth);
	}

	protected void ChasePlayer()
	{
		agent.destination = playerCombatScript.transform.position;
		ChangeAnimationState(moveHash);
	}

	protected bool AttackPlayer()
	{
		if (isAttacking || !canAttack)
		{
			return false;
		}

		agent.SetDestination(transform.position);

		transform.LookAt(playerCombatScript.transform);

		return true;
	}

	public void TakeDamage(float multiplier)
	{
		currentHealth -= AMDTools.CalculateDamage(playerCombatScript.AttackLevel, defenseLevel, multiplier);
		
		if (currentHealth <= 0f)
		{
			Die();
			return;
		}

		healthBar.SetCurrentHealth(currentHealth, maxHealth);
	}

	public void TakePostureDamage(float postureDamage)
	{
		currentPosture -= postureDamage;
		if (postureDamage <= 0f)
		{
			//BreakPosture();
		}
	}

	private void Die()
	{
		characterController.enabled = false;
		isDead = true;
		animator.Play(deathHash);

		currentHealth = 0f;
		healthBar.SetCurrentHealth(currentHealth, maxHealth);

		var boxCollider = GetComponent<BoxCollider>();
		if (boxCollider != null)
		{
			boxCollider.enabled = false;
		}

		Invoke(nameof(DisableHealthBar), 2f);
	}

	public void SetPlayerInAttackRange(bool value)
	{
		playerInAttackRange = value;
	}

	private void DisableHealthBar()
	{
		healthBar.gameObject.SetActive(false);
	}

	protected IEnumerator AttackCoroutine()
	{
		isAttacking = true;
		canAttack = false;

		yield return new WaitForSeconds(0.5f);
		lightAttackRangeCollider.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.25f);
		lightAttackRangeCollider.gameObject.SetActive(false);
		yield return new WaitForSeconds(0.25f);
		isAttacking = false;

		yield return new WaitForSeconds(1f);
		canAttack = true;
	}

	protected void ChangeAnimationState(int stateHash)
	{
		if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateHash)
		{
			return;
		}
		animator.Play(stateHash);
	}

	protected bool TryAttackPlayer()
	{
		if (!canAttack || !AttackPlayer())
		{
			return false;
		}

		ChangeAnimationState(attack1Hash);
		StartCoroutine(AttackCoroutine());
		return true;
	}
}
