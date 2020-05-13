using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickLobby : MonoBehaviourPunCallbacks
{
	[SerializeField] private int maxPlayerCount = 2;
	[SerializeField] private int sceneToLoad = 2;
	[SerializeField] private TextMeshProUGUI stats;
	[SerializeField] private Button quickStart;

	private void Start()
	{
		quickStart.gameObject.SetActive(false);
		stats.text = "connecting..";
		PhotonNetwork.AutomaticallySyncScene = true;
		Conenct();
	}

	private void Conenct()
	{
		if (PhotonNetwork.IsConnected)
		{
			JoinOrCreateRoom();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	public override void OnConnectedToMaster()
	{
		base.OnConnectedToMaster();
		stats.text = "creating room..";
		JoinOrCreateRoom();
	}

	private void JoinOrCreateRoom()
	{
		var roomName = new Guid().ToString();

		var roomOptions = new RoomOptions
		{
			IsOpen = true,
			IsVisible = true,
			MaxPlayers = (byte) maxPlayerCount,
		};

		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		JoinOrCreateRoom();
	}

	public override void OnJoinedRoom()
	{
		UpdateDisplay();
		if (PhotonNetwork.IsMasterClient)
		{
			quickStart.gameObject.SetActive(true);
			quickStart.onClick.AddListener(LoadSceneOnFullRoom);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		UpdateDisplay();

		if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayerCount)
		{
			LoadSceneOnFullRoom();
		}
	}

	private void LoadSceneOnFullRoom()
	{
		if (!PhotonNetwork.IsMasterClient) return;

		PhotonNetwork.LoadLevel(sceneToLoad);
	}

	private void UpdateDisplay()
	{
		var currentCount = PhotonNetwork.CurrentRoom.PlayerCount;
		stats.text = $"{currentCount}/{maxPlayerCount}";
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		UpdateDisplay();
	}
}