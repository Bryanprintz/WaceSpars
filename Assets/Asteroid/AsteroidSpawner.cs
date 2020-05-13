using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField] private float spawnRate = 5;
	[SerializeField] private float spawnAmount = 3;
	[SerializeField] private List<Asteroid> asteroids = new List<Asteroid>();

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			StartCoroutine(SpawnAsteroids());
		}
	}

	private IEnumerator SpawnAsteroids()
	{
		for (var i = 0; i < spawnAmount; i++)
		{
			var asteroid = asteroids[Random.Range(0, asteroids.Count)];

			var randomX = Random.Range(-50.0f, 50.0f);
			var randomY = Random.Range(-50.0f, 50.0f);
			var comet = PhotonNetwork.Instantiate(asteroid.name, new Vector3(randomX,randomY,0), Quaternion.identity);
			comet.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomX,randomY));
		}

		yield return new WaitForSeconds(spawnRate);
		
		StartCoroutine(SpawnAsteroids());
	}
}