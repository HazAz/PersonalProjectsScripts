using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionStats : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI characterName;
	[SerializeField] private Image characterImage; 
	[SerializeField] private TextMeshProUGUI healthStat;	
	[SerializeField] private TextMeshProUGUI strengthStat;
	[SerializeField] private TextMeshProUGUI defenseStat;

	[SerializeField] private Image equippedRelic;
	
	// Start is called before the first frame update
	private void Start()
	{
		gameObject.SetActive(false);
	}

	public void DisplayCharacterStats(PlayerCharacterScriptableObject characterData)
	{
		characterName.text = characterData.CharacterName;
		healthStat.text = $"HEALTH: {characterData.HealthPoints}";
		strengthStat.text = $"STRENGTH: {characterData.Strength}";
		defenseStat.text = $"DEFENSE: {characterData.Defense}";

		characterImage.sprite = characterData.CharacterImage;

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
