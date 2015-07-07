using UnityEngine;
using System.Collections;

public class CameraController: MonoBehaviour {


	public GameObject selectHighLight;
	public GameObject selectPanel;

	public Camera selectedCamera;
	public Transform camInit;

	public Transform camMinY;
	public Vector3 right;
	public Vector3 forward;
	public float pinchSpeed = 0.2f;
	public float moveSpeed = 0.04f;

	
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
	private float currentForwardLimit;
	private float currentBackLimit;
	private float camToOrigHeight;


	bool isReadyMove;
	bool remainSelect; //if moveScreen
	Vector2 initInputPos;
	Vector3 camOrg;
	Vector3 selectPos;
	void Start()
	{
		selectHighLight.SetActive(false);

		rightTan   = Mathf.Abs(( right.x - camInit.transform.position.x )    / (camInit.position.y - camMinY.position.y)); 
		forwardTan = Mathf.Abs(( forward.z - camInit.transform.position.z + camMinY.transform.position.z)  / (camInit.position.y - camMinY.position.y)); 
		backTan    =( -forward.z - camInit.transform.position.z + camMinY.transform.position.z) / (camInit.position.y - camMinY.position.y); 
		selectPos.y = 0.01f;
	}
	void Update () 
	{
		


		if(Input.touchCount == 1  && !UI.fixMoveCam)
		{

			selectPanel.SetActive(true);

			
			if(Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				
				moveVec = (initInputPos - Input.GetTouch(0).position) * moveSpeed;
				if(moveVec.magnitude > 0.3f)
					remainSelect = true;
				if(isReadyMove && selectedCamera.transform.position.y != camInit.transform.position.y)
				{
					selectedCamera.transform.position = new Vector3 (
						Mathf.Clamp(camOrg.x + moveVec.x, -currentRightLimit, currentRightLimit ),
						selectedCamera.transform.position.y,
						Mathf.Clamp(camOrg.z + moveVec.y, currentBackLimit , currentForwardLimit )
						);
				}
				
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				camOrg       = selectedCamera.transform.position;
				initInputPos = Input.GetTouch(0).position;
				isReadyMove      = true;
				remainSelect = false;
			}


			else if(Input.GetTouch(0).phase == TouchPhase.Ended && remainSelect == false)
			{
				Ray ray = selectedCamera.ScreenPointToRay((Input.GetTouch(0).position));
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 35.5f))
				{
				
					selectHighLight.transform.localScale = new Vector3(GridMgr.cursorSize,GridMgr.cursorSize,1);

					if(GridMgr.cursorSize == 2)
					{

						if(Mathf.Sign( hit.point.x ) == 1)
							selectPos.x = (int)(hit.point.x + 0.5f);
						else
							selectPos.x = (int)(hit.point.x - 0.5f);
						if(Mathf.Sign( hit.point.y ) == 1)
							selectPos.z = (int)(hit.point.z - 0.5f);
						else
							selectPos.z = (int)(hit.point.z + 0.5f);
						selectPanel.transform.position = selectPos;
					}
					else if(GridMgr.cursorSize == 3)
					{
						selectPos =  hit.transform.position;
						selectPanel.transform.position = selectPos;
					}
						
					else
						print("cursor size undefined");


				}
			}




		}

		else if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) 
		{
			remainSelect = false;
			isReadyMove = false;
			countCurrentLimit();
			Ray ray = selectedCamera.ScreenPointToRay((Input.GetTouch(0).position + Input.GetTouch(1).position )/ 2);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 35.5f))
			{
				Debug.DrawLine(ray.origin, hit.point);
				curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
				prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
				touchDelta = curDist.magnitude - prevDist.magnitude;
				
				
				//zoom out
				if ((touchDelta  <= 0) )
				{
					moveBack = (camInit.transform.position - selectedCamera.transform.position).normalized * pinchSpeed;

					selectedCamera.transform.position = new Vector3(    Mathf.Clamp( moveBack.x + selectedCamera.transform.position.x,-currentRightLimit, currentRightLimit ),
						                                                Mathf.Clamp(moveBack.y + selectedCamera.transform.position.y ,camMinY.position.y,20.0f),
					                                                    Mathf.Clamp(moveBack.z + selectedCamera.transform.position.z ,currentBackLimit , currentForwardLimit)
						                                                );
					
				}
				//zoom in
				else if ((touchDelta > 0) && selectedCamera.transform.position.y != camMinY.position.y)
				{
				
					camToHit  = (hit.point - selectedCamera.transform.position).normalized * pinchSpeed;
				
					selectedCamera.transform.position = new Vector3( Mathf.Clamp(selectedCamera.transform.position.x + camToHit.x,-currentRightLimit, currentRightLimit ),
					                                                Mathf.Clamp(selectedCamera.transform.position.y + camToHit.y,camMinY.position.y,camInit.position.y),
					                                                Mathf.Clamp(selectedCamera.transform.position.z + camToHit.z,currentBackLimit , currentForwardLimit)
					                                                );
				}
			}

		}        
	}

	void countCurrentLimit()
	{
		camToOrigHeight    = (camInit.position.y - selectedCamera.transform.position.y);


		currentRightLimit  = camToOrigHeight * rightTan;
		currentForwardLimit = camToOrigHeight * forwardTan + camInit.transform.position.z ;
		currentBackLimit   =  (camToOrigHeight * backTan + camInit.transform.position.z)  ;
	}
	public bool showGizmo=true;
	void OnDrawGizmos(){
		if(showGizmo){

			
			Gizmos.color=Color.yellow;
			Gizmos.DrawLine(camInit.position , right + forward + camMinY.position);
			Gizmos.DrawLine(camInit.position , right - forward + camMinY.position);
			Gizmos.DrawLine(camInit.position , -right + forward+ camMinY.position);
			Gizmos.DrawLine(camInit.position , -right - forward + camMinY.position);
			Gizmos.DrawLine(right + forward + camMinY.position , right- forward + camMinY.position);
			Gizmos.DrawLine(right - forward + camMinY.position , -right - forward + camMinY.position);
			Gizmos.DrawLine(-right - forward + camMinY.position ,-right + forward + camMinY.position);
			Gizmos.DrawLine(-right + forward + camMinY.position , right + forward + camMinY.position);
		}
	}



}
