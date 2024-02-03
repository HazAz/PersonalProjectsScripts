using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
	private Vector3 cameraRelativeMovement;
	public PlayerMoveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
	public override void EnterState()
	{
		_ctx.Animator.CrossFade(_ctx.IsCrouching ? _ctx.CrouchMoveHash : _ctx.MoveHash, 0.1f);
	}

	public override void ExitState() 
	{
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
		HandleMove();
		HandleRotation();
		HandleGravity();
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

		if (_ctx.CanHeavyAttack())
		{
			SwitchState(_factory.HeavyAttackState());
		}

		if (!_ctx.IsMovementPressed)
		{
			SwitchState(_factory.IdleState());
		}
	}

	private void HandleRotation()
	{
		Vector3 positionToLookAt;
		Quaternion currentRotation = _ctx.transform.rotation;

		positionToLookAt.x = cameraRelativeMovement.x;
		positionToLookAt.y = 0.0f;
		positionToLookAt.z = cameraRelativeMovement.z;

		if (_ctx.IsMovementPressed)
		{
			// Creates a new rotation based on where the player is currently pressing.
			Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
			_ctx.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _ctx.RotationFactorPerFrame * Time.deltaTime);
		}
	}

	private void HandleMove()
	{
		cameraRelativeMovement = _ctx.ConvertToCameraSpace(_ctx.CurrentMovement);
		var movementSpeed = (_ctx.IsCrouching) ? _ctx.CrouchedMovementSpeed : _ctx.MovementSpeed;
		_ctx.CharacterController.Move(cameraRelativeMovement * movementSpeed * Time.deltaTime);
	}

	private void HandleGravity()
	{
		if (_ctx.CharacterController.isGrounded)
		{
			_ctx.CurrentMovement = new Vector3(_ctx.CurrentMovement.x, AMDConsts.GROUNDED_GRAVITY, _ctx.CurrentMovement.z);
		}
		else
		{
			_ctx.CurrentMovement = _ctx.CurrentMovement + new Vector3(_ctx.CurrentMovement.x, AMDConsts.GRAVITY, _ctx.CurrentMovement.z);
		}
	}
}