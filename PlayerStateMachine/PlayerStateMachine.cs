using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerStateMachine : MonoBehaviour
{
	#region Serialized Fields
	// -------------------------------------------- SERIALIZED FIELDS ---------------------------------------------------------------------------------
	public Animator Animator;

	// Movement
	[Header("Movement Variables")]
	[SerializeField] private float _movementSpeed = 10f;
	[SerializeField] private float _crouchedMovementSpeed = 5f;
	[SerializeField] float _rotationFactorPerFrame = 15.0f;
	#endregion

	#region Private fields
	// -------------------------------------------- PRIVATE FIELDS ---------------------------------------------------------------------------------
	// Declare reference variables.
	private CharacterController characterController;
	private PlayerCombatScript playerCombatScript;

	// Player stats
	private bool isDead = false;

	// State variables
	private PlayerBaseState _currentState;
	private PlayerStateFactory _stateFactory;

	// Movement
	private Vector2 currentMovementInput;
	private Vector3 currentMovement;
	private bool isMovementPressed;

	// Dash
	private float dashSpeed = 20f;
	private float dashDuration = 0.4f;
	private float dashCooldown = 0.2f;
	private bool isDashPressed = false;
	private bool canDash = true;
	private bool alreadyDashedThisClick = false;
	private bool isDashUnlocked = true;

	// Crouch
	private bool isCrouching = false;

	// Recharging Magika
	private bool isRechargingMagikaPressed = false;

	// Combat
	// Light Attacks
	private float lightAttackLastAttackTime = 0.0f;
	private int lightAttackComboCounter = 1;
	private bool isLightAttackPressed = false;
	private bool alreadyLightAttackedThisClick = false;
	private int lightAttackMaxAttackComboAmount = 5;

	// Long range attack
	private bool isLongRangeAttackPressed = false;

	// Heavy Attacks
	private bool isHeavyAttackUpPressed = false;
	private bool isHeavyAttackRightPressed = false;
	private bool isHeavyAttackDownPressed = false;
	private bool isHeavyAttackLeftPressed = false;

	// Heavy Attacks init
	private HeavyAttackUp heavyAttackUp;
	private HeavyAttackRight heavyAttackRight;
	private HeavyAttackDown heavyAttackDown;
	private HeavyAttackLeft heavyAttackLeft;
	private HeavyAttackBase currentHeavyAttack;

	// Hashes for animations
	private int moveHash;
	private int idleHash;
	private int attack1Hash;
	private int attack2Hash;
	private int attack3Hash;
	private int attack4Hash;
	private int attack5Hash;
	private int deathHash;
	private int crouchIdleHash;
	private int crouchMoveHash;
	#endregion

	#region Getters and Setters
	// -------------------------------------------- GETTERS AND SETTERS ---------------------------------------------------------------------------------
	// States
	public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
	public PlayerStateFactory StateFactory { get { return _stateFactory; } }

	// Character Controller
	public CharacterController CharacterController { get { return characterController; } }
	public PlayerCombatScript PlayerCombatScript { get { return playerCombatScript; } }

	public bool IsDead { get { return isDead; } set { isDead = value; } }

	// Movement
	public bool IsMovementPressed { get { return isMovementPressed; } }
	public Vector3 CurrentMovement { get { return currentMovement; } set { currentMovement = value; } }
	public float MovementSpeed { get { return _movementSpeed; } }
	public float CrouchedMovementSpeed { get { return _crouchedMovementSpeed; } }
	public float RotationFactorPerFrame { get { return _rotationFactorPerFrame; } }

	// Dash
	public float DashSpeed { get { return dashSpeed; } }
	public float DashDuration { get { return dashDuration; } }
	public bool AlreadyDashedThisClick { set { alreadyDashedThisClick = value; } }

	// Crouch
	public bool IsCrouching { get { return isCrouching; } }

	// Recharging Magika
	public bool IsRechargingMagikaPressed { get { return isRechargingMagikaPressed; } }

	// Combat
	// Light Attacks
	public float LightAttackLastAttackTime { get { return lightAttackLastAttackTime; } set { lightAttackLastAttackTime = value; } }
	public int LightAttackComboCounter { get { return lightAttackComboCounter; } set { lightAttackComboCounter = value; } }
	public int LightAttackMaxAttackComboAmount { get { return lightAttackMaxAttackComboAmount; } set { lightAttackMaxAttackComboAmount = value; } }
	public bool AlreadyLightAttackedThisClick { set { alreadyLightAttackedThisClick = value; } }

	// Long Range Attacks
	public bool IsLongRangeAttackPressed { get { return isLongRangeAttackPressed; } }

	// Heavy Attacks
	public bool IsHeavyAttackUpPressed { get { return isHeavyAttackUpPressed; } }
	public bool IsHeavyAttackRightPressed { get { return isHeavyAttackRightPressed; } }
	public bool IsHeavyAttackDownPressed { get { return isHeavyAttackDownPressed; } }
	public bool IsHeavyAttackLeftPressed { get { return isHeavyAttackLeftPressed; } }
	public HeavyAttackBase CurrentHeavyAttack { get { return currentHeavyAttack; } set { currentHeavyAttack = value; } }

	// Animation Hashes
	public int MoveHash {  get { return moveHash; } }
	public int IdleHash {  get { return idleHash; } }
	public int DeathHash {  get { return deathHash; } }
	public int CrouchIdleHash { get { return crouchIdleHash; } }
	public int CrouchMoveHash { get { return crouchMoveHash; } }
	public int Attack1Hash { get { return attack1Hash; } }
	public int Attack2Hash { get { return attack2Hash; } } 
	public int Attack3Hash { get { return attack3Hash; } }
	public int Attack4Hash { get { return attack4Hash; } }
	public int Attack5Hash { get { return attack5Hash; } }
	#endregion

	#region Unity Lifecycle
	// -------------------------------------------- UNITY LIFECYCLE ---------------------------------------------------------------------------------
	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		playerCombatScript = GetComponent<PlayerCombatScript>();

		// Setup states.
		SetupStates();

		// Setup Animation Hashes.
		SetUpAnimationHashes();
	}

	private void Start()
	{
		// Setup input callbacks.
		SetInputCallbacks();

		// Create Heavy Attack stuff and Set them up
		SetUpHeavyAttacks();
	}

	private void OnEnable()
	{
		PlayerInputController.instance.PlayerInput?.CharacterControls.Enable();
	}

	private void OnDisable()
	{
		PlayerInputController.instance.PlayerInput?.CharacterControls.Disable();
	}

	private void OnDestroy()
	{
		
	}

	void Update()
	{
		if (isDead)
		{
			return;
		}

		_currentState.UpdateState();
	}
	#endregion

	#region Private Helpers
	// Set the player input callbacks
	private void SetInputCallbacks()
	{
		var characterControls = PlayerInputController.instance.PlayerInput.CharacterControls;
		// Movement
		characterControls.Move.started += OnMovementInput;
		characterControls.Move.canceled += OnMovementInput;
		characterControls.Move.performed += OnMovementInput; // Need for Vector 2

		// Dash
		characterControls.Dash.started += OnDashInput;
		characterControls.Dash.canceled += OnDashInput;

		// Combat
		// Light Attack
		characterControls.LightAttack.started += OnLightAttackInput;
		characterControls.LightAttack.canceled += OnLightAttackInput;

		// Long Range Attack
		characterControls.LongRangeAttack.started += OnLongRangeAttackInput;
		characterControls.LongRangeAttack.canceled += OnLongRangeAttackInput;

		// Heavy Attack
		characterControls.StrongAttackUp.started += OnHeavyAttackUpInput;
		characterControls.StrongAttackUp.canceled += OnHeavyAttackUpInput;

		characterControls.StrongAttackRight.started += OnHeavyAttackRightInput;
		characterControls.StrongAttackRight.canceled += OnHeavyAttackRightInput;

		characterControls.StrongAttackDown.started += OnHeavyAttackDownInput;
		characterControls.StrongAttackDown.canceled += OnHeavyAttackDownInput;

		characterControls.StrongAttackLeft.started += OnHeavyAttackLeftInput;
		characterControls.StrongAttackLeft.canceled += OnHeavyAttackLeftInput;

		// Crouch
		PlayerInputController.instance.PlayerInput.CharacterControls.Crouch.started += OnCrouchInput;
	}

	private void SetupStates()
	{
		_stateFactory = new PlayerStateFactory(this);

		_currentState = _stateFactory.IdleState();
		_currentState.EnterState();
	}

	private void SetUpAnimationHashes()
	{
		deathHash = Animator.StringToHash("Death");
		moveHash = Animator.StringToHash("Move");
		idleHash = Animator.StringToHash("Idle");
		crouchIdleHash = Animator.StringToHash("CrouchIdle");
		crouchMoveHash = Animator.StringToHash("CrouchMove");

		// Combat
		attack1Hash = Animator.StringToHash("Attack1");
		attack2Hash = Animator.StringToHash("Attack2");
		attack3Hash = Animator.StringToHash("Attack3");
		attack4Hash = Animator.StringToHash("Attack4");
		attack5Hash = Animator.StringToHash("Attack5");
	}

	private void SetUpHeavyAttacks()
	{
		heavyAttackUp = new();
		heavyAttackRight = new();
		heavyAttackDown = new();
		heavyAttackLeft = new();

		heavyAttackUp.Init();
		heavyAttackRight.Init();
		heavyAttackDown.Init();
		heavyAttackLeft.Init();
	}
	#endregion

	#region Public Functions
	public Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
	{
		Vector3 cameraForward = Camera.main.transform.forward;
		Vector3 cameraRight = Camera.main.transform.right;
		float currentYValue = vectorToRotate.y;

		// remove the Y values to ignore upward/downward camera angles.
		cameraForward.y = 0;
		cameraRight.y = 0;

		// re-normalize both vecttors so they have magnitude 1.
		cameraForward = cameraForward.normalized;
		cameraRight = cameraRight.normalized;

		// rotate X and Z vectorToRotate values
		Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
		Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

		// The sum of both is the vector3 in camera space.
		return cameraForwardZProduct + cameraRightXProduct + new Vector3(0f, currentYValue, 0f);
	}

	public void DisableCharacterControls()
	{
		PlayerInputController.instance.PlayerInput.CharacterControls.Disable();
	}

	// Dash
	public bool CanDash()
	{
		return isDashUnlocked && isDashPressed && canDash && !alreadyDashedThisClick;
	}

	public void DashCooldown()
	{
		canDash = false;
		StartCoroutine(DashCoolDownCoroutine());
	}

	// Combat
	public bool CanLightAttack()
	{
		return isLightAttackPressed && !alreadyLightAttackedThisClick;
	}

	public bool CanHeavyAttack()
	{
		if (isHeavyAttackUpPressed && heavyAttackUp.IsHeavyAttackReady())
		{
			currentHeavyAttack = heavyAttackUp;
			return true;
		}

		if (isHeavyAttackRightPressed && heavyAttackRight.IsHeavyAttackReady())
		{
			currentHeavyAttack = heavyAttackRight;
			return true;
		}

		if (isHeavyAttackDownPressed && heavyAttackDown.IsHeavyAttackReady())
		{
			currentHeavyAttack = heavyAttackDown;
			return true;
		}

		if (isHeavyAttackLeftPressed && heavyAttackLeft.IsHeavyAttackReady())
		{
			currentHeavyAttack = heavyAttackLeft;
			return true;
		}

		return false;
	}

	#endregion

	#region Coroutines
	private IEnumerator DashCoolDownCoroutine()
	{
		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
	}
	#endregion

	#region Handle Inputs
	private void OnMovementInput(InputAction.CallbackContext context)
	{
		currentMovementInput = context.ReadValue<Vector2>();
		currentMovement.x = currentMovementInput.x;
		currentMovement.z = currentMovementInput.y;
		isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
	}

	private void OnDashInput(InputAction.CallbackContext context)
	{
		isDashPressed = context.ReadValueAsButton();

		if (!isDashPressed)
		{
			alreadyDashedThisClick = false;
		}
	}

	private void OnLightAttackInput(InputAction.CallbackContext context)
	{
		isLightAttackPressed = context.ReadValueAsButton();

		if (!isLightAttackPressed)
		{
			alreadyLightAttackedThisClick = false;
		}
	}

	private void OnLongRangeAttackInput(InputAction.CallbackContext context)
	{
		isLongRangeAttackPressed = context.ReadValueAsButton();
	}

    private void OnHeavyAttackUpInput(InputAction.CallbackContext context)
    {
        isHeavyAttackUpPressed = context.ReadValueAsButton();
    }

    private void OnHeavyAttackRightInput(InputAction.CallbackContext context)
    {
        isHeavyAttackRightPressed = context.ReadValueAsButton();
    }

    private void OnHeavyAttackDownInput(InputAction.CallbackContext context)
    {
        isHeavyAttackDownPressed = context.ReadValueAsButton();
    }

    private void OnHeavyAttackLeftInput(InputAction.CallbackContext context)
    {
        isHeavyAttackLeftPressed = context.ReadValueAsButton();
    }

    private void OnCrouchInput(InputAction.CallbackContext context)
	{
		if (!context.ReadValueAsButton() || _currentState is PlayerDashState)
		{
			return;
		}

		isCrouching = !isCrouching;

		// Adjust this when doing visibility
		// BoxCollider.enabled(isCrouching);

		if (_currentState is PlayerMoveState)
		{
			Animator.CrossFade(isCrouching ? crouchMoveHash : moveHash, 0.2f);
		}
		else if (_currentState is PlayerIdleState)
		{
			Animator.CrossFade(isCrouching ? crouchIdleHash : idleHash, 0.2f);
		}
	}
	#endregion
}
