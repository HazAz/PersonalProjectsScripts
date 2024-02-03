using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public static class BoardRules
{
    public static int NumberTeams = 2;
    public static int BoardX = 10;
    public static int BoardY = 10;
    public static GameObject BoardPlane = GameObject.FindGameObjectWithTag("BoardPlane");
    public static bool isRedTurn = false;

    public static void AdjustBoardPlane()
    {
        BoardPlane.transform.position = new Vector3(BoardX / 2, 0, BoardY / 2);
        BoardPlane.transform.localScale = new Vector3(BoardX / 10, 1, BoardY / 10);
    }
}
