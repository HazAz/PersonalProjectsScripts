using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupTypes
{
	Banana, // bananapoleon 1
	Cucumber, // hercucumber 2
	BokChoy, // usain bok 3
	Plum, // Plumario 4
	Chili, // Achilis 5
	Peppers, // Kylian mbappe 6 
	Grape, // Alexander the grape 7
	Apple, // Hippocratapple 8
	Carrot, // Micarrot jordan 9
	Berry, // Alberry Einstein 10
	Orange, // Orangeheimer 11
	Kiwi, // Kiwianu Reeves 12
	Broccoli, // Mohammed Broccoli 13
}

public class PowerupScripts : MonoBehaviour
{
	[SerializeField] private PowerupPanelScript powerupPanelScript;
	[SerializeField] private PearSeedPanel gainedSeedPanel;
	[SerializeField] private PlayerPowerup playerPowerup;
	[SerializeField] private bool isLevel1 = true;

	private void Start()
	{
		if (isLevel1 || StaticPowerupScript.AvailablePowerups.Count == 0)
		{
			StaticPowerupScript.NewGameStart();
		}
		else
		{
			StaticPowerupScript.EnteredNewLevel();
		}

		foreach (var powerup in StaticPowerupScript.AcquiredPowerups)
		{
			playerPowerup.ApplyPowerups(powerup);
		}
	}

	public void CreatePowerupPanelScript(Action onComplete = null)
	{
		if (StaticPowerupScript.AvailablePowerups.Count == 0)
		{
			onComplete?.Invoke();
			return;
		}

		List<PowerupTypes> availablePowerups = new List<PowerupTypes>(StaticPowerupScript.AvailablePowerups);
		var powerup1 = availablePowerups[UnityEngine.Random.Range(0, availablePowerups.Count)];
		availablePowerups.Remove(powerup1);
		var powerup2 = availablePowerups[UnityEngine.Random.Range(0, availablePowerups.Count)];

		powerupPanelScript.Init(powerup1, powerup2, this, onComplete);
	}

	public void ApplyPowerup(PowerupTypes powerupType)
	{
		StaticPowerupScript.AddPowerupInLevel(powerupType);
		playerPowerup.ApplyPowerups(powerupType);
	}

	public void CreateGainedSeedPanel(Action onComplete = null)
	{
		if (gainedSeedPanel == null)
		{
			onComplete?.Invoke();
			return;
		}

		gainedSeedPanel.gameObject.SetActive(true);
		gainedSeedPanel.Init(onComplete);
	}
}
