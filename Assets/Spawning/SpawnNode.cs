using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SpawnNode : MonoBehaviour
{
	[SerializeField] private List<Transform> Nodes = new List<Transform>();

	private void Awake()
	{
		SpawnEvents.RespawnRandomEvent += RandomSpawnNode;
	}

	private void OnDisable()
	{
		SpawnEvents.RespawnRandomEvent -= RandomSpawnNode;
	}

	private Transform RandomSpawnNode()
	{
		return Nodes[Random.Range(0, Nodes.Count - 1)];
	}
}