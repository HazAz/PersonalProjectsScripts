using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
	public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
	public override void EnterState() 
	{
		_ctx.Animator.CrossFade(_ctx.IsCrouching ? _ctx.CrouchIdleHash : _ctx.IdleHash, 0.4f);
	}

	public override void ExitState() { }

	public override void UpdateState()
	{
		CheckSwitchStates();
	}

	public override void CheckSwitchStates()
	{
		if (_ctx.CanDash())
		{
			SwitchState(_factory.DashState());
		}

		if (_ctx.CanLightAttack())
		{
			SwitchState(_factory.LightAttackState());
		}

		if (_ctx.IsMovementPressed)
		{
			SwitchState(_factory.MoveState());
		}

		if (_ctx.CanHeavyAttack())
		{
			SwitchState(_factory.HeavyAttackState());
		}
	}
}