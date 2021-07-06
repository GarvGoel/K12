using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    GameManager gameManager;

    public enum state { left, right, none}
    public state helicopterState = state.none;
    float xdropPoint;
    bool droppedMan = false;

    public Transform DropPoint;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnEnable()
    {
        Invoke("RandomDropPoint",0.1f);
        droppedMan = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);

        if (helicopterState == state.left)
        {
            transform.position += new Vector3(3f * Time.deltaTime, 0, 0);
            if (screenPos.x > 1.1)
            {
                gameObject.SetActive(false);
                HelicopterController.instance.helicopterPool.Add(gameObject);
            }
        }

        else if (helicopterState == state.right)
        {
            transform.position -= new Vector3(2.8f * Time.deltaTime, 0, 0);
            if (screenPos.x < -0.1)
            {
                gameObject.SetActive(false);
                HelicopterController.instance.helicopterPool.Add(gameObject);
            }
        }

        if(screenPos.x >=xdropPoint-0.05 && screenPos.x<= xdropPoint+0.05 && !droppedMan)
        {
            droppedMan = true;
           // Debug.Log("Drop " +  xdropPoint);
            GameObject Man= Aimer.instance.manPool[0];
            Man.SetActive(true);
            Man.transform.position = DropPoint.position;
            Man.transform.GetChild(0).gameObject.SetActive(false);
            Man.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2f);
            Man.GetComponent<Rigidbody2D>().gravityScale = 0.18f;
            Aimer.instance.manPool.RemoveAt(0);


        }


    }

    void RandomDropPoint()
    {
        float lowerValue;
        float upperValue;
        if (GameManager.instance.leftChecking == true)
        {
             lowerValue = 50;
        }
        else
        {
            lowerValue = 5;
        }
        if(GameManager.instance.rightChecking == true)
        {
            upperValue = 50;
        }
        else
        {
            upperValue = 95;
        }
       // Debug.Log("lower,upper= " + lowerValue + "," + upperValue);
        float abc = Random.Range(lowerValue, upperValue);
        xdropPoint = abc / 100;
       // Debug.Log(dropPoint);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            gameManager.score += 10;

            collision.gameObject.SetActive(false);
            Aimer.instance.bulletsPool.Add(collision.gameObject);

            gameObject.SetActive(false);
            HelicopterController.instance.helicopterPool.Add(gameObject);
        }
    }
}
