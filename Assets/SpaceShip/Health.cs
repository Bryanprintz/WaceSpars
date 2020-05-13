using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HealthChanged : UnityEvent<float, float> { }

[System.Serializable]
public class OnDeath : UnityEvent<Player> { }

[RequireComponent(typeof(PhotonView))]
public class Health : MonoBehaviour, IDamageable, IPunObservable
{
	public HealthChanged OnHealthChanged;
	public OnDeath OnDeath;
	[SerializeField] private float maxHealth = 1000;
	private float currentHealth;
	private PhotonView view;
	private Player lastHit;

	private void Awake()
	{
		view = GetComponent<PhotonView>();
	}

	private void Start()
	{
		HandleObservable();

		currentHealth = maxHealth;
		OnHealthChanged.Invoke(currentHealth, maxHealth);
	}

	private void HandleObservable()
	{
		if (!view.ObservedComponents.Contains(this))
		{
			view.ObservedComponents.Add(this);
		}
	}

	/// <summary>
	/// Send an damage RPC to the objects owner.
	/// </summary>
	/// <param name="amount">How much damage should be dealt.</param>
	public void ApplyDamage(float amount)
	{
		if (view.IsSceneView)
		{
			ApplyDamageInternal(amount,new PhotonMessageInfo(PhotonNetwork.LocalPlayer,PhotonNetwork.ServerTimestamp,view));
		}
		else
		{
			view.RPC(nameof(ApplyDamageInternal), view.Owner, amount);
		}
	}

	/// <summary>
	/// Send an heal RPC to the objects owner.
	/// </summary>
	public void ApplyHealth(float amount)
	{
		view.RPC(nameof(ApplyHealthInternal), view.Owner, amount);
	}

	[PunRPC]
	private void ApplyDamageInternal(float amount, PhotonMessageInfo info)
	{
		lastHit = info.Sender;
		currentHealth = Mathf.Clamp(currentHealth -= amount, 0, maxHealth);
		OnHealthChanged.Invoke(currentHealth, maxHealth);

		if (currentHealth <= 0)
		{
			ManageDeath();
		}
	}

	[PunRPC]
	private void ApplyHealthInternal(float amount)
	{
		currentHealth = Mathf.Clamp(currentHealth += amount, 0, maxHealth);

		OnHealthChanged.Invoke(currentHealth, maxHealth);
	}

	private void ManageDeath()
	{
		if (view.IsMine)
		{
			OnDeath?.Invoke(lastHit);
			PhotonNetwork.Destroy(gameObject);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(currentHealth);
		}
		else
		{
			currentHealth = (float) stream.ReceiveNext();
			OnHealthChanged.Invoke(currentHealth, maxHealth);
		}
	}
}