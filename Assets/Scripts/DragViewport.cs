using UnityEngine;
using System.Collections;

public class DragViewport : MonoBehaviour {


	public float dragSpeed = 2;
	private Vector3 dragOrigin;
	private Vector3 camOrigin;
	public Transform up;
	public Transform right;

	Vector2 viewport;
	Vector2 moveVec;


	void Update()
	{

		
		print (Camera.main);
	
		if(Input.touchCount == 1)
		{
			if( Input.GetTouch(0).phase == TouchPhase.Began)
			{
				dragOrigin = Input.mousePosition;
				camOrigin  = Camera.main.transform.position;
			}

			moveVec = ( dragOrigin - Input.mousePosition ) * 0.02f;

			Camera.main.transform.position = new Vector3 
				(
					Mathf.Clamp(camOrigin.x + moveVec.x, -right.position.x , right.position.x),
					Camera.main.transform.position.y,
					Mathf.Clamp(camOrigin.z + moveVec.y, -up.position.z, up.position.z)
				);
		}




	}



}
