using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public void StartGame() {
        SceneManager.LoadScene("GameScene"); 
        Debug.Log("start game");
    }

    public void StartTutorial() {
        Debug.Log("start tutorial");
        //SceneManager.LoadScene("TutorialScene");  
    }

    // public void ExitGame() {
    //     Application.Quit();
    // }
}