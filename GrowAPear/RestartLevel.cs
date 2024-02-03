using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void onRestartLevelButton()
    {
        Debug.LogError(SceneManager.GetActiveScene().name);
        StaticPowerupScript.OnDeath();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
