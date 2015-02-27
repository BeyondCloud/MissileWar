using UnityEngine;
using System.Collections;

public class BuildMgr : MonoBehaviour {


	public CameraController camCtrl;
	public GameObject basis;
	public GameObject drill;
	private bool startDrag = false;
	private GameObject selectTower;
	private GameObject towerBasis;
	private Vector3 selectPos;
	void Start()
	{
		selectPos.y = 0.01f;
	}
public void buildMine()
	{


			startDrag = true;

			selectTower = (GameObject)Instantiate(drill,Vector3.zero,Quaternion.Euler(0,90,0));
		   towerBasis =  (GameObject)Instantiate(basis,Vector3.zero,Quaternion.Euler(90,0,0));

	




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

			Ray ray = Camera.main.ScreenPointToRay((Input.GetTouch(0).position));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 35.5f))
			{
				camCtrl.selectHighLight.transform.localScale = new Vector3(GridMgr.cursorSize,GridMgr.cursorSize,1);
				
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
					assignCursorValue(selectPos);
				}
				else if(GridMgr.cursorSize == 3)
				{
					selectPos =  hit.transform.position;
					assignCursorValue(selectPos);
				}
				
				else
					print("cursor size undefined");
				
				
				
			}
			
			
			
			if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				startDrag = false;
				UI.fixMoveCam = false;
				camCtrl.selectHighLight.SetActive(false);
				yield return null;
			}
		}

	 }
	void assignCursorValue(Vector3 selectPos)
	{
		selectTower.transform.position = selectPos;
		towerBasis.transform.position = selectPos;
		camCtrl.selectHighLight.transform.position = selectPos;
	}
	
	
	
}
