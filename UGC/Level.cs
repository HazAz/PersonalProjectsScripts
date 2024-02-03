using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    // From: https://answers.unity.com/questions/1220414/rotating-object-by-x-degree-once-every-y-seconds.html
    public float rotationDelayInSeconds = 3.0f;
    public float rotationDuration = 1.5f;
    public GameObject timer;
    private bool counterClockwiseRot = false;
    private Rigidbody2D playerRigidBody;
    private float gameTime = 0.0f;
    public bool isInEditMode;
    public bool inCoroutine;
    public static int difficulty = 0;

    void Start()
    {
        inCoroutine = false;
        GameObject ugc = GameObject.Find("UGCMenuScript");
        isInEditMode = (ugc) ? true : false;
        playerRigidBody = GameObject.Find("2D_Player").GetComponent<Rigidbody2D>();
        if (!isInEditMode)  StartLevel();
    }
    
    void Update()
    {
        if (isInEditMode) return;
        if (!isInEditMode && !inCoroutine) StartLevel();
        gameTime += Time.deltaTime;
        TimeSpan seconds = TimeSpan.FromSeconds(gameTime % 60);
        timer.GetComponent<Text>().text = seconds.ToString(@"mm\:ss\:fff");
    }

    public void Victory()
    {
        // Check for victory.  The distance is hardcoded, but that'll do for now.
        var time = GameObject.Find("Timer").GetComponent<Text>().text;
        var name = PlayerPrefs.GetString("PlayerName");
        HighScoreTable.AddHighscoreEntry(time, name);
        SceneManager.LoadScene("Leaderboard");
    }

    IEnumerator RotateLevel(Vector3 axis, float inTime)
    {
        float angle = 90;

        // Initial Delay
        yield return new WaitForSeconds(rotationDelayInSeconds);

        float speedScale = 1.0f;
        float waitScale = 1.0f;
        switch ( difficulty )
        {
            case 0:
                speedScale = 0.5f;
                waitScale = 1.0f;
                break;
            case 1:
                speedScale = 1.0f;
                waitScale = 1.0f;
                break;
            case 2:
                speedScale = 2.0f;
                waitScale = 0.25f;
                break;
        }

        // Calcuate rotation speed
        float rotationSpeed = speedScale * angle / (inTime * 2.0f);

        while (true)
        {
            // Determine which direction it's rotating in
            counterClockwiseRot = (UnityEngine.Random.value > 0.5f);

            // Save starting rotation
            Quaternion startRotation = transform.rotation;
            float deltaAngle = 0;
            float direction = counterClockwiseRot ? 1f : -1f;

            // Rotate until reaching angle
            if (counterClockwiseRot)
            {
                while (deltaAngle < angle)
                {

                    deltaAngle += rotationSpeed * Time.deltaTime;
                    deltaAngle = Mathf.Min(deltaAngle, angle);

                    transform.rotation = startRotation * Quaternion.AngleAxis(deltaAngle, axis);

                    // Toggle the player rigid body gravity on and off to force it to reapply
                    //playerRigidBody.gravityScale = 0;
                    //playerRigidBody.gravityScale = 1;

                    yield return null;
                }
            }
            else
            {
                while (deltaAngle > -angle)
                {

                    deltaAngle -= rotationSpeed * Time.deltaTime;
                    deltaAngle = Mathf.Min(deltaAngle, angle);

                    transform.rotation = startRotation * Quaternion.AngleAxis(deltaAngle, axis);

                    // Toggle the player rigid body gravity on and off to force it to reapply
                    //playerRigidBody.gravityScale = 0;
                    //playerRigidBody.gravityScale = 1;

                    yield return null;
                }
            }            
            
            // Delay
            float waitTime = 0.5f + rotationDelayInSeconds * UnityEngine.Random.value;
            waitTime *= waitScale;
            yield return new WaitForSeconds( waitTime );

            angle = 10.0f + UnityEngine.Random.value * 80.0f;
            // Debug.Log("InRotate2");
        }
    }

    public void StartLevel()
    {
        isInEditMode = false;
        inCoroutine = true;
        StartCoroutine(RotateLevel(Vector3.forward, rotationDuration));
    }
}


