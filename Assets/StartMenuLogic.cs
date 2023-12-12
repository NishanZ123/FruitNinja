using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuLogic : MonoBehaviour
{
    public void StartGame() 
    {
        SceneManager.LoadScene("NinjaGame");
    }

    public void OpenStore()
    {
        SceneManager.LoadScene("Store");
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
        public void OpenModeSelection()
    {
        SceneManager.LoadScene("ModeSelection");
    }

    public void OpenClassicMode()
    {
        SceneManager.LoadScene("ClassicMode");
    }   
        public void OpenQuickshotMode()
    {
        SceneManager.LoadScene("QuickshotMode");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
