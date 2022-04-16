using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Unit
{
    infantry,
    ranged,
    tank
}

[CreateAssetMenu(fileName = "Unit")]
public class UnitScriptable : ScriptableObject
{
    public int health;
    public Sprite[] sprites;
    public int damage;
    public int experience;
    public int moneyOut;
    public int price;
    public int critChance;
    public bool ranged;
    public float range;


}
