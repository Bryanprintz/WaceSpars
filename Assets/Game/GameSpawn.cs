using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameSpawn : MonoBehaviour
{
	public void SpawnAll()
	{
		if (!PhotonNetwork.IsMasterClient) return;

		Player[] photonPlayerList = PhotonNetwork.PlayerList;

		foreach (var pp in photonPlayerList)
		{
			SingleSpawn(pp);
		}
	}

	public void SingleSpawn(Player player)
	{
		SpawnEvents.SpawnRandom(player);
	}
}