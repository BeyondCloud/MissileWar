using UnityEngine;
using System.Collections;

public class BuildMgr : MonoBehaviour {


	public CameraController camCtrl;
	public GameObject basis;
	public GameObject drill;
	public GameObject[] redBar;


	private bool startDrag = false;
	private GameObject selectTower;
	private GameObject towerBasis;
	private Vector3 selectPos;
	Ray ray ;
	RaycastHit hit;
	Vector2 prevSelect;
	bool buildableFlag;
	void Start()
	{
		prevSelect = Vector2.zero;
		selectPos.y = 0.01f;
	}
public void buildMine()
	{


			startDrag = true;

			selectTower = (GameObject)Instantiate(drill,Vector3.zero,Quaternion.Euler(0,90,0));
		   

	




		//GameObject drill = Instantiate(drill,buildPos,Quaternion.identity);
	}

void Update()
	{
		if(startDrag)
		{

		
			StartCoroutine("OnDrag");
		}
	}
IEnumerator OnDrag()
	{
		if(Input.touchCount == 1)
		{
			camCtrl.selectHighLight.SetActive(true);
			camCtrl.selectPanel.SetActive(false);

			ray  = Camera.main.ScreenPointToRay((Input.GetTouch(0).position));

			if (Physics.Raycast(ray, out hit, 35.5f))
			{
				camCtrl.selectHighLight.transform.localScale = new Vector3(GridMgr.cursorSize,GridMgr.cursorSize,1);

				switch(GridMgr.cursorSize)
				{
				    case 2:
						if(Mathf.Sign( hit.point.x ) == 1)
							selectPos.x = (int)(hit.point.x + 0.5f);
						else
							selectPos.x = (int)(hit.point.x - 0.5f);
						if(Mathf.Sign( hit.point.y ) == 1)
							selectPos.z = (int)(hit.point.z - 0.5f);
						else
							selectPos.z = (int)(hit.point.z + 0.5f);
					     
					     

					//    if(prevSelect.x != selectPos.x || prevSelect.y != selectPos.z)
					{
						prevSelect.x = selectPos.x;
						prevSelect.y = selectPos.z;
						assignCursorValue(selectPos);
						twoByTwoRedZoneDetect(selectPos);

					}
					break;
	     			case 3:
						selectPos =  hit.transform.position;
						assignCursorValue(selectPos);
					break;
				    default:
					    print("cursor size undefined");
					break;

				}
				
			}
			
			
			
			if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				startDrag = false;
				UI.fixMoveCam = false;
				camCtrl.selectHighLight.SetActive(false);
				if(buildableFlag)
				{
					towerBasis =  (GameObject)Instantiate(basis,selectPos,Quaternion.Euler(90,0,0));
					towerBasis.tag = "unbuildable";
				}
				else
					Destroy(selectTower);

				yield return null;
			}
		}

	 }
	void assignCursorValue(Vector3 selectPos)
	{
		selectTower.transform.position = selectPos;
		camCtrl.selectHighLight.transform.position = selectPos;
	}

	void twoByTwoRedZoneDetect(Vector3 buildPoint)
	{
		buildableFlag = true;
		float halfPanelWidth = GridMgr.panelWidth/2;

		Vector3[] castOrig = {
			new Vector3( buildPoint.x + halfPanelWidth , 1 , buildPoint.z + halfPanelWidth),
			new Vector3( buildPoint.x - halfPanelWidth , 1 , buildPoint.z + halfPanelWidth),
			new Vector3( buildPoint.x - halfPanelWidth , 1 , buildPoint.z - halfPanelWidth),
			new Vector3( buildPoint.x + halfPanelWidth , 1 , buildPoint.z - halfPanelWidth)};

		ray.direction = Vector3.down;

		//reset color to gray
		for(int i = 0;i < 4; i++)
			redBar[i].renderer.material.SetColor("_TintColor", Color.gray);
		for(int i = 0;i < 4; i++)
		{
			ray.origin = castOrig[i];
		    
			if (Physics.Raycast(ray, out hit, 35.5f))
			{
				Debug.DrawRay(ray.origin,hit.point);
				if(hit.collider.tag == "unbuildable")
				{
					buildableFlag = false;
					redBar[i].renderer.material.SetColor("_TintColor", Color.red);
					if(i == 3)
					    redBar[0].renderer.material.SetColor("_TintColor", Color.red);
					else
						redBar[i+1].renderer.material.SetColor("_TintColor", Color.red);

				}

			}


				   
					
		}



	}

	
}
