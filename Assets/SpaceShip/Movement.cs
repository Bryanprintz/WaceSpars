using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
public class Movement : MonoBehaviourPun
{
	[SerializeField] private float speed;

	private Rigidbody2D rb;
	private Camera cam;

	private void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		cam = Camera.main;
	}

	private void Start()
	{
		//logic
	}

	private void Update()
	{
		if (!photonView.IsMine) return;

		Move();
		RotateToMouse();
	}

	private void Move()
	{
		var horizontal = Input.GetAxis("Horizontal");
		var vertical = Input.GetAxis("Vertical");
		var moveDirection = new Vector3(horizontal, vertical);

		transform.rotation = Quaternion.Euler(moveDirection.x, moveDirection.y, 0);
		rb.velocity = moveDirection * speed;
	}

	private void RotateToMouse()
	{
		var diff = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		diff.Normalize();

		var rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
	}
}