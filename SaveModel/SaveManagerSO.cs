using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "SaveManagerSO", menuName = "ScriptableObjects/Managers/SaveManager")]
public class SaveManagerSO : ScriptableObject
{
	[Header("Debugging")]
	[SerializeField] private bool disableDataPersistence = false;
	[SerializeField] private bool initializeDataIfNull = false;
	[SerializeField] private bool overrideSelectedProfileId = false;
	[SerializeField] private string testSelectedProfileId = "test";

	[Header("File storage config")]
	[SerializeField] private string fileName;
	[SerializeField] private bool useEncryption;

	private GameSaveData gameSaveData = null;
	private List<ISaveDataObject> gameSaveDataObjectList;
	private FileDataHandler fileDataHandler;

	private string selectedProfileId = "";

	public void Init()
	{
		gameSaveData = null;
		this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption); 
		this.gameSaveDataObjectList = FindAllGameSaveObjects();
		InitializeSelectedProfileId();
	}

	public void NewGame()
	{
		this.gameSaveData = new GameSaveData();
	}

	public void LoadGame()
	{
		// return right away if data persistence is disabled
		if (disableDataPersistence)
		{
			return;
		}

		this.gameSaveData = fileDataHandler.Load(selectedProfileId);

		// start a new game if the data is null and we're configured to initialize data for debugging purposes
		if (this.gameSaveData == null && initializeDataIfNull)
		{
			NewGame();
		}

		if (this.gameSaveData == null)
		{
			Debug.LogError("No data was found. Starting new Game");
			return;
		}

		foreach(ISaveDataObject saveDataObject in this.gameSaveDataObjectList)
		{
			saveDataObject.LoadData(gameSaveData);
		}
	}

	public void SaveGame()
	{
		// return right away if data persistence is disabled
		if (disableDataPersistence)
		{
			return;
		}

		// if we don't have any data to save, log a warning here
		if (this.gameSaveData == null)
		{
			Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
			return;
		}

		foreach (ISaveDataObject saveDataObject in this.gameSaveDataObjectList)
		{
			saveDataObject.SaveData(gameSaveData);
		}

		// timestamp the data so we know when it was last saved
		gameSaveData.lastUpdated = System.DateTime.Now.ToBinary();

		fileDataHandler.Save(gameSaveData, selectedProfileId);
	}

	public bool HasGameSaveData()
	{
		return gameSaveData != null;
	}

	public Dictionary<string, GameSaveData> GetAllProfiles()
	{
		return fileDataHandler.LoadAllProfiles();
	}

	public void ChangeSelectedProfileId(string newProfileId)
	{
		this.selectedProfileId = newProfileId;
	}

	public List<ISaveDataObject> FindAllGameSaveObjects()
	{
		IEnumerable<ISaveDataObject> saveDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveDataObject>();
		var result = new List<ISaveDataObject>(saveDataObjects);
		return result;
	}

	private void InitializeSelectedProfileId()
	{
		this.selectedProfileId = fileDataHandler.GetMostRecentlyUpdatedProfileId();
		if (overrideSelectedProfileId)
		{
			this.selectedProfileId = testSelectedProfileId;
			Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
		}
		this.gameSaveData = fileDataHandler.Load(selectedProfileId);
	}

	public void DeleteProfileData(string profileId)
	{
		// delete the data for this profile id
		fileDataHandler.Delete(profileId);
		// initialize the selected profile id
		InitializeSelectedProfileId();
		// reload the game so that our data matches the newly selected profile id
		LoadGame();
	}
}