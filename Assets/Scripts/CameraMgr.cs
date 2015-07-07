using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour {

	public GameObject cam1;
	public GameObject cam2;

	public void chooseCam()
	{

        if(Network.isServer)
		{
			cam1.SetActive(true);
			cam2.SetActive(false);
		}
		else if(Network.isClient)
		{
			print ("asd");
			cam1.SetActive(false);
			cam2.SetActive(true);
		}
	}
	

}
