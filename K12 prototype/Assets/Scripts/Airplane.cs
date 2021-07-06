using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    public enum state { left, right, none }
    public state airplaneState = state.none;


    [SerializeField] Transform DropPoint;
    Vector3 Target = new Vector3(-0.09f,-3.228f,0);

    float xdropPoint=0.3f;
    bool droppedBomb;
    public float bombSpeed;

    Vector2 screenPos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
       Invoke("AirplaneDropPoint",0.1f);
        droppedBomb = false;
    }

    // Update is called once per frame
    void Update()
    {
         screenPos = Camera.main.WorldToViewportPoint(transform.position);

        #region Airplane motion

        if (airplaneState == state.left)
        {
            transform.position += new Vector3(3f * Time.deltaTime, 0, 0);
            if (screenPos.x > 1.1)
            {
                gameObject.SetActive(false);
                HelicopterController.instance.airplanePool.Add(gameObject);
            }
        }

        else if (airplaneState == state.right)
        {
            transform.position -= new Vector3(2.8f * Time.deltaTime, 0, 0);
            if (screenPos.x < -0.1)
            {
                gameObject.SetActive(false);
                HelicopterController.instance.airplanePool.Add(gameObject);
            }
        }
        #endregion

        if(screenPos.x >= xdropPoint - 0.05 && screenPos.x <= xdropPoint + 0.05 && !droppedBomb)
        {
            droppedBomb = true;

            Vector3 direction = (Target - DropPoint.position).normalized;
            GameObject bomb = Aimer.instance.bombPool[0];
            bomb.SetActive(true);
            bomb.transform.position = DropPoint.position;
            Aimer.instance.bombPool.RemoveAt(0);
            bomb.GetComponent<Rigidbody2D>().velocity = direction * bombSpeed;
          

        }
    }

    void AirplaneDropPoint()
    {
        if (transform.position.x <0 )
        {
            float startPoint = Random.Range(5, 35);
            xdropPoint = startPoint / 100;
           // Debug.Log(" left xDropPoint of airplane= " + xdropPoint);
           
        }
        else
        {
            float startPoint = Random.Range(65,95);
            xdropPoint = startPoint / 100;
            //Debug.Log("right xDropPoint of airplane= " + xdropPoint);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            GameManager.instance.score += 10;

            collision.gameObject.SetActive(false);
            Aimer.instance.bulletsPool.Add(collision.gameObject);

            gameObject.SetActive(false);
            HelicopterController.instance.airplanePool.Add(gameObject);
        }
    }
}
