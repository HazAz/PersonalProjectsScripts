using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
	[SerializeField] private RotatingBananaScript rotatingBananaPrefab;
	[SerializeField] private AttackColliderScript attackCollider;
	[SerializeField] private AttackColliderScript grapeCollider;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private PlayerHealth playerHealth;
	[SerializeField] private GameObject leftGloves;
	[SerializeField] private GameObject rightGloves;
	public void ApplyPowerups(PowerupTypes powerup)
	{
		switch (powerup)
		{
			case PowerupTypes.Banana:
				var rotatingBanana = Instantiate(rotatingBananaPrefab, transform.position, Quaternion.identity);
				rotatingBanana.Init(this.transform);
				break;

			case PowerupTypes.Cucumber:
				attackCollider.IncreaseAttack();
				break;

			case PowerupTypes.BokChoy:
				playerMovement.IncreaseMovementSpeed();
				break;

			case PowerupTypes.Plum:
				playerMovement.AllowDoubleJump();
				break;

			case PowerupTypes.Chili:
				playerHealth.HasChili = true;
				break;

			case PowerupTypes.Peppers:
				playerHealth.HasPepper = true;
				break;

			case PowerupTypes.Grape:
				grapeCollider.gameObject.SetActive(true);
				grapeCollider.SetGrapeCollider();
				break;

			case PowerupTypes.Apple:
				playerHealth.SetHasApple();
				break;

			case PowerupTypes.Carrot:
				playerMovement.IncreaseJumpForce();
				break;

			case PowerupTypes.Berry:
				playerHealth.HasBerry = true;
				break;

			case PowerupTypes.Orange:
				playerHealth.HasOrange = true;
				break;

			case PowerupTypes.Kiwi:
				playerHealth.HasKiwi = true;
				break;

			case PowerupTypes.Broccoli:
				leftGloves.SetActive(true);
				rightGloves.SetActive(true);
				attackCollider.IncreaseSize();
				playerMovement.CanPunch = true;
				break;
		}
	}
}
