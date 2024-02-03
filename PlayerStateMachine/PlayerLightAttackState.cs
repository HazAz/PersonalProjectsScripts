public class PlayerLightAttackState : PlayerBaseState
{
	private int currentCounter = 1;

	public PlayerLightAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
	public override void EnterState()
	{
		_ctx.Animator.Play("Attack1");
		_ctx.AlreadyLightAttackedThisClick = true;
	}

	public override void ExitState()
	{
	}

	public override void UpdateState()
	{
		CheckAttackCombo();

		if (_ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
		{
			CheckSwitchStates();
		}

	}

	public override void CheckSwitchStates()
	{
		if (_ctx.CanDash())
		{
			_ctx.LightAttackComboCounter = 1;
			SwitchState(_factory.DashState());
		}

		if (_ctx.IsMovementPressed)
		{
			_ctx.LightAttackComboCounter = 1;
			SwitchState(_factory.MoveState());
		}
		else
		{
			_ctx.LightAttackComboCounter = 1;
			SwitchState(_factory.IdleState());
		}
	}

	private void CheckAttackCombo()
	{
		if (_ctx.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + currentCounter) && _ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
		{
			if (_ctx.CanLightAttack())
			{
				++currentCounter;

				if (currentCounter > _ctx.LightAttackMaxAttackComboAmount)
				{
					currentCounter = 1;
				}

				_ctx.Animator.Play("Attack" + currentCounter);
			}
		}
	}
}