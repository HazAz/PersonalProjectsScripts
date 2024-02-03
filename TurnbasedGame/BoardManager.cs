using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }

    private bool[,] allowedMoves { set; get; }

    public Units[,] Units { set; get; }
    private Units SelectedUnit;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    private int intWait = 1;
    private int intAttack = 2;
    private int intBack = 3;
    private int selectedAction = -1;
    private bool inAttack = false;

    private int numMoves = 0;

    public List<GameObject> unitPrefabs;
    private List<GameObject> activeUnits;

    private List<int> enemyNearbyX;
    private List<int> enemyNearbyY;

    private List<GameObject> blueUnits;
    private List<GameObject> redUnits;

    public GameObject ActionsMenu;
    public GameObject AttackOption;

    public Text turnText;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    void Start()
    {
        Instance = this;
        BoardRules.AdjustBoardPlane();
        SpawnAllUnits();
        ActionsMenu.SetActive(false);
        AttackOption.SetActive(false);
        turnText.text =  (BoardRules.isRedTurn) ? "Red turn" : "Blue turn";
    }

    void Update()
    {
        UpdateSelection();
        DrawBoard();

        if(Input.GetMouseButtonDown(0))
        {
            if(selectionX >= 0 && selectionY >= 0)
            {
                if (SelectedUnit == null)
                {
                    //select unit
                    SelectUnit(selectionX, selectionY);
                }
                else if (inAttack)
                {
                    //RIGHT HERE CLEM
                    AttackUnit(selectionX, selectionY);
                }
                else
                {
                    //moveUnit
                    MoveUnit(selectionX, selectionY);
                }
            }
        }
    }

    //Draw the board and the selection
    private void DrawBoard()
    {
        Vector3 widthLine = Vector3.right * BoardRules.BoardX;
        Vector3 heightLine = Vector3.forward * BoardRules.BoardY;

        //draw the board
        for (int i = 0; i <= BoardRules.BoardX; i++)
        {
            Vector3 start = Vector3.forward * i;
            UnityEngine.Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= BoardRules.BoardY; j++)
            {
                start = Vector3.right * j;
                UnityEngine.Debug.DrawLine(start, start + heightLine);
            }
        }

        //Draw the selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            UnityEngine.Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            UnityEngine.Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }

    public void EndTurn()
    {
        numMoves = 0;
        BoardRules.isRedTurn = !BoardRules.isRedTurn;
        for (int i = 0; i < BoardRules.BoardX; ++i)
        {
            for (int j = 0; j < BoardRules.BoardY; ++j)
            { 
                if (Units[i, j] != null)
                {
                    Units[i, j].canMove = BoardRules.isRedTurn == Units[i, j].isRed;
                    ColorReseter.Instance.ResetColor(Units[i, j]);
                }
            }
        }
        turnText.text = (BoardRules.isRedTurn) ? "Red turn" : "Blue turn";
    }


    //Find selected tile
    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 
                            out hit, 25.0f, LayerMask.GetMask("BoardPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    

    //gets center of selected tile
    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.y += 0.5f;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    //spawns a specific unit at a specific tile
    private void SpawnUnit(int index, int x, int y)
    {
        GameObject go = Instantiate(unitPrefabs[index], GetTileCenter(x, y), unitPrefabs[index].GetComponent<Units>().isRed ? orientation : Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        Units[x, y] = go.GetComponent<Units>();
        Units[x, y].SetPosition(x, y);
        activeUnits.Add(go);
        if (index < 4)
        {
            blueUnits.Add(go);
        }
        else
        {
            redUnits.Add(go);
        }
        //Units[x, y].StartHealth();
    }
    
    //spawns all units at start of game
    private void SpawnAllUnits()
    {
        activeUnits = new List<GameObject>();
        blueUnits = new List<GameObject>();
        redUnits = new List<GameObject>();
        Units = new Units[BoardRules.BoardX, BoardRules.BoardY];

        //spawn blue team
        SpawnUnit(0, 0, 0);
        SpawnUnit(1, 1, 0);
        SpawnUnit(2, 0, 1);
        SpawnUnit(3, 1, 1);

        //spawn red team
        SpawnUnit(4, BoardRules.BoardX - 1, BoardRules.BoardY - 1);
        SpawnUnit(5, BoardRules.BoardX - 2, BoardRules.BoardY - 1);
        SpawnUnit(6, BoardRules.BoardX - 1, BoardRules.BoardY - 2);
        SpawnUnit(7, BoardRules.BoardX - 2, BoardRules.BoardY - 2);
    }

    //select unit by clicking on it and display allowed moves
    private void SelectUnit(int x, int y)
    {
        if (Units[x,y] == null)
        {
            return;
        }
        if (Units[x,y].isRed != BoardRules.isRedTurn || !Units[x, y].canMove)
        {
            return;
        }
        //display allowed moves
        allowedMoves = Units[x, y].PossibleMove();
        SelectedUnit = Units[x, y];
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    //move selected unit
    private void MoveUnit(int x, int y)
    {
        if (allowedMoves[x,y] && SelectedUnit.canMove)
        {
            int prevX = SelectedUnit.CurrentX;
            int prevY = SelectedUnit.CurrentY;
            Units[SelectedUnit.CurrentX, SelectedUnit.CurrentY] = null;
            SelectedUnit.transform.position = GetTileCenter(x, y);
            SelectedUnit.SetPosition(x, y);
            DecideAction(prevX, prevY, x, y);
            
        }
    }

    //show action menu
    private void DecideAction(int prevX, int prevY, int newX, int newY)
    {
        ActionsMenu.SetActive(true);
        SelectedUnit.canMove = false;
        enemyNearbyX = new List<int>();
        enemyNearbyY = new List<int>();
        CheckEnemy();
        if (enemyNearbyX.Count > 0 && enemyNearbyX.Count == enemyNearbyY.Count)
        {
            AttackOption.SetActive(true);
        }
        StartCoroutine(WaitSelectedAction(prevX, prevY, newX, newY)); 
    }


    //When moved, wait till action is chosen
    IEnumerator WaitSelectedAction(int prevX, int prevY, int newX, int newY)
    {
        while (selectedAction < 0)
        {
            yield return null;
        }
 
        if (selectedAction == intWait || selectedAction == intAttack)
        {
            Units[prevX, prevY] = null;
            Units[newX, newY] = SelectedUnit;
            if (selectedAction == intAttack)
            {
                inAttack = true;
                BoardHighlights.Instance.HighlightEnemies(enemyNearbyX, enemyNearbyY);
                StartCoroutine(WaitSelectedAttack());
            }
            else
            {
                BoardHighlights.Instance.HideHighlights();
                ColorReseter.Instance.SetGrey(SelectedUnit);
            }

            numMoves++;
            if ((SelectedUnit.isRed && numMoves >= redUnits.Count) || (!SelectedUnit.isRed && numMoves >= blueUnits.Count))
            {
                EndTurn();
            }
            if (selectedAction == intWait)
            {
                SelectedUnit = null;
            }
        }

        else 
        {
            SelectedUnit.transform.position = GetTileCenter(prevX, prevY);
            SelectedUnit.SetPosition(prevX, prevY);
            SelectedUnit.canMove = true;
            Units[newX, newY] = null;
            Units[prevX, prevY] = SelectedUnit;
            BoardHighlights.Instance.HideHighlights();
            SelectedUnit = null;
        }

        AttackOption.SetActive(false);
        ActionsMenu.SetActive(false);
        selectedAction = -1;
    }

    IEnumerator WaitSelectedAttack()
    {
        while (inAttack)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2);
        ColorReseter.Instance.SetGrey(SelectedUnit);
        BoardHighlights.Instance.HideHighlights();
        SelectedUnit = null;
    }


    //EDIT
    //RETURN LIST OF UNITS, AND WRITE FUNCTION THAT FINDS THE POSITIONS
    //check if enemies are nearby
    private void CheckEnemy()
    {
        Units go = null;
        if (SelectedUnit.CurrentX < BoardRules.BoardX - 1)
        {
            go = Units[SelectedUnit.CurrentX + 1, SelectedUnit.CurrentY];
            if (go != null && go.isRed != SelectedUnit.isRed)
            {
                enemyNearbyX.Add(SelectedUnit.CurrentX + 1);
                enemyNearbyY.Add(SelectedUnit.CurrentY);
            }
        }

        if (SelectedUnit.CurrentX > 0)
        {
            go = Units[SelectedUnit.CurrentX - 1, SelectedUnit.CurrentY];
            if (go != null && go.isRed != SelectedUnit.isRed)
            {
                enemyNearbyX.Add(SelectedUnit.CurrentX - 1);
                enemyNearbyY.Add(SelectedUnit.CurrentY);
            }
        }

        if (SelectedUnit.CurrentY < BoardRules.BoardY - 1)
        {
            go = Units[SelectedUnit.CurrentX, SelectedUnit.CurrentY + 1];
            if (go != null && go.isRed != SelectedUnit.isRed)
            {
                enemyNearbyX.Add(SelectedUnit.CurrentX);
                enemyNearbyY.Add(SelectedUnit.CurrentY + 1);
            }
        }

        if (SelectedUnit.CurrentY > 0)
        {
            go = Units[SelectedUnit.CurrentX, SelectedUnit.CurrentY - 1];
            if (go != null && go.isRed != SelectedUnit.isRed)
            {
                enemyNearbyX.Add(SelectedUnit.CurrentX);
                enemyNearbyY.Add(SelectedUnit.CurrentY - 1);
            }
        }
    }

    //Selected specific actions
    public void SelectWait()
    {
        selectedAction = intWait;
    }

    public void SelectAttack()
    {
        selectedAction = intAttack;
    }

    public void SelectBack()
    {
        selectedAction = intBack;
    }

    public void AttackUnit(int selectedX, int selectedY)
    {
        //check if enemy is chosen and if in highlighted enemy
        for (int i = 0; i < enemyNearbyX.Count; i++)
        {
            if (selectionX == enemyNearbyX[i] && selectionY == enemyNearbyY[i])
            {
                Units enemy = Units[selectionX, selectionY];
                bool isAlive = enemy.TakeDamage(SelectedUnit.Attack);
                if (isAlive)
                {
                    bool isAlive2 = SelectedUnit.TakeDamage(enemy.Attack);
                    if (!isAlive2)
                    {
                        //remove from active units and red / blue list and destroy unit
                    }
                }
                else
                {
                    //remove from active units and red / blue list and destroy unit
                }
                UnityEngine.Debug.Log("ATTACK");
                inAttack = false;
                break;
            }
        }
    }
}


