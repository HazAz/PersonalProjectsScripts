using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private MosquitoScript mosquitoPrefab;
	[SerializeField] private SkeeterBossScript skeeterBossPrefab;
	[SerializeField] private MosquitoBulletScript bulletPrefab;

	[SerializeField] private AntScript antPrefab;
	[SerializeField] private AntBossScript antBossPrefab;

	[SerializeField] private BeeScript beePrefab;
	[SerializeField] private BeeBossScript beeBossPrefab;

	[SerializeField] private int maxMosquitoCountWave1= 5;
	[SerializeField] private int maxMosquitoCountWave2 = 10;
	[SerializeField] private int maxAntCountWave1 = 5;
	[SerializeField] private int maxAntCountWave2 = 10;
	[SerializeField] private int maxBeeCountWave1 = 0;
	[SerializeField] private int maxBeeCountWave2 = 0;

	[SerializeField] private int minTimeToSpawn = 2;
	[SerializeField] private int maxTimeToSpawn = 5;

	[SerializeField] private List<Transform> mosquitoSpawnPositions = new();
	[SerializeField] private List<Transform> antSpawnPositions = new();

	[SerializeField] private AudioSource sceneAudioSource;
	[SerializeField] private AudioClip bossAudio;

	private int numMosquitoSpawned = 0;
	private int numAntSpawned = 0;
	private int numBeeSpawned = 0;

	private int totalEnemiesCountWave1;
	private int totalEnemiesCountWave2;
	private int currentTotalEnemiesCount;
	private int enemiesDead = 0;

	private int wave = 1;

	[SerializeField] private Transform player;
	[SerializeField] private PowerupScripts playerPowerupScripts;

	// Start is called before the first frame update
	void Start()
	{
		totalEnemiesCountWave1 = maxMosquitoCountWave1 + maxAntCountWave1 + maxBeeCountWave1;
		totalEnemiesCountWave2 = maxMosquitoCountWave2 + maxAntCountWave2 + maxBeeCountWave2;
		currentTotalEnemiesCount = totalEnemiesCountWave1;
		StartCoroutine(SpawnerCoroutine());
	}

	IEnumerator SpawnerCoroutine()
	{
		while (player == null)
		{
			yield return new WaitForSeconds(1f);
			player = GameObject.FindGameObjectWithTag("Player").transform;
			playerPowerupScripts = player.GetComponent<PowerupScripts>();
		}

		bool canSpawnMosquito = true;
		bool canSpawnAnt = true;
		bool canSpawnBee = true;

		int maxMosquitoCount = wave == 1 ? maxMosquitoCountWave1 : maxMosquitoCountWave2;
		int maxAntCount = wave == 1 ? maxAntCountWave1 : maxAntCountWave2;
		int maxBeeCount = wave == 1 ? maxBeeCountWave1 : maxBeeCountWave2;
		int totalEnemiesCount = wave == 1 ? totalEnemiesCountWave1 : totalEnemiesCountWave2;

		while (numMosquitoSpawned + numAntSpawned + numBeeSpawned < totalEnemiesCount)
		{
			yield return new WaitForSeconds(Random.Range(minTimeToSpawn, maxTimeToSpawn));

			if (numMosquitoSpawned >= maxMosquitoCount)
			{
				canSpawnMosquito = false;
			}

			if (numAntSpawned >= maxAntCount)
			{
				canSpawnAnt = false;
			}

			if (numBeeSpawned >= maxBeeCount)
			{
				canSpawnBee = false;
			}

			if (canSpawnMosquito && !canSpawnAnt && !canSpawnBee)
			{
				SpawnMosquito();
			}
			else if (!canSpawnMosquito && canSpawnAnt && !canSpawnBee)
			{
				SpawnAnt();
			}
			else if (!canSpawnMosquito && !canSpawnAnt && canSpawnBee)
			{
				SpawnBee();
			}
			else if (canSpawnMosquito && canSpawnAnt && !canSpawnBee)
			{
				var chance = Random.value;
				if (chance < 0.5f)
				{
					SpawnMosquito();
				}
				else
				{
					SpawnAnt();
				}
			}
			else if (canSpawnMosquito && !canSpawnAnt && canSpawnBee)
			{
				var chance = Random.value;
				if (chance < 0.5f)
				{
					SpawnMosquito();
				}
				else
				{
					SpawnBee();
				}
			}
			else if (!canSpawnMosquito && canSpawnAnt && canSpawnBee)
			{
				var chance = Random.value;
				if (chance < 0.5f)
				{
					SpawnAnt();
				}
				else
				{
					SpawnBee();
				}
			}
			else
			{
				var chance = Random.value;
				if (chance < 0.33f)
				{
					SpawnAnt();
				}
				else if (chance < 0.66f)
				{
					SpawnMosquito();
				}
				else
				{
					SpawnBee();
				}
			}
		}
	}

	private void SpawnMosquito()
	{
		var mosquito = Instantiate(mosquitoPrefab, GetPositionForMosquito(), Quaternion.identity);
		mosquito.Init(player, bulletPrefab, this);
		++numMosquitoSpawned;
	}

	private void SpawnAnt()
	{
		var ant = Instantiate(antPrefab, GetPositionForAnt(), Quaternion.identity);
		ant.Init(player, this);
		++numAntSpawned;
	}

	private void SpawnBee()
	{
		var bee = Instantiate(beePrefab, GetPositionForMosquito(), Quaternion.identity);
		bee.Init(player, this);
		++numBeeSpawned;
	}

	private Vector3 GetPositionForMosquito()
	{
		if (mosquitoSpawnPositions.Count == 0)
		{
			return Vector3.zero;
		}

		return mosquitoSpawnPositions[(int)Random.Range(0, mosquitoSpawnPositions.Count)].position;
	}

	private Vector3 GetPositionForAnt()
	{
		if (antSpawnPositions.Count == 0)
		{
			return Vector3.zero;
		}

		return antSpawnPositions[(int)Random.Range(0, antSpawnPositions.Count)].position;
	}

	public void EnemyDied()
	{
		enemiesDead++;

		if (enemiesDead == currentTotalEnemiesCount)
		{
			WaveEnded();
		}
	}

	private void WaveEnded()
	{
		if (wave < 3)
		{
			Invoke("CreatePowerupPanel", 3f);
		}
		else
		{
			Invoke("CreatePearSeedPanel", 3f);
		}
	}

	private void CompleteWave()
	{
		switch (wave)
		{
			case 1:
				++wave;
				ResetParams();
				currentTotalEnemiesCount = totalEnemiesCountWave2;
				StartCoroutine(SpawnerCoroutine());
				break;

			case 2:
				++wave;
				ResetParams();
				SpawnFinalBoss();
				break;

			case 3:
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				break;
		}
	}

	private void ResetParams()
	{
		enemiesDead = 0;
		numAntSpawned = 0;
		numMosquitoSpawned = 0;
		numBeeSpawned = 0;
	}

	private void SpawnFinalBoss()
	{
		var time = sceneAudioSource.time;
		sceneAudioSource.clip = bossAudio;
		sceneAudioSource.time = time;
		sceneAudioSource.Play();

		currentTotalEnemiesCount = 0;

		if (skeeterBossPrefab != null)
		{
			var skeeterBoss = Instantiate(skeeterBossPrefab, GetPositionForMosquito(), Quaternion.identity);
			skeeterBoss.Init(player, this);
			currentTotalEnemiesCount++;
		}

		if (antBossPrefab != null)
		{
			var antBoss = Instantiate(antBossPrefab, GetPositionForAnt(), Quaternion.identity);
			antBoss.Init(player, this);
			currentTotalEnemiesCount++;
		}

		if (beeBossPrefab != null)
		{
			var beeBoss = Instantiate(beeBossPrefab, GetPositionForMosquito(), Quaternion.identity);
			beeBoss.Init(player, this);
			currentTotalEnemiesCount++;
		}
	}

	private void CreatePowerupPanel()
	{
		playerPowerupScripts.CreatePowerupPanelScript(onComplete: CompleteWave);
	}

	private void CreatePearSeedPanel()
	{
		playerPowerupScripts.CreateGainedSeedPanel(onComplete: CompleteWave);
	}

}
