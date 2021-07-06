using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : MonoBehaviour
{
    public enum ManState { idle, attack }
    public ManState manState; 

    bool parachuteOpen = false;
    bool stopMoving = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnEnable()
    {
        parachuteOpen = false;
        manState = ManState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);

        if (screenPos.y < 0.65  && !parachuteOpen)
        {
            //GetComponent<Rigidbody2D>().gravityScale = 0.05f;  //decrease the gravity when parachute is open
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1f);
            transform.GetChild(0).gameObject.SetActive(true);
            parachuteOpen = true;
          
        }


        if(manState == ManState.attack)
        {
            if (screenPos.x < 0.5 && !stopMoving)
            {
                transform.position += new Vector3(1 * Time.deltaTime, 0, 0);
            }
            else if(screenPos.x > 0.5 && !stopMoving)
            {
                transform.position -= new Vector3(1 * Time.deltaTime, 0, 0);
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("man"))
        {
            GameObject collider = collision.gameObject;

            if (collider.CompareTag("gun"))
            {
                Debug.Log("Game Over");
                GameManager.instance.GameOver();
             
            }

            else if (collider.CompareTag("platform"))
            {
                if (transform.GetChild(0).gameObject.activeSelf)
                {
                    gameObject.tag = "landman";
                    //parchute was open
                    transform.GetChild(0).gameObject.SetActive(false);
                    if (transform.position.x < 0)
                    {
                        GameManager.instance.manOnLeft += 1;
                        GameManager.instance.LeftManList.Add(gameObject);
                        if (GameManager.instance.leftManOnAttack)
                        {
                            manState = ManState.attack;
                        }
                    }
                    else if (transform.position.x > 0)
                    {
                        GameManager.instance.manOnRight += 1;
                        GameManager.instance.RightManList.Add(gameObject);
                        if (GameManager.instance.rightManOnAttack)
                        {
                            manState = ManState.attack;
                        }
                    }
                }
                else
                {
                    //parachute was not open, disable the man
                    gameObject.SetActive(false);
                    Aimer.instance.manPool.Add(gameObject);
                }
            }

            else if (collider.CompareTag("landman"))
            {
                if (transform.GetChild(0).gameObject.activeSelf)  //if parachute is on when landing on platform
                {
                    Debug.Log("Parachute was on while landing on man");
                    transform.GetChild(0).gameObject.SetActive(false);
                    gameObject.tag = "landman";
                    if (transform.position.x < 0)
                    {
                        transform.position -= new Vector3(0.300f, 0, 0);
                        GameManager.instance.manOnLeft += 1;
                        GameManager.instance.LeftManList.Add(gameObject);
                    }
                    else if (transform.position.x > 0)
                    {
                        transform.position += new Vector3(0.300f, 0, 0);
                        GameManager.instance.manOnRight += 1;
                        GameManager.instance.RightManList.Add(gameObject);
                    }
                }
                else  // if parachute is not on when landing on platform
                {

                    //disable both the man
                    Debug.Log("Parachute not on while landing on man");
                    if (transform.position.x < 0)
                    {
                        GameManager.instance.manOnLeft -= 1;
                        //GameManager.instance.LeftManList.Remove(collider);
                    }
                    else if (transform.position.x > 0)
                    {
                        GameManager.instance.manOnRight -= 1;

                    }
                    GameManager.instance.RightManList.Add(collider);


                    gameObject.SetActive(false);
                    Aimer.instance.manPool.Add(gameObject);

                    collider.SetActive(false);
                    Aimer.instance.manPool.Add(collider);
                }
            }
        }

        else if(gameObject.CompareTag("landman"))
        {
            if (collision.collider.CompareTag("gun"))
            {
                stopMoving = true;
                gameObject.tag = "bridge1";
                this.enabled = false;
                //stop moving
            }

            else if (collision.collider.CompareTag("bridge1"))
            {
                if (collision.collider.transform.childCount == 1)
                {
                    
                    stopMoving = true;
                    gameObject.tag = "bridge2";
                    transform.position = new Vector2(collision.collider.transform.position.x, collision.collider.transform.position.y + 0.312f);
                    transform.SetParent(collision.collider.transform);
                    this.enabled = false;
                }
                else if (collision.collider.transform.childCount == 2)
                {
                    stopMoving = true;
                    gameObject.tag = "bridge3";
                    this.enabled = false;
                }
            }

            else if (collision.collider.CompareTag("bridge3"))
            {
                stopMoving = true;
                Debug.Log("Hurry");
                transform.position = new Vector2(collision.collider.transform.position.x, collision.collider.transform.position.y + 0.312f);
                Invoke("Step2", 0.5f);
            }

            
        }
    }

    void Step2()
    {
        GameObject bridge2 = GameObject.FindGameObjectWithTag("bridge2");
        transform.position = new Vector2(bridge2.transform.position.x, bridge2.transform.position.y + 0.312f);
        Invoke("Step3", 0.5f);
    }
    void Step3()
    {
        if (transform.position.x < 0)
        {
            transform.position = new Vector2(-0.597f, transform.position.y + 0.400f);
        }
        if (transform.position.x >0)
        {
            transform.position = new Vector2(+378f, transform.position.y + 0.400f);
        }
        Debug.Log("GameOver");
        GameManager.instance.GameOver();
       
    }
}
