using UnityEngine;
using System.Collections;

public class GridMgr : MonoBehaviour {


	public GameObject panel;
	public int gridWidth = 14;
	public int gridLength = 10;
	private float panelWidth = 1;
	public int [,] gridAddr;
	void Start () 
	{
		gridAddr = new int[gridWidth,gridLength];
		float halfgridWidth = gridWidth / 2;
		float halfPanelWidth  = panelWidth/2;

		if( gridWidth % 2 == 1)
			print ("grid width should be odd to get right position");

		for(int i = 0; i < gridLength ;i++)
		{
	   		 for(int j = 0; j < gridWidth ;j++)
			{
				Instantiate(panel,new Vector3((-halfgridWidth * panelWidth + halfPanelWidth ) + j * panelWidth, 0.01f , - halfPanelWidth - panelWidth * i ) , Quaternion.Euler(90,0,0));

			}

		}
	
	}
	

}
