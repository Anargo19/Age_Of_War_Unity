using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_spawner : MonoBehaviour
{
    public GameObject[] units;
    [SerializeField] bool isEnemy;
    Game_Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game_Manager>();
        if (isEnemy)
        {
            StartCoroutine(UnitSpawn());

        }
    }

    IEnumerator UnitSpawn()
    {
        while (true)
        {
            GameObject unit = Instantiate(units[0], this.transform);
            Transform transform = unit.transform;
           
                transform.localScale = new Vector3(unit.transform.localScale.x * -1, unit.transform.localScale.y, unit.transform.localScale.z);
                //transform.position = new Vector3(transform.position.x, transform.position.y + unit.transform.localScale.y / 2, transform.position.z);
                Unit_Controller unit_Controller = unit.GetComponent<Unit_Controller>();
                unit_Controller.player1 = false;
                yield return new WaitForSeconds(5);

        }
    }

    public void SpawnUnit(int id, int money)
    {
        int price = units[id].GetComponent<Unit_Controller>().unit.price;
        if (price > money)
        {
            Debug.Log("Too pricey !");
            return;
        }
        GameObject unit = Instantiate(units[id], this.transform);
        Transform transform = unit.transform;

        transform.localScale = new Vector3(unit.transform.localScale.x * -1, unit.transform.localScale.y, unit.transform.localScale.z);
       // transform.position = new Vector3(transform.position.x, transform.position.y + unit.transform.localScale.y / 2, transform.position.z);
        if (isEnemy)
        {
            unit.transform.localScale = new Vector3(unit.transform.localScale.x * -1, unit.transform.localScale.y, unit.transform.localScale.z);
            Unit_Controller unit_Controller = unit.GetComponent<Unit_Controller>();
            unit_Controller.player1 = false;



        }
        else
        {
            Unit_Controller unit_Controller = unit.GetComponent<Unit_Controller>();
            unit_Controller.player1 = true;
            manager.AddMoney(-price);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
