using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectCardButton : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI characterName;
	[SerializeField] private Button button;
	[SerializeField] private Image characterIcon;

	private PlayerCharacterScriptableObject characterData;

	private QuestSelectionManager questSelectionManager;

	public Button Button { get { return button; } }
	public PlayerCharacterScriptableObject CharacterData { get { return characterData; } }

	public void Init(PlayerCharacterScriptableObject characterdata, QuestSelectionManager manager)
	{
		questSelectionManager = manager;

		characterData = characterdata;
		characterName.text = characterData.CharacterName;
		characterIcon.sprite = characterData.CharacterImage;
	}

	public void ButtonPressed()
	{
		questSelectionManager.CharacterSelected(this);
	}

	public void ShowCharacterStats()
	{
		questSelectionManager.ShowCharacterStats(characterData);
	}

	public void HideCharacterStats()
	{
		questSelectionManager.HideCharacterStats();
	}
}
