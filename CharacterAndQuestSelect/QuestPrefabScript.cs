using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPrefabScript : MonoBehaviour
{
	[Header("Quest Details")]
	[SerializeField] private GameObject questDescriptionGO;
	[SerializeField] private TextMeshProUGUI questObjectiveText;
	[SerializeField] private TextMeshProUGUI questObjectiveLore;
	[SerializeField] private TextMeshProUGUI questDifficulty;

	[Space(5)]
	[Header("Quest Character")]
	[SerializeField] private Image currentCharacterIcon;
	[SerializeField] private GameObject removeCharacterIconGO;

	[Space(5)]
	[Header("Potential Quest Character")]
	[SerializeField] private GameObject potentialCharacterGO;
	[SerializeField] private Image potentialCharacterImage;
	[SerializeField] private GameObject potentialCharacterArrowUp;
	[SerializeField] private GameObject potentialCharacterArrowDown;

	[Space(5)]
	[Header("Is Quest Mandatory")]
	[SerializeField] private GameObject mandatoryQuestOutline;
	[SerializeField] private GameObject mandatoryTextGO;

	[Space(5)]
	[Header("Misc")]
	[SerializeField] private Button button;

	private QuestSelectionManager questSelectionManager;
	private CharacterSelectCardButton currentCharacterButton;
	private bool isQuestMandatory = false;

	private const float QUEST_DETAILS_DISTANCE_X = 100f;
	private const float QUEST_DETAILS_DISTANCE_Y = 180f;
	private const float POTENTIAL_CHARACTER_DISTANCE_Y = 150f;

	public Button Button { get { return button; } }
	public bool IsAssignedCharacter { get { return currentCharacterButton != null; } }
	public PlayerCharacterScriptableObject CharacterPlayingQuest { get { return currentCharacterButton?.CharacterData; } }

	public void Init(QuestsScriptableObject questSO, QuestSelectionManager manager)
	{
		currentCharacterIcon.gameObject.SetActive(false);

		questSelectionManager = manager;

		SetQuestDetails(questSO);

		SetPotentialCharacterPlacement();

		SetQuestMandatory(questSO);
	}

	public void HoverOnObject()
	{
		questDescriptionGO.SetActive(true);
		
		if (questSelectionManager.IsCharacterSelected)
		{
			potentialCharacterImage.sprite = questSelectionManager.CurrentSelectedCharacter.CharacterData.CharacterImage;
			potentialCharacterGO.SetActive(true);
		}
		else if (currentCharacterButton != null)
		{
			questSelectionManager.ShowCharacterStats(currentCharacterButton.CharacterData);
			removeCharacterIconGO.SetActive(true);
		}
	}

	public void HoverOffObject()
	{
		questDescriptionGO.SetActive(false);
		potentialCharacterGO.SetActive(false);

		if (currentCharacterButton != null && !questSelectionManager.IsCharacterSelected)
		{
			questSelectionManager.HideCharacterStats();
			removeCharacterIconGO.SetActive(false);
		}
	}

	private void SetQuestDetails(QuestsScriptableObject questSO)
	{
		questDescriptionGO.SetActive(false);

		questObjectiveLore.text = questSO.QuestLore;
		questDifficulty.text = $"Difficulty: {questSO.QuestDifficulty}";

		switch (questSO.QuestType)
		{
			case QuestType.SurviveTime:
				questObjectiveText.text = $"Survive for {questSO.SurviveMin} minutes";
				break;

			case QuestType.KillEnemies:
				questObjectiveText.text = $"Kill {questSO.NumberOfEnemiesToKill} enemies";
				break;

			case QuestType.GetObjectiveAndReturn:
				questObjectiveText.text = $"Investigate and return to the ship";
				break;

			case QuestType.KillBoss:
				questObjectiveText.text = $"Kill boss";
				break;
		}

		SetQuestDetailsPosition();
	}

	private void SetQuestDetailsPosition()
	{
		var camera = Camera.main;

		if (camera != null)
		{
			var xPos = transform.position.x;
			var yPos = transform.position.y;

			xPos += (xPos > Screen.width / 2) ? -QUEST_DETAILS_DISTANCE_X : QUEST_DETAILS_DISTANCE_X;
			yPos += (yPos > Screen.height / 2) ? -QUEST_DETAILS_DISTANCE_Y : QUEST_DETAILS_DISTANCE_Y;

			questDescriptionGO.transform.position = new Vector3(xPos, yPos, 0f);
		}
	}

	private void SetPotentialCharacterPlacement()
	{
		var camera = Camera.main;

		if (camera != null)
		{
			var xPos = transform.position.x;
			var yPosPotentialCharacter = transform.position.y;
			var yPosRemoveCharacterIconGO = transform.position.y;

			if (yPosPotentialCharacter > Screen.height / 2)
			{
				yPosPotentialCharacter += POTENTIAL_CHARACTER_DISTANCE_Y;
				yPosRemoveCharacterIconGO += POTENTIAL_CHARACTER_DISTANCE_Y / 2f;

				potentialCharacterArrowUp.SetActive(false);
			}
			else
			{
				yPosPotentialCharacter -= POTENTIAL_CHARACTER_DISTANCE_Y;
				yPosRemoveCharacterIconGO -= POTENTIAL_CHARACTER_DISTANCE_Y / 2f;

				potentialCharacterArrowDown.SetActive(false);
			}

			potentialCharacterGO.transform.position = new Vector3(xPos, yPosPotentialCharacter, 0f);
			removeCharacterIconGO.transform.position = new Vector3(xPos, yPosRemoveCharacterIconGO, 0f);

			potentialCharacterGO.SetActive(false);
			removeCharacterIconGO.SetActive(false);
		}
	}

	private void SetQuestMandatory(QuestsScriptableObject questSO)
	{
		isQuestMandatory = questSO.IsQuestMandatory;
		mandatoryTextGO.SetActive(isQuestMandatory);
		mandatoryQuestOutline.SetActive(isQuestMandatory);

		if (isQuestMandatory)
		{

		}
	}

	public void SelectCharacterForQuestOrRemoveCharacter()
	{
		if (currentCharacterButton != null)
		{
			currentCharacterButton.gameObject.SetActive(true);
			currentCharacterButton = null;
			removeCharacterIconGO.SetActive(false);
		}

		if (!questSelectionManager.IsCharacterSelected)
		{
			currentCharacterIcon.gameObject.SetActive(false);
			questSelectionManager.CheckIfCanPlay();
			return;
		}

		currentCharacterButton = questSelectionManager.CurrentSelectedCharacter;
		currentCharacterIcon.sprite = currentCharacterButton.CharacterData.CharacterImage;

		currentCharacterIcon.gameObject.SetActive(true);
		potentialCharacterGO.SetActive(false);

		questSelectionManager.CharacterChosenForQuest();
	}
}
