using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
	[SerializeField] private float force;
	[SerializeField] private ForceMode2D forceMode;
	[SerializeField] private float damage;
	private Rigidbody2D rb;
	private Gun origin;

	public void Setup(Gun origin)
	{
		rb = GetComponent<Rigidbody2D>();
		this.origin = origin;
		rb.AddForce(transform.up * force, forceMode);
		
		Destroy(gameObject,1);
	}
	

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other.gameObject);
		if (other.gameObject == origin.gameObject) return;

		if (origin.photonView.IsMine)
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