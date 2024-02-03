public class PlayerStateFactory
{
	PlayerStateMachine _context;

	// Regular States
	public PlayerStateFactory(PlayerStateMachine context)
	{
		_context = context;
	}

	public PlayerBaseState IdleState()
	{
		return new PlayerIdleState(_context, this);
	}

	public PlayerBaseState MoveState()
	{
		return new PlayerMoveState(_context, this);
	}

	public PlayerBaseState DashState()
	{
		return new PlayerDashState(_context, this);
	}

	public PlayerBaseState LightAttackState()
	{
		return new PlayerLightAttackState(_context, this);
	}

	public PlayerBaseState StaggerState()
	{
		return new PlayerStaggerState(_context, this);
	}

	public PlayerBaseState HeavyAttackState()
	{
		return new PlayerHeavyAttackState(_context, this);
	}
}