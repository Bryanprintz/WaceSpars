using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
public class Asteroid : MonoBehaviourPun
{
	[SerializeField] private int ep;
	[SerializeField] private int maxNewAstroids = 3;
	[SerializeField] private Asteroid SmallAsteroid;
	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void OnDeath(Player lastHit)
	{
		var ship = (Spaceship) lastHit.TagObject;
		ship.GainEP(ep);
		FactoryCreate();
	}

	private void FactoryCreate()
	{
		if (SmallAsteroid == null) return;

		for (var i = 0; i < maxNewAstroids; i++)
		{
			var randomX = Random.Range(-50.0f, 50.0f);
			var randomY = Random.Range(-50.0f, 50.0f);
			var newComet = PhotonNetwork.Instantiate(SmallAsteroid.name, transform.position,
												 Quaternion.identity);
			
			newComet.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomX,randomY));
			
		}
	}
}