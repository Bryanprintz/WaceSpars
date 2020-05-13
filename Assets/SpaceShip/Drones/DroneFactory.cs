using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DroneFactory : MonoBehaviourPun
{
    [SerializeField] private float spawnRate = 5;
    [SerializeField] private float spawnAmount = 3;
    [SerializeField] private Drone drone;
    [SerializeField] private Canvas canvas;

    [SerializeField] private Text droneCounterText;
    
    private int dronesReady = 0;
    [SerializeField] private int maxDroneAmount;

    private void Awake()
    {
        StartCoroutine(SpawnDrone());
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            canvas.transform.SetParent(null);
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        droneCounterText.text = dronesReady.ToString();
    }

    public void ReleaseDrones()
    {
        for (int i = 0; i < dronesReady; i++)
        {
            var randomX = Random.Range(-3, 0);
            var randomY = Random.Range(-3, 0);
            var posOffset = new Vector3(randomX, randomY, 0);

            var newDrone = PhotonNetwork.Instantiate(drone.name, gameObject.transform.position + posOffset,
                Quaternion.identity);

            newDrone.GetComponent<Drone>().SetTarget(gameObject.transform);
        }

        dronesReady = 0;
    }


    private IEnumerator SpawnDrone()
    {
        for (var i = 0; i < spawnAmount; i++)
        {
            if (dronesReady < maxDroneAmount)
            {
                dronesReady++;
            }
        }

        yield return new WaitForSeconds(spawnRate);

        StartCoroutine(SpawnDrone());
    }
}