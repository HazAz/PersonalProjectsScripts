public class PlayerLongAttackState : PlayerBaseState
{
	public PlayerLongAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
	public override void EnterState() { }

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

		if (_ctx.IsMovementPressed)
		{
			SwitchState(_factory.MoveState());
		}
	}
}