using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceRaceScript : MonoBehaviour
{
    private int maxBulletsCreated;
    public int numBulletsCreated;

    private GameObject Player1;
	private GameObject Player2;
    private int player1Score;
	private int player2Score;

	public Text Player1Score;
	public Text Player2Score;
    public Text StartText;
    public Text VictoryText;

    public GameObject ballPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");

        player1Score = 0;
        player2Score = 0;

        Player1Score.text = "" + player1Score;
        Player2Score.text = "" + player2Score;

        maxBulletsCreated = 20;
        numBulletsCreated = 0;
        StartCoroutine("WaitThreeSeconds");
    }

    void Create()
    {
        if (numBulletsCreated < maxBulletsCreated)
        {
            var initialXPosition = Random.Range(0.0f, 2.0f) >= 1.0f ? 7.5f: -7.5f;
            Instantiate(ballPrefab, new Vector3(initialXPosition, Random.Range(-3.5f, 4.75f), -1.0f), Quaternion.identity);
            numBulletsCreated++;
        }
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
        Create();
    }

    IEnumerator WaitThreeSeconds()
    {
        Create();
    	SwitchFunctionsOff();
        VictoryText.enabled = false;

    	yield return new WaitForSeconds(1);
        StartText.text = "2";

        yield return new WaitForSeconds(1);
        StartText.text = "1";

        yield return new WaitForSeconds(1);
        StartText.enabled = false;
        
        SwitchFunctionsOn();
    }

    private void SwitchFunctionsOff()
    {
        Player1.GetComponent<Player1SpaceRace>().enabled = false;
    	Player2.GetComponent<Player2SpaceRace>().enabled = false;
        Player2.GetComponent<AISpaceRaceScript>().enabled = false;
    }

    private void SwitchFunctionsOn()
    {
        Player1.GetComponent<Player1SpaceRace>().enabled = true;
        if (GlobalScript.GetPlayerInfo() == GlobalScript.PlayerInfo.OnePlayer)
        {
            Player2.GetComponent<AISpaceRaceScript>().enabled = true;
        }
        else
        {
            Player2.GetComponent<Player2SpaceRace>().enabled = true;
        }
    }

    public void Player1Scored()
    {
    	player1Score++;
    	Player1Score.text = "" + player1Score;
        if (player1Score >= 5)
        {
            Victory(true);
        }
    }

    public void Player2Scored()
    {
    	player2Score++;
    	Player2Score.text = "" + player2Score;
        if (player2Score >= 5)
        {
            Victory(false);
        }
    }

    private void Victory(bool player1Wins)
    {
        VictoryText.text = player1Wins ? "Victory! Player 1 wins!" : "Victory! Player 2 wins!";
        VictoryText.enabled = true;
        SwitchFunctionsOff();
    }
}
