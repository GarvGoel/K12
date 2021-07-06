using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
       // Debug.Log(pos);

        if(pos.x<0 || pos.x>1 || pos.y > 1)   //disable bullet if it is out of view port
        {
            gameObject.SetActive(false);
            Aimer.instance.bulletsPool.Add(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.gameObject;

        //when bullet hits the parachute
         if (collision.collider.gameObject.name == "Parachute")
         {
            GameManager.instance.score += 5;
            Rigidbody2D rigidbody2D = collider.GetComponent<Rigidbody2D>();

            collision.collider.gameObject.SetActive(false);
            rigidbody2D.velocity = new Vector2(0, -5f);
           // rigidbody2D.gravityScale = 0.18f;
         }

         //when bullet hits the man
        else if (collider.CompareTag("man"))
        {
            GameManager.instance.score += 5;
            collider.transform.GetChild(0).gameObject.SetActive(false);  //disable the parachute
           // collider.GetComponent<Rigidbody2D>().gravityScale = 0.18f; 
            collider.SetActive(false);
            Aimer.instance.manPool.Add(collider);
        }

        

        gameObject.SetActive(false);
        Aimer.instance.bulletsPool.Add(gameObject);
    }
}
