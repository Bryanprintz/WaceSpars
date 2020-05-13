using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Gun))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(DroneFactory))]
public class Spaceship : MonoBehaviourPun
{
	private int currentEP = 0;
	private int currentLevel = 1;
	private Gun gun;
	private Health health;
	private DroneFactory droneFactory;

	private void Start()
	{
		gun = GetComponent<Gun>();
		health = GetComponent<Health>();
		droneFactory = GetComponent<DroneFactory>();
		photonView.Owner.TagObject = this;
	}

	public void GainEP(int amount)
	{
		photonView.RPC("GainEPInternal", RpcTarget.AllViaServer, amount);
	}

	[PunRPC]
	private void GainEPInternal(int amount)
	{
		currentEP += amount;

		//TODO level stuff
	}

	private void Update()
	{
		if (!photonView.IsMine) return;

		if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
		{
			gun.Fire();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			droneFactory.ReleaseDrones();
		}
	}
}