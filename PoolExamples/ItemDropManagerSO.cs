using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDropManager", menuName = "ScriptableObjects/Managers/ItemDropManagerSO")]
public class ItemDropManagerSO : ScriptableObject
{
	[SerializeField] private ItemDataManagerSO itemDataManager;
	[SerializeField] private GameObject itemDropPrefab;
	[SerializeField] private Sprite healthSprite;
	[SerializeField] private Sprite coinSprite;
	[SerializeField] private Sprite gemSprite;

	// For the pool
	private const int maxNumber = 40;
	private Transform parentTransform;
	private int count;
	private List<PickupItemScript> pickupItemList = new();

	public void Init()
	{
		count = 0;

		if (parentTransform != null)
		{
			Destroy(parentTransform.gameObject);
		}

		pickupItemList.Clear();

		parentTransform = new GameObject("ItemPickup").transform;
		parentTransform.position = Vector3.zero;

		for (int i = 0; i < maxNumber; ++i)
		{
			var pickupItem = Instantiate(itemDropPrefab, parentTransform);
			pickupItemList.Add(pickupItem.GetComponent<PickupItemScript>());
			pickupItem.SetActive(false);
		}
	}

	public void TryDrop(ItemDropChancesData itemDropData, Vector3 position)
	{
		TrySpawn(PickupType.Gems ,itemDropData.ExpDropChance, itemDropData.ExpDropAmount, position, gemSprite);
		TrySpawn(PickupType.Coins, itemDropData.CoinDropChance, (uint)Random.Range(itemDropData.CoinDropAmountMin, itemDropData.CoinDropAmountMin), position, coinSprite);
		TrySpawn(PickupType.Health, itemDropData.HealthDropChance, itemDropData.HealthDropAmount, position, healthSprite);
		TrySpawnItem(itemDropData, position);
	}

	private void TrySpawn(PickupType pt, float chance, uint amount, Vector3 position, Sprite sprite)
	{
		if (Random.value <= chance)
		{
			SpawnItem(pt, amount, position, sprite);
		}
	}

	private void TrySpawnItem(ItemDropChancesData itemDropData, Vector3 position)
	{
		if (Random.value > itemDropData.ItemDropChance)
		{
			return;
		}

		var itemDropChanceByRarityList = itemDropData.ItemDropChanceByRarity;
		var totalWeight = itemDropData.ItemDropChanceByRarity.Sum();
		var randomWeightedIndex = Random.Range(0, totalWeight);
		var weightedIndex = 0f;
		int index;

		for (index = 0; index < itemDropChanceByRarityList.Count; ++index)
		{
			weightedIndex += itemDropChanceByRarityList[index];
			if (randomWeightedIndex < weightedIndex)
			{
				break;
			}
		}

		switch (index)
		{
			case 0:
				SpawnItem(PickupType.Item, 1, position, itemData: itemDataManager.GetRandomItemByRarity(ItemRarity.Common));
				break;
			case 1:
				SpawnItem(PickupType.Item, 1, position, itemData: itemDataManager.GetRandomItemByRarity(ItemRarity.Uncommon));
				break;
			case 2:
				SpawnItem(PickupType.Item, 1, position, itemData: itemDataManager.GetRandomItemByRarity(ItemRarity.Rare));
				break;
			case 3:
				SpawnItem(PickupType.Item, 1, position, itemData: itemDataManager.GetRandomItemByRarity(ItemRarity.Epic));
				break;
			case 4:
				SpawnItem(PickupType.Item, 1, position, itemData: itemDataManager.GetRandomItemByRarity(ItemRarity.Legendary)); 
				break;
		}
	}

	public void SpawnItem(PickupType pt, uint amount, Vector3 spawnPosition, Sprite sprite = null, ItemSO itemData = null)
	{
		var currentItem = pickupItemList[count];

		currentItem.gameObject.SetActive(true);
		currentItem.transform.position = spawnPosition;
		currentItem.Init(pt, amount, sprite, itemData);

		++count;

		if (count >= maxNumber)
		{
			count = 0;
		}
	}
}
