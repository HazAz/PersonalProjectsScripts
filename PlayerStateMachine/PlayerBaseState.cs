public abstract class PlayerBaseState
{
	protected PlayerStateMachine _ctx;
	protected PlayerStateFactory _factory;

	public PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory factory)
	{
		_ctx = ctx;
		_factory = factory;
	}

	public abstract void EnterState();
	public abstract void UpdateState();
	public abstract void ExitState();
	public abstract void CheckSwitchStates();

	protected void SwitchState(PlayerBaseState newState)
	{
		// current state exits state
		ExitState();

		// new state enter states
		newState.EnterState();

		_ctx.CurrentState = newState;
	}
}
