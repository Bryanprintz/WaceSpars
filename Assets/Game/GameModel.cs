using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[RequireComponent(typeof(ReadyOnSceneLoaded))]
public class GameModel : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameSpawn gameSpawn = null;
	private bool spawned;

	private void Awake()
	{
		gameSpawn = GetComponent<GameSpawn>();
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.CurrentRoom.IsOpen = false;
			PhotonNetwork.CurrentRoom.IsVisible = false;
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		if (!PhotonNetwork.IsMasterClient) return;
		var players = PhotonNetwork.CurrentRoom.Players.Values;

		if (players.Any(player => !player.IsReady()))
		{
			return;
		}

		if (!spawned)
		{
			gameSpawn.SpawnAll();
			spawned = true;
		}
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(0);
	}
}