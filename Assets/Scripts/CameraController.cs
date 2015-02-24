using UnityEngine;
using System.Collections;

public class CameraController: MonoBehaviour {


	public GameObject selectHighLight;
	public Camera selectedCamera;
	public Transform camInit;
	public Transform camMaxY;
	public Transform camMinY;
	public Transform right;
	public Transform forward;
	public float pinchSpeed = 0.2f;
	
	
	private Vector2 prevDist = new Vector2(0,0);
	private Vector2 curDist = new Vector2(0,0);
	private Vector2 moveVec;
	private Vector3 camToHit;
	private Vector3 moveBack;
	private float touchDelta = 0.0F;
	private float rightTan;
	private float forwardTan;
	private float backTan;

	private float currentRightLimit;
	private float currentForwarLimit;
	private float camToOrigHeight;


	bool isReady;
	bool remainSelect; //if moveScreen
	Vector2 initInputPos;
	Vector3 camOrg;
	void Start()
	{
		rightTan   = right.position.x   / (camMaxY.position.y - camMinY.position.y); 
		forwardTan = forward.position.z / (camMaxY.position.y - camMinY.position.y); 
	}
	void Update () 
	{
		
		

		if(Input.touchCount == 1  )
		{


			if(Input.GetTouch(0).phase == TouchPhase.Ended && remainSelect == false)
			{
				Ray ray = selectedCamera.ScreenPointToRay((Input.GetTouch(0).position));
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 35.5f))
				{
					selectHighLight.transform.position = hit.collider.gameObject.transform.position;
				}
			}
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{

					camOrg       = selectedCamera.transform.position;
					initInputPos = Input.GetTouch(0).position;
					isReady      = true;
					remainSelect = false;
			}
			if(Input.GetTouch(0).phase == TouchPhase.Moved)
			{

				camToOrigHeight    = (camMaxY.position.y - selectedCamera.transform.position.y);
				currentRightLimit  = camToOrigHeight * rightTan;
				currentForwarLimit = camToOrigHeight * forwardTan;
				print(moveVec.magnitude);
				moveVec = (initInputPos - Input.GetTouch(0).position) * 0.02f;
				if(moveVec.magnitude > 0.3f)
				    remainSelect = true;
				if(isReady)
			        selectedCamera.transform.position = new Vector3 (
															Mathf.Clamp(camOrg.x + moveVec.x, -currentRightLimit, currentRightLimit ),
															selectedCamera.transform.position.y,
															Mathf.Clamp(camOrg.z + moveVec.y, -currentForwarLimit , currentForwarLimit)
														    );
			}


		}

		else if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) 
		{
			remainSelect = false;
			isReady = false;
			Ray ray = selectedCamera.ScreenPointToRay((Input.GetTouch(0).position + Input.GetTouch(1).position )/ 2);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 35.5f))
			{
				Debug.DrawLine(ray.origin, hit.point);
				curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
				prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
				touchDelta = curDist.magnitude - prevDist.magnitude;
				
				
				//two finger dist decrease
				if ((touchDelta  <= 0) )
				{
					moveBack = (camInit.transform.position - selectedCamera.transform.position).normalized * pinchSpeed;
					if(selectedCamera.transform.position.y != camInit.position.y)
						selectedCamera.transform.position = new Vector3( moveBack.x + selectedCamera.transform.position.x,
						                                                Mathf.Clamp(moveBack.y + selectedCamera.transform.position.y ,camMinY.position.y,camInit.position.y),
						                                                moveBack.z + selectedCamera.transform.position.z
						                                                );
					
				}
				//two finger dist increase
				else if ((touchDelta > 0) )
				{
					camToHit  = (hit.point - selectedCamera.transform.position).normalized * pinchSpeed;
					selectedCamera.transform.position = new Vector3( Mathf.Clamp(selectedCamera.transform.position.x + camToHit.x,-right.position.x,right.position.x),
					                                                Mathf.Clamp(selectedCamera.transform.position.y + camToHit.y,camMinY.position.y,camInit.position.y),
					                                                Mathf.Clamp(selectedCamera.transform.position.z + camToHit.z,-forward.position.z,forward.position.z)
					                                                );
				}
			}

		}        
	}
	public bool showGizmo=true;
	void OnDrawGizmos(){
		if(showGizmo){

			
			Gizmos.color=Color.yellow;
			Gizmos.DrawLine(camMaxY.position , right.position + forward.position + camMinY.position);
			Gizmos.DrawLine(camMaxY.position , right.position - forward.position + camMinY.position);
			Gizmos.DrawLine(camMaxY.position , -right.position + forward.position + camMinY.position);
			Gizmos.DrawLine(camMaxY.position , -right.position - forward.position + camMinY.position);
			Gizmos.DrawLine(right.position + forward.position + camMinY.position , right.position - forward.position + camMinY.position);
			Gizmos.DrawLine(right.position - forward.position + camMinY.position , -right.position - forward.position + camMinY.position);
			Gizmos.DrawLine(-right.position - forward.position + camMinY.position ,-right.position + forward.position + camMinY.position);
			Gizmos.DrawLine(-right.position + forward.position + camMinY.position , right.position + forward.position + camMinY.position);
		}
	}
	


}
