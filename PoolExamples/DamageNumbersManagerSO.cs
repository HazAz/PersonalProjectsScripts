using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageNumbersManager", menuName = "ScriptableObjects/Managers/DamageNumbersManager")]
public class DamageNumbersManagerSO : ScriptableObject
{
	[SerializeField] private GameObject DamageNumberPrefab;

	private int count;
	private const int maxNumber = 25;

	private List<DamageNumbersUI> damageNumbers = new();
	private Transform parentTransform;

	public void Init(Transform questParentTransform)
	{
		count = 0;

		if (parentTransform != null)
		{
			Destroy(parentTransform.gameObject);
		}

		damageNumbers.Clear();

		parentTransform = new GameObject("DamageNumbersParent").transform;
		parentTransform.parent = questParentTransform;

		for (int i = 0; i < maxNumber; ++i)
		{
			var damageNumber = Instantiate(DamageNumberPrefab, parentTransform);
			damageNumbers.Add(damageNumber.GetComponent<DamageNumbersUI>());
			damageNumber.SetActive(false);
		}
	}

	public void CreateDamageNumber(int amount, Vector3 position)
	{
		var currentDamageNumber = damageNumbers[count];

		currentDamageNumber.gameObject.SetActive(true);
		currentDamageNumber.transform.position = position;
		currentDamageNumber.Init($"{amount}");
		++count;

		if (count >= maxNumber)
		{
			count = 0;
		}
	}
}
