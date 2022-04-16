using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public Unit_spawner player1;
    public int player1Money = 175;
    public int player1Age = 0;
    public TextMeshProUGUI player1MoneyText;
    public int player1XP = 0;
    public TextMeshProUGUI player1XPText;



    public Unit_spawner player2;
    public int player2Money = 0;
    [SerializeField] List<UnitScriptable> infantry = new List<UnitScriptable> ();

    private void Start()
    {
        player1MoneyText.text = player1Money.ToString();
    }

    public void AddMoney(int amount)
    {
        player1Money += amount;
        player1MoneyText.text = player1Money.ToString();
        
    }
    public void AddXP(int amount)
    {
        player1XP += amount;
        player1XPText.text = player1XP.ToString();
    }

    public void SpawnUnit(int id)
    {
        player1.SpawnUnit(id, player1Money);
    }
}
