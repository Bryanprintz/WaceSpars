using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TurretGun : MonoBehaviour
{
    [SerializeField] private BulletTurret bullet;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private float fireRate;
    private float nextFire;
    private Spaceship target;
    
    public PhotonView view;
    public bool rotateParent;
    public bool CanShoot => Time.time >= nextFire;


    private void Start()
    {
        nextFire = Time.time;
    }

    private void Update()
    {
        if (target==null) return;

        if (rotateParent)
        {
            GetComponentInParent<Turret>().RotateTowards(target.transform.position);
        }

        if (CanShoot)
        {
            Fire();
            //currentGunParticle.Play();
            GetComponent<Animation>().Play("GunShoot");
				
        }
    }

    public void Fire()
    {
        if (CanShoot)
        {
            view.RPC("FireInternal",
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && target.gameObject == other.gameObject)
        {
            target = null;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && target == null)
        {
            target = other.gameObject.GetComponent<Spaceship>();
        }
    }

    
}
