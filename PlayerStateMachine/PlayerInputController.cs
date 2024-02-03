public class PlayerInputController : Singleton<PlayerInputController>
{
	public PlayerInput PlayerInput;

	private void Awake()
	{
		PlayerInput = new PlayerInput();

		EnablePlayerController();
		DisableUIController();
	}
	
	public void DisablePlayerController()
	{
		PlayerInput.CharacterControls.Disable();
	}

	public void EnablePlayerController()
	{
		PlayerInput.CharacterControls.Enable();
	}

	public void DisableUIController()
	{
		PlayerInput.UI.Disable();
	}

	public void EnableUIController()
	{
		PlayerInput.UI.Enable();
	}
}
