using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pongScript : MonoBehaviour
{
	private GameObject Player1;
	private GameObject Player2;
	private GameObject PongBall;

	private int player1Score;
	private int player2Score;

	public Text Player1Score;
	public Text Player2Score;
    public Text StartText;
    public Text VictoryText;
    // Start is called before the first frame update
    void Start()
    {
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        PongBall = GameObject.Find("PongBall");

        player1Score = 0;
        player2Score = 0;

        Player1Score.text = "" + player1Score;
        Player2Score.text = "" + player2Score;

        StartCoroutine("WaitThreeSeconds");
    }

    public void Player1Scored()
    {
    	player1Score++;
    	Player1Score.text = "" + player1Score;
        if (player1Score >= 10)
        {
            Victory(true);
        }
    }

    public void Player2Scored()
    {
    	player2Score++;
    	Player2Score.text = "" + player2Score;
        if (player2Score >= 10)
        {
            Victory(false);
        }
    }

    IEnumerator WaitThreeSeconds()
    {
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

    private void Victory(bool player1Wins)
    {
        VictoryText.text = player1Wins ? "Victory! Player 1 wins!" : "Victory! Player 2 wins!";
        VictoryText.enabled = true;
        SwitchFunctionsOff();
    }

    private void SwitchFunctionsOff()
    {
        Player1.GetComponent<Player1>().enabled = false;
    	Player2.GetComponent<Player2>().enabled = false;
        Player2.GetComponent<AIScript>().enabled = false;
    	PongBall.GetComponent<Ball>().enabled = false;
    }

    private void SwitchFunctionsOn()
    {
        Player1.GetComponent<Player1>().enabled = true;
        if (GlobalScript.GetPlayerInfo() == GlobalScript.PlayerInfo.OnePlayer)
        {
            Player2.GetComponent<AIScript>().enabled = true;
        }
        else
        {
            Player2.GetComponent<Player2>().enabled = true;
        }
    	
    	PongBall.GetComponent<Ball>().enabled = true;
    }
}
