using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    public static Aimer instance;
    GameManager gameManager;

    [Tooltip("Rotating speed of aimer")]
    public float speed;

    [Tooltip("Speed of bullet")]
    public float bulletSpeed;

    public Transform AimerTip;

    [Header("Man Pool")]
    public List<GameObject> manPool;
    public GameObject manPrefab;
    public Transform Mans;

    [Header("Bullet Pool")]
    public List<GameObject> bulletsPool;
    public GameObject bulletPrefab;
    public Transform Bullets;

    [Header("Bomb Pool")]
    public List<GameObject> bombPool;
    public GameObject bombPrefab;
    public Transform Bombs;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       gameManager = GameManager.instance;

        for(int i = 0; i <20; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bulletsPool.Add(bullet);
            bullet.transform.SetParent(Bullets);
            bullet.SetActive(false);
        }

        for(int i = 0; i < 15; i++)
        {
            GameObject man = Instantiate(manPrefab);
            manPool.Add(man);
            man.transform.SetParent(Mans);
            man.SetActive(false);
        }

        for(int i = 0; i < 10; i++)
        {
            GameObject bomb = Instantiate(bombPrefab);
            bombPool.Add(bomb);
            bomb.transform.SetParent(Bombs);
            bomb.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region Aim Motion

        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //clamping the motion of aimer on both sides
        if(angle>=160  || angle <= -90)  
        {
            angle = 160;
        }
        else if(angle<=20 || (angle > -90 && angle<=0))
        {
            angle = 20;
        } 

        Quaternion rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

        #endregion


        // releasing bullet on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            if (gameManager.score > 0)
            {
               gameManager.score--;
            }
            GameObject bulletShooted = bulletsPool[0];
            bulletsPool.RemoveAt(0);
            bulletShooted.SetActive(true);
            bulletShooted.transform.position = AimerTip.position;
            Vector3 bulletDirection = (AimerTip.position - transform.position);
            bulletShooted.GetComponent<Rigidbody2D>().AddForce(bulletDirection * bulletSpeed);
            
        }



        
    }
}
