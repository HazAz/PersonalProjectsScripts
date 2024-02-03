using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalInfo
{
    public static float GameSpeed = -8f;
    public static float VerticalSpeed = 4f;

    public static int NumIterations = 0;

    public static bool IsGameOver = false;
    
    public static void MakeGameFaster()
    {
        GameSpeed *= 1.2f;
        VerticalSpeed *= 1.2f;
    }

    public static void ResetValues()
    {
        GameSpeed = -8f;
        VerticalSpeed = 4f;
        NumIterations = 0;
        IsGameOver = false;
    }
}
