using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This allows us to right click and do Create->ArabicGames->CreateGameData
/// </summary>
[CreateAssetMenu(fileName = "GameData", menuName = "ArabicGames/CreateGameData", order = 1)]
// Must extend Scriptable Object
public class GameData : ScriptableObject
{
    [System.Serializable]
    public class GameDataParams
    {
        public string Name;
        public int Age;
        public bool IsAlly;
        public bool IsEnemy;
        public bool IsControllable;
        public float Health;
        public float Mana;
        public float Attack;
        public float MagicDamage;
        public float Armor;
        public float MagicResist;
        public float Luck;
        public float CritChance;
        public float AttackSpeed;
        public float EvasionSpeed;
        public float CooldownReduction;
        public float AbilityCooldown;
    }

    public GameDataParams[] gameDataParams = new GameDataParams[5];
}
