using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestSelectionManager : MonoBehaviour
{
	[Header("Quests prefab and data")]
	[SerializeField] private List<QuestPrefabScript> quests = new();
	[SerializeField] private AreaScriptableObject areaSO;

	[Space(5)]
	[Header("Character Cards")]
	[SerializeField] private CharactersAvailablePerArea charactersAvailable;
	[SerializeField] private CharactersContainerScript characterContainer;
	[SerializeField] private GameObject characterSelectCardDraggableItemPrefab;

	[Space(5)]
	[Header("Character Stats")]
	[SerializeField] private CharacterSelectionStats characterSelectionStats;

	[Space(5)]
	[Header("Play Button")]
	[SerializeField] private Button playButton;

	[Space(5)]
	[Header("UI Manager")]
	[SerializeField] private UIManagerScriptableObject uiManager;

	[Space(5)]
	[Header("Ra Base")]
	[SerializeField] private GameObject raBase;



	private List<CharacterSelectCardButton> characterSelectCards = new();

	private List<QuestPrefabScript> mandatoryQuests = new();

	private CharacterSelectCardButton currentSelectedCharacter;

	public CharacterSelectCardButton CurrentSelectedCharacter { get { return currentSelectedCharacter; } }

	public bool IsCharacterSelected { get { return currentSelectedCharacter != null; } }

	private event Action OnComplete;

	public void Init(Action onComplete)
	{
		// Init quests
		if (quests.Count != areaSO.QuestList.Count || quests.Count == 0 || areaSO.QuestList.Count == 0)
		{
			Debug.LogError("SOMETHING WENT WRONG, QUESTS COUNT IS NOT THE SAME AS THE DATA");
			return;
		}

		for (int i = 0; i < quests.Count; ++i)
		{
			quests[i].Init(areaSO.QuestList[i], this);

			if (areaSO.QuestList[i].IsQuestMandatory)
			{
				mandatoryQuests.Add(quests[i]);
			}
		}

		// character cards
		foreach (var characterData in charactersAvailable.AvailableCharacters)
		{
			var characterSelectCard = Instantiate(characterSelectCardDraggableItemPrefab, characterContainer.transform).GetComponent<CharacterSelectCardButton>();
			characterSelectCard.Init(characterData, this);
			characterSelectCards.Add(characterSelectCard);
		}

		characterContainer.Init(this);

		playButton.interactable = false;

		OnComplete = onComplete;

		this.gameObject.SetActive(false);
	}

	public void CharacterSelected(CharacterSelectCardButton selectedCharacter)
	{
		currentSelectedCharacter = selectedCharacter;

		foreach (var characterSelectCard in characterSelectCards)
		{
			characterSelectCard.Button.interactable = false;
		}

		ShowCharacterStats(currentSelectedCharacter.CharacterData);

		SelectQuest();
	}

	public void SelectQuest()
	{
		var isButtonSelected = false;

		foreach (var quest in quests)
		{
			quest.Button.interactable = true;

			if (!isButtonSelected && !quest.IsAssignedCharacter)
			{
				isButtonSelected = true;
				EventSystem.current.SetSelectedGameObject(quest.gameObject);
			}
		}

		if (!isButtonSelected)
		{
			EventSystem.current.SetSelectedGameObject(quests[0].gameObject);
		}
	}

	public void CharacterChosenForQuest()
	{
		currentSelectedCharacter.gameObject.SetActive(false);
		currentSelectedCharacter = null;

		foreach (var characterSelectCard in characterSelectCards)
		{
			characterSelectCard.Button.interactable = true;
		}

		CheckIfCanPlay();

		characterContainer.SelectFirstCharacter();
	}

	public void CheckIfCanPlay()
	{
		var canPlay = true;

		foreach (var mandatoryQuest in mandatoryQuests)
		{
			if (!mandatoryQuest.IsAssignedCharacter)
			{
				canPlay = false;
				break;
			}
		}

		playButton.interactable = canPlay;
	}


	#region Character Stats Display
	public void ShowCharacterStats(PlayerCharacterScriptableObject character)
	{
		characterSelectionStats.DisplayCharacterStats(character);
	}

	public void HideCharacterStats()
	{
		if (currentSelectedCharacter != null) return;

		characterSelectionStats.Hide();
	}
	#endregion

	public void PlayButton()
	{
		foreach (var mandatoryQuest in mandatoryQuests)
		{
			if (!mandatoryQuest.IsAssignedCharacter)
			{
				Debug.LogError("mandatory quest doesnt have character");
				return;
			}
		}

		for (int i = 0; i < quests.Count; ++i)
		{
			areaSO.QuestList[i].CharacterPlayingQuest = quests[i].CharacterPlayingQuest;
		}

		uiManager.FadeIntoBlack(false, OnComplete);
	}

	public void BackButton()
	{
		uiManager.FadeIntoBlack(true, onComplete: () =>
		{
			this.gameObject.SetActive(false);
			raBase.gameObject.SetActive(true);
			uiManager.FadeFromBlack(true);
		});
	}
}
