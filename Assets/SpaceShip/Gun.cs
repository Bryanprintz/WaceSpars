using Photon.Pun;
using UnityEngine;

public class Gun : MonoBehaviourPun
{
	[SerializeField] private Bullet bullet;
	[SerializeField] private Transform bulletOrigin;
	[SerializeField] private float fireRate;
	private float nextFire;
	public bool CanShoot => Time.time >= nextFire;

	private void Start()
	{
		nextFire = Time.time;
	}

	public void Fire()
	{
		if (CanShoot)
		{
			photonView.RPC("FireInternal",
						   RpcTarget.AllViaServer,
						   bulletOrigin.position,
						   bulletOrigin.eulerAngles);
			nextFire = fireRate + Time.time;
		}
	}

	[PunRPC]
	void FireInternal(Vector3 startPos, Vector3 startDir)
	{
		var newBullet = Instantiate(bullet, startPos, Quaternion.Euler(startDir));
		newBullet.Setup(this);
	}
}