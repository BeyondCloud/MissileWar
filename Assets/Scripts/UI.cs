using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {
	public BuildMgr buildMgr;

	public GameObject buildingBtnSet;

	public GameObject buildBtn;

	public GameObject cancelBtn;

	public static bool fixMoveCam = false;
	public enum _building {drill,cannon};



    void Start()
	{
		buildBtn.SetActive(false);
		cancelBtn.SetActive(false);
	}
	public void buildMine()
	{

		hideBtnSet();
		GridMgr.cursorSize = 2;
		print ("buildMine");
		buildMgr.buildSomething(_building.drill);


	}
	public void buildCannon()
	{

		hideBtnSet();
		GridMgr.cursorSize = 2;
		print ("buildCannon");
		buildMgr.buildSomething(_building.cannon);
		
		
	}
	void hideBtnSet()
	{
		if(buildMgr.buildMode == BuildMgr._buildMode.selectDrop)
		{
			buildingBtnSet.SetActive(false);
			buildBtn.SetActive(true);
			cancelBtn.SetActive(true);
			
		}
	}
	public void fixCam()
	{

		fixMoveCam = true;
	}

}
