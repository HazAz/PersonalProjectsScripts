using UnityEngine;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public void ReturnToTitle()
    {
		  SceneManager.LoadScene("MainMenuScene");
    }

    public void Replay()
    {
    	SceneManager.LoadScene("LevelOne");
    }
}
