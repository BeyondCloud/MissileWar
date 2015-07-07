using UnityEngine;
using System.Collections;
using System.Collections.Generic;



enum _panelState {buildable,unbuildable};

public class GridMgr : MonoBehaviour {


	public GameObject touchPanel;


	public static int gridWidth = 28;
	public static int gridLength = 16;
	public static float panelWidth = 1;

	public static int cursorSize = 2;
	public TwoDimString[] point = new  TwoDimString[10];

	Vector3 addr;


	void Start () 
	{

		float halfgridWidth = gridWidth / 2;
		float halfPanelWidth  = panelWidth/2;


		if( gridWidth % 2 == 1)
			print ("grid width should be odd to get right position");



		for(int i = 0; i < gridLength ;i++)
		{
	   		 for(int j = 0; j < gridWidth ;j++)
			{

				addr = new Vector3((-halfgridWidth * panelWidth + halfPanelWidth ) + j * panelWidth, 0.01f , - halfPanelWidth - panelWidth * i ) ;


				Instantiate(touchPanel,addr, Quaternion.Euler(90,0,0));
			    
			}

		}

	
	}
	

}
