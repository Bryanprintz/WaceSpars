using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerFactory : MonoBehaviourPunCallbacks, IOnEventCallback
{
	[SerializeField] private Spaceship playerPrefab = null;
	[SerializeField] private CameraController cam;

	private void Awake()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	public void OnEvent(EventData photonEvent)
	{
		switch (photonEvent.Code)
		{
			case SpawnEventProperties.Spawn:
				var data = (object[]) photonEvent.CustomData;
				var position = (Vector3) data[0];
				var rotation = (Quaternion) data[1];

				Create(position, rotation);
				break;
		}
	}

	private void Create(Vector3 spawnPosition, Quaternion spawnRotation)
	{
		var playerObject = PhotonNetwork.Instantiate(playerPrefab.name,
													 spawnPosition,
													 spawnRotation, 0);
		SetUpComponents(playerObject);
	}

	private void SetUpComponents(GameObject player)
	{
		cam.SetTarget(player);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		PhotonNetwork.RemoveCallbackTarget(this);
	}
}