using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLogic : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0; //Pauses game 
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1; //Runs game again
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart current scene
        Time.timeScale = 1;
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }




}
