using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
	private Vector3 dashDirection = Vector2.zero;
	private float currentTimer = 0.0f;
	public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
	public override void EnterState() 
	{
		if (_ctx.IsMovementPressed)
		{
			// Dash
			dashDirection = _ctx.ConvertToCameraSpace(_ctx.CurrentMovement);
		}
		else
		{
			// Dodge
			dashDirection = _ctx.ConvertToCameraSpace(_ctx.CurrentMovement);
		}

		_ctx.AlreadyDashedThisClick = true;
	}

	public override void ExitState()
	{
		_ctx.DashCooldown();
	}

	public override void UpdateState()
	{
		currentTimer += Time.deltaTime;
		if (currentTimer >= _ctx.DashDuration)
		{
			CheckSwitchStates();
			return;
		}

		HandleDash();
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

	private void HandleDash()
	{
		_ctx.CharacterController.Move(dashDirection * _ctx.DashSpeed * Time.deltaTime);
	}
}