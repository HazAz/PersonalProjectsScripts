using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public abstract class Units : MonoBehaviour
{

    public HealthBarScript healthbar;
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public bool isRed;
    public bool canMove;
    public int MaxHealth;
    public int CurrHealth;
    public int Attack;
    public int MaxMove;

    //public int AttackChance;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        bool[,] r = new bool[BoardRules.BoardX, BoardRules.BoardY];
        CheckGrid(r, MaxMove, CurrentX, CurrentY);
        return r;
    }
    
    public void StartHealth()
    {
        healthbar.SetMaxHealth(MaxHealth);
    }

    public virtual void CheckGrid(bool[,] r, int moves, int x, int y)
    {
        Units u0, u1;
        if (moves <= 0 || x > BoardRules.BoardX - 1 || x < 0 || y > BoardRules.BoardY - 1 || y < 0)
        {
            return;
        }
        u0 = BoardManager.Instance.Units[CurrentX, CurrentY];
        u1 = BoardManager.Instance.Units[x, y];
        if (u1 != null && (x != CurrentX || y != CurrentY))
        {
            //if there's another unit there, set to unavailable
            r[x, y] = false;
            //if opposite teams, return
            if (u0.isRed != u1.isRed)
            {
                return;
            }
        }
        else
        {
            r[x, y] = true;
        }

        CheckGrid(r, moves - 1, x, y + 1);
        CheckGrid(r, moves - 1, x, y - 1);
        CheckGrid(r, moves - 1, x + 1, y);
        CheckGrid(r, moves - 1, x - 1, y);
    }

    //check if this works later
    public bool TakeDamage(int hit)
    {
        UnityEngine.Debug.Log("In Take Damage");
        CurrHealth -= hit;
        if (CurrHealth <= 0)
        {
            healthbar.SetHealth(0);
            //Destroy(this);
            return false;
        }
        healthbar.SetHealth(CurrHealth);
        return true;
    }
    

}
