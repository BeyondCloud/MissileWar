using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {
	public BuildMgr buildMgr;
	public Button mineBtn;
	public static bool fixMoveCam = false;


    void Update()
	{

	}

    public void buildMine()
	{
		GridMgr.cursorSize = 2;
		print ("build");
		buildMgr.buildMine();
	}
	public void fixCam()
	{
		fixMoveCam = true;
	}

}
