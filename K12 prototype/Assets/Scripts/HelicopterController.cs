using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    public static HelicopterController instance;

    public List<GameObject> helicopterPool = new List<GameObject>();
    [SerializeField] GameObject helicopterPrefab;

    public List<GameObject> airplanePool = new List<GameObject>();
    [SerializeField] GameObject airplanePrefab;


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
        for(int i = 0; i < 15; i++)  //making the helicopter pool
        {
            GameObject helicopter = Instantiate(helicopterPrefab);
            helicopterPool.Add(helicopter);
            helicopter.transform.SetParent(transform);
            helicopter.SetActive(false);
        }
        for (int i=0;i<10;i++)  //making the airplane pool
        {
            GameObject airplane = Instantiate(airplanePrefab);
            airplanePool.Add(airplane);
            airplane.transform.SetParent(transform);
            airplane.SetActive(false);
        }
       StartCoroutine(InstantiateLeftHelicopter());
       StartCoroutine(InstantiateRightHelicopter());
       StartCoroutine(InstantiateAirplane());
    }

  

    IEnumerator InstantiateLeftHelicopter()
    {
        while (true)
        {

            Vector2 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f,0.95f,0),Camera.MonoOrStereoscopicEye.Mono);
            GameObject helicopterOut = helicopterPool[0];
            helicopterPool.RemoveAt(0);
            helicopterOut.SetActive(true);
            helicopterOut.transform.position = worldPos;
            helicopterOut.transform.rotation = Quaternion.Euler(0, 180, 0);
            helicopterOut.GetComponent<Helicopter>().helicopterState = Helicopter.state.left;
            float randomTimeGap = Random.Range(3, 5);
            yield return new WaitForSeconds(randomTimeGap);
        }
    }

    IEnumerator InstantiateRightHelicopter()
    {
        while (true)
        {
            float randomTimeGap = Random.Range(3, 5);
            yield return new WaitForSeconds(randomTimeGap);


            Vector2 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(+1.1f, 0.89f, 0), Camera.MonoOrStereoscopicEye.Mono);
            GameObject helicopterOut = helicopterPool[0];
            helicopterPool.RemoveAt(0);
            helicopterOut.SetActive(true);
            helicopterOut.transform.position = worldPos;
            helicopterOut.transform.rotation = Quaternion.Euler(0, 0, 0);
            helicopterOut.GetComponent<Helicopter>().helicopterState = Helicopter.state.right;

        }
    }

    IEnumerator InstantiateAirplane()
    {
        while (true)
        {
            yield return new WaitForSeconds(6f);
            int value = Random.Range(0, 2);

            // if value is 0, then instantiate plane aeroplane from left. otherwise from right
            if (value == 0)
            {
                Vector2 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(-0.01f, 0.83f, 0), Camera.MonoOrStereoscopicEye.Mono);
                GameObject airplaneOut = airplanePool[0];
                airplanePool.RemoveAt(0);
                airplaneOut.transform.position = worldPos;
                airplaneOut.transform.rotation = Quaternion.Euler(0, 180, 0);
                airplaneOut.SetActive(true);
                airplaneOut.GetComponent<Airplane>().airplaneState = Airplane.state.left;
            }
            else
            {
                Vector2 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(1.01f, 0.83f, 0), Camera.MonoOrStereoscopicEye.Mono);
                GameObject airplaneOut = airplanePool[0];
                airplanePool.RemoveAt(0);
                airplaneOut.SetActive(true);
                airplaneOut.transform.position = worldPos;
                airplaneOut.transform.rotation = Quaternion.Euler(0, 0, 0);
                airplaneOut.GetComponent<Airplane>().airplaneState = Airplane.state.right;
            }
        }
    
    }
}
