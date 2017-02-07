using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;

    void Start() {
        PauseMenu.IsOn = false;
    }

	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePauseMenu();
        }

	}

    void TogglePauseMenu() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
