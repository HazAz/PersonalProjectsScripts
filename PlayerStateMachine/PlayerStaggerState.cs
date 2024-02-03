using UnityEngine;

public class PlayerStaggerState : PlayerBaseState
{
	public PlayerStaggerState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
	public override void EnterState()
	{
		//_ctx.Animator.Play(stagger)
	}
	public override void ExitState()
	{
	}

	public override void UpdateState()
	{
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