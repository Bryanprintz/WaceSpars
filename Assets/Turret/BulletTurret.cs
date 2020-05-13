using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletTurret : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private ForceMode2D forceMode;
    [SerializeField] private float damage;
    private Rigidbody2D rb;
    private TurretGun origin;
    
    
    public void Setup(TurretGun origin)
    {
        rb = GetComponent<Rigidbody2D>();
        this.origin = origin;
        rb.AddForce(transform.up * force, forceMode);
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == origin.gameObject) return;
        
        var t = other.gameObject.GetComponent<BulletTurret>();
        if (t != null) return;

        if (origin.view.IsMine)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                DealDamage(damageable);
            }
        }

        Destroy(this.gameObject);
    }
    
    private void DealDamage(IDamageable damageable)
    {
        damageable.ApplyDamage(damage);
    }
}
