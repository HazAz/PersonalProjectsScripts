using UnityEngine;

public class PlayerHeavyAttackState : PlayerBaseState
{
	public PlayerHeavyAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
	private HeavyAttackBase currentHeavyAttack;
	private float currentTimer = 0f;
	public override void EnterState()
	{
		//_ctx.Animator.Play(stagger)
		currentHeavyAttack = _ctx.CurrentHeavyAttack;
		_ctx.CurrentHeavyAttack = null;

		currentHeavyAttack.ActivateHeavyAttack();
	}
	public override void ExitState()
	{
	}

	public override void UpdateState()
	{
		currentTimer += Time.deltaTime;

		if (currentTimer > 2f)
		{
			currentHeavyAttack.StartHeavyAttackCooldown();
			CheckSwitchStates();
		}
	}

	public override void CheckSwitchStates()
	{
		if (_ctx.IsMovementPressed)
		{
			SwitchState(_factory.MoveState());
		}
		else
		{
			SwitchState(_factory.IdleState());
		}
	}
}