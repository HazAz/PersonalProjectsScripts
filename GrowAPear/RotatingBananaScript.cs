using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBananaScript : MonoBehaviour
{
	private Transform target;
	[SerializeField] private float speed = 120f;

	public void Init(Transform playerTransform)
	{
		target = playerTransform;
	}

	// Update is called once per frame
	void Update()
	{
		if (target == null) return;

		transform.position = target.position;
		transform.RotateAround(target.position, Vector3.forward, speed * Time.deltaTime);
	}
}
