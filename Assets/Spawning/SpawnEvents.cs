using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnEventProperties
{
	public const byte Spawn = 20;
}

public static class SpawnEvents
{
	public static event Func<Transform> RespawnRandomEvent;

	public static void SpawnRandom(Player targetPlayer)
	{
		var spawnPoint = new Vector3(0, 0, 0);
		var spawnRotation = Quaternion.identity;

		var spawnNode = RespawnRandomEvent?.Invoke();
		Debug.Assert(spawnNode != null, "Missing Spawn Node");

		if (spawnNode != null)
		{
			spawnPoint = spawnNode.position;
			spawnRotation = spawnNode.rotation;
		}


		PhotonNetwork.RaiseEvent(SpawnEventProperties.Spawn,
								 new object[] {spawnPoint, spawnRotation},
								 new RaiseEventOptions
								 {
									 TargetActors = new int[] {targetPlayer.ActorNumber}
								 }, SendOptions.SendReliable);
	}
}