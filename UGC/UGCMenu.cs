using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UGCMenu : MonoBehaviour
{
    private GameObject level;
    private GameObject levelChild;
    // Start is called before the first frame update
    void Start()
    {
        level = GameObject.Find("Level");
        levelChild = level.transform.GetChild(0).gameObject;
        GlobalInfo.ResetArrays();
        //levelChild.GetComponent<Level>().isInEditMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        //levelChild.GetComponent<Level>().isInEditMode = true;
    }

    public void EditBarrier()
    {
    	this.GetComponent<WebSelection>().EndWebSelection();
    	this.GetComponent<BarrierSelection>().StartBarrierSelection();
    }

    public void EditWeb()
    {
    	this.GetComponent<BarrierSelection>().EndBarrierSelection();
    	this.GetComponent<WebSelection>().StartWebSelection();
    }

    public void MainMenu()
    {
    	SceneManager.LoadScene("MainMenuScene");
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("PlayEditedScene");
    	//StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();
        levelChild.GetComponent<Level>().StartLevel();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PlayEditedScene", LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //levelChild.GetComponent<Level>().StartLevel();
        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(level, SceneManager.GetSceneByName("PlayEditedScene"));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
