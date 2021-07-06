using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
     

    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("bullet"))
        {
            collision.gameObject.SetActive(false);
            Aimer.instance.bulletsPool.Add(collision.gameObject);

            gameObject.SetActive(false);
            Aimer.instance.bombPool.Add(gameObject);
        }

        else if (collision.CompareTag("gun"))
        {
            // gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.SetActive(false);
            Debug.Log("Game Over");
            GameManager.instance.GameOver();
        }
    }
}
