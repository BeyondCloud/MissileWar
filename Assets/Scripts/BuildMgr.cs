using UnityEngine;
using System.Collections;

public class BuildMgr : MonoBehaviour {

	public UI ui;
	public CameraController camCtrl;
	public GameObject drill;
	public GameObject cannon;

	public GameObject[] redBar;
	public enum _buildMode {dragNdrop,selectDrop};

	public _buildMode buildMode = _buildMode.dragNdrop;

	private bool isDrag = false;
	private bool isSelect = false;
	private bool isBuild = false;

	private GameObject selectTower;

	private GameObject towerBasis;
	private Vector3 selectPos;
	private Vector3 towerAppearPoint;

	Ray ray ;
	RaycastHit hit;
	Vector2 prevSelect;
	bool buildableFlag;
	string towerTag;

	void Start()
	{
		towerAppearPoint = new Vector3(8,0,-15);
		prevSelect = Vector2.zero;
		selectPos.y = 0.02f;
	}
public void buildSomething(UI._building building)
	{
		 
		switch(building)
		{
		    case UI._building.drill:
			   selectTower = drill;
		       towerTag = "basicDrill";
			break;
		    case UI._building.cannon:
			   selectTower = cannon;
			   towerTag = "basicCannon";
			break;
		}

		camCtrl.selectPanel.SetActive(false);
		if(buildMode == _buildMode.dragNdrop)
		{
			camCtrl.selectHighLight.SetActive(true);
			isDrag = true;
			
			selectTower = (GameObject)Instantiate(selectTower,Vector3.zero,Quaternion.Euler(90,90,0));
			

		}
		else if(buildMode == _buildMode.selectDrop)
		{
			UI.fixMoveCam = true;
			isSelect= true;
			
			selectTower = (GameObject)Instantiate(selectTower,towerAppearPoint,Quaternion.Euler(90,90,0));


		}


		//GameObject drill = Instantiate(drill,buildPos,Quaternion.identity);
	}

void Update()
	{

		if(isDrag)
		{

			StartCoroutine("OnDrag");
		}
		if(isSelect)
		{

			StartCoroutine("OnSelect");
		}
	}
IEnumerator OnDrag()
	{
		if(Input.touchCount == 1)
		{
			ray  = Camera.main.ScreenPointToRay((Input.GetTouch(0).position));

			if (Physics.Raycast(ray, out hit, 50.0f))
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
					     
					     

					    if(prevSelect.x != selectPos.x || prevSelect.y != selectPos.z)
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
				settleBuild();
				yield return null;
			}
		}

	}
	IEnumerator OnSelect()
	{

		if(Input.touchCount == 1 && !isBuild )
		{

				camCtrl.selectHighLight.SetActive(true);
				ray  = Camera.main.ScreenPointToRay((Input.GetTouch(0).position));
				
				if (Physics.Raycast(ray, out hit, 50.0f))
				{

					if(hit.point.y < 5.0f)
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
							
							
							
							if(prevSelect.x != selectPos.x || prevSelect.y != selectPos.z)
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

					
				}

			
			if(!isSelect)
			 yield return null;
		}
		
	}
	//func will be call after press build(if use selectDrop)
	public void settleBuild()
	{
		isBuild = true;
		if(buildMode == _buildMode.dragNdrop)
		{
			isDrag = false;
			UI.fixMoveCam = false;
			camCtrl.selectHighLight.SetActive(false);
		}
		if(buildableFlag)
		{ 
			networkView.RPC("build",RPCMode.All,selectPos);
		}
		else if(buildMode == _buildMode.dragNdrop)
			Destroy(selectTower);
		twoByTwoRedZoneDetect(selectPos);

	}
	[RPC]
	void build(Vector3 pos)
	{
		print ("settle");
		//	if(buildMode == _buildMode.selectDrop)
		//		Instantiate(selectTower,selectPos,Quaternion.Euler(0,0,0));
		
		towerBasis =  (GameObject)Instantiate(selectTower,pos,Quaternion.Euler(90,0,0));
		towerBasis.tag = towerTag;
		towerBasis.collider.enabled = true;

	}
	public void btnExit()
	{
		isBuild = false;
	}
	public void cancelBuild()
	{
		UI.fixMoveCam = false;
		ui.buildingBtnSet.SetActive(true);
		ui.buildBtn.SetActive(false);
		ui.cancelBtn.SetActive(false);
		camCtrl.selectHighLight.SetActive(false);
		isSelect = false;
		Destroy(selectTower);
	}

	void assignCursorValue(Vector3 selectPos)
	{
		selectTower.transform.position = selectPos;
		camCtrl.selectHighLight.transform.position = selectPos;
	}

	void twoByTwoRedZoneDetect(Vector3 buildPoint)
	{
		print ("Red_Zone_detect");
		buildableFlag = true;
		float halfPanelWidth = GridMgr.panelWidth/2;

		Vector3[] castOrig = {
			new Vector3( buildPoint.x + halfPanelWidth , 3 , buildPoint.z + halfPanelWidth),
			new Vector3( buildPoint.x - halfPanelWidth , 3 , buildPoint.z + halfPanelWidth),
			new Vector3( buildPoint.x - halfPanelWidth , 3 , buildPoint.z - halfPanelWidth),
			new Vector3( buildPoint.x + halfPanelWidth , 3 , buildPoint.z - halfPanelWidth)};

		ray.direction = Vector3.down;


		//reset color to gray
		for(int i = 0;i < 4; i++)
			redBar[i].renderer.material.SetColor("_TintColor", Color.gray);
		for(int i = 0;i < 4; i++)
		{
			ray.origin = castOrig[i];
		    
			if (Physics.Raycast(ray, out hit, 10.0f))
			{
				Debug.DrawRay(castOrig[i],Vector3.down);
				if(hit.collider.tag != "buildable")
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
