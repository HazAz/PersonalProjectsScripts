using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{
    public static BoardHighlights Instance { set; get; }

    public GameObject highlightPrefab;
    public GameObject highlightEnemy;
    private List<GameObject> highlights;
    private List<GameObject> enemyHighlights;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
        enemyHighlights = new List<GameObject>();
    }

    private GameObject GetHightlightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);
        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }
        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i < BoardRules.BoardX; i++)
        {
            for (int j = 0; j < BoardRules.BoardY; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetHightlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                }
            }
        }
    }

    public void HideHighlights()
    {
        foreach(GameObject go in highlights)
        {
            go.SetActive(false);
        }
        
        foreach(GameObject go in enemyHighlights)
        {
            go.SetActive(false);
        }
        
    }

    public void HighlightEnemies(List<int> listX, List<int> listY)
    {
        for (int i = 0; i < listX.Count; i++)
        {
            int xPos = listX[i];
            int yPos = listY[i];
            GameObject go = enemyHighlights.Find(g => !g.activeSelf);
            if (go == null)
            {
                go = Instantiate(highlightEnemy);
                enemyHighlights.Add(go);
            }
            go.SetActive(true);
            go.transform.position = new Vector3(xPos + 0.5f, 0, yPos + 0.5f);
        }
    }
}
