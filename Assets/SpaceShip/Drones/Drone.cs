using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Drone : MonoBehaviourPun
{
    [SerializeField] private float lifetime;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float distToTarget = 2f;
    
    
    private Transform target;

    private void Awake()
    {
        StartCoroutine(DestroyTimer());
    }

    private void Update()
    {
        if (target == null) return;
        transform.LookAt(target.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);


        if (Vector3.Distance(transform.position, target.position) > distToTarget)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(lifetime);

        PhotonNetwork.Destroy(gameObject);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}