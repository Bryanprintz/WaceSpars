using UnityEngine;

public class CameraController : MonoBehaviour
{
	private GameObject target;

	public void SetTarget(GameObject obj)
	{
		target = obj;
	}

	public void Update()
	{
		if (target == null) return;
		Follow();
	}

	private void Follow()
	{
		//follow stuff here
		var camPos = new Vector3(target.transform.position.x,target.transform.position.y,transform.position.z);
		
		gameObject.transform.position = camPos;
	}
}