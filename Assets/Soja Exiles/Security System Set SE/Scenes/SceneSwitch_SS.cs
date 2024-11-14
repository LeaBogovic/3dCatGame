using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch_SS : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
			SceneManager.LoadScene("Demo 01");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
			SceneManager.LoadScene("Demo 02");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
			SceneManager.LoadScene("Demo 03");
        }

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			SceneManager.LoadScene("Cameras Screens and Textures Demo");
		}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
