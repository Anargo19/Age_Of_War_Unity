using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Controller : MonoBehaviour
{
    [SerializeField] bool unitsInFront = false;
    [SerializeField] bool enemyInFront = false;
    public bool noMove = false;
    LayerMask layerMask;
    BoxCollider2D collider;
    [SerializeField] GameObject spriteObject;
    public UnitScriptable unit;
    [SerializeField] Image healthbar;
    SpriteRenderer sprite;
    public bool player1;
    Vector3 forward;
    Vector3 anchor;
    Game_Manager manager;
    GameObject ownCollider;


    [SerializeField] GameObject DyingCanvas;

   [SerializeField] Transform enemy;
    GameObject rangedEnemy;

    [SerializeField] int health = 1;
    // Start is called before the first frame update
    void Start()
    {
        collider = spriteObject.GetComponent<BoxCollider2D>();
        ownCollider = collider.gameObject;
        health = unit.health;
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game_Manager>();
        sprite = spriteObject.GetComponent<SpriteRenderer>();
        sprite.sprite = unit.sprites[0];

        if (!player1)
        {
            gameObject.layer = LayerMask.NameToLayer("Player2");
        }
        
    }

    IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(0.5f);
        while (unitsInFront)
        {
            int random = Random.Range(0, 100);
            if (random < unit.critChance)
            {
                Debug.LogWarning("Critic !");
                enemy.GetComponent<Unit_Controller>().Change_Health(unit.damage * 2);

            }
            else
            {

                enemy.GetComponent<Unit_Controller>().Change_Health(unit.damage);
            }
            yield return new WaitForSeconds(1);

        }

        yield return null;
    }
    IEnumerator RangedAttack()
    {
        sprite.sprite = unit.sprites[1];
        yield return new WaitForSeconds(0.5f);
        while (!enemyInFront)
        {
            int random = Random.Range(0, 100);
            if (random < unit.critChance)
            {
                Debug.LogWarning("Critic !");
                rangedEnemy.GetComponent<Unit_Controller>().Change_Health(unit.damage * 2);

            }
            else
            {

                rangedEnemy.GetComponent<Unit_Controller>().Change_Health(unit.damage);
            }
            yield return new WaitForSeconds(1);

        }

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
        SetDirection();

        if (!unit.ranged)
            MeleeBehavior();
        
        else
            RangedBehavior();

        
    }

    private void SetDirection()
    {
        if (!player1)
        {
            forward = Vector2.left;
            anchor = transform.position - new Vector3(collider.size.x / 2 + 0.02f, -collider.size.y/2, 0);   

        }
        else
        {
            forward = Vector2.right;
            anchor = transform.position + new Vector3(collider.size.x / 2 + 0.02f, collider.size.y / 2, 0);
        }
    }

    public void Change_Health(int amount)
    {
       
            healthbar.transform.parent.gameObject.SetActive(true);
        
        health -= amount;
        healthbar.fillAmount = (float)health / unit.health;
        Debug.Log(health + " + " +  unit.health + " = " + (float)health / unit.health);
        if(health <= 0)
        {
            StopAllCoroutines();
            for(int t = 11; t > 0; t--)
            {
                spriteObject.GetComponent<SpriteRenderer>().material.SetFloat("_StepSlider", (t / 11) * 1.15f);
            }
            if (!player1)
            {
                manager.AddMoney(unit.moneyOut);
                manager.AddXP(unit.experience);
                GameObject dying = Instantiate(DyingCanvas);
                dying.transform.position = transform.position;
                dying.GetComponent<DyingCanvas>().textmesh.text = "+ " + unit.moneyOut;
            }
            Destroy(enemy.parent.gameObject);
            
        }
    }

    public void MeleeBehavior()
    {
        if (!unitsInFront)
        {
            transform.Translate(forward * Time.deltaTime * 0.7f);



            RaycastHit2D raycastHit2D = Physics2D.Raycast(anchor, forward, 0.1f);
                Debug.DrawRay(anchor, new Vector3(forward.x * 0.1f, 0, 0) , Color.red);
            if (raycastHit2D)
            {
                if (raycastHit2D.collider.gameObject != ownCollider)
                {
                    unitsInFront = true;
                    enemy = raycastHit2D.collider.transform.parent;
                    Debug.Log(enemy.name);
                    if (enemy.GetComponent<Unit_Controller>().player1 != player1)
                    {
                        StartCoroutine(MeleeAttack());

                    }
                    else
                    {
                        enemy = null;
                    }
                }
            }

        }
        else if (unitsInFront && !noMove)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(anchor, forward, 0.1f);
            Debug.DrawRay(anchor, new Vector3(forward.x * 0.1f, 0, 0), Color.red);
            if (raycastHit2D.collider == null)
            {
                unitsInFront = false;
                enemyInFront = false;

            }
        }
    }
    public void RangedBehavior()
    {
        if (!enemyInFront)
        {
            if (unit.ranged && rangedEnemy == null)
            {
                Debug.Log("No one in front");
                RaycastHit2D collider = Physics2D.Raycast(anchor, forward, unit.range, LayerMask.GetMask("Player2"));
                Debug.DrawRay(anchor, new Vector3(forward.x * unit.range, 0, 0), Color.red);
                if (collider)
                {
                    rangedEnemy = collider.transform.gameObject;
                    StartCoroutine(RangedAttack());


                }
            }
        }

        if (!unitsInFront && !noMove)
        {

            transform.Translate(forward * Time.deltaTime * 0.7f);



            RaycastHit2D raycastHit2D = Physics2D.Raycast(anchor, forward, 0.1f);
            Debug.DrawRay(anchor, new Vector3(forward.x, 0, 0), Color.red);
            if (raycastHit2D)
            {
                if (raycastHit2D.collider.gameObject != gameObject)
                {
                    unitsInFront = true;
                    enemy = raycastHit2D.collider.transform.parent;
                    if (enemy.GetComponent<Unit_Controller>().player1 != player1)
                    {
                        enemyInFront = true;
                        rangedEnemy = null;
                        StopCoroutine(RangedAttack());
                        StartCoroutine(MeleeAttack());

                    }
                    else
                    {
                        enemy = null;
                    }
                }
            }

        }
        else if (unitsInFront && !noMove)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(anchor, forward, 0.1f);
            Debug.DrawRay(anchor, forward, Color.red);
            if (!raycastHit2D)
            {
                unitsInFront = false;
                enemyInFront = false;

            }
        }
    }


}
