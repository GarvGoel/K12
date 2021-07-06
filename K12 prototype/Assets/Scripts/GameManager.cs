using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] TextMeshProUGUI actualScore;

    public int manOnLeft = 0;
    public List<GameObject> LeftManList;
    public bool leftChecking=false;
    public bool leftManOnAttack=false;

    public int manOnRight=0;
    public List<GameObject> RightManList;
    public bool rightChecking = false;
    public bool rightManOnAttack = false;

    public GameObject GameOverPanel;
    public GameObject CentrePoint;
    public GameObject Gun;

    public int score;

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
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(manOnLeft >= 4 && !leftChecking)
        {
            //now wait for 2 seconds and tell the helicopter to stop dropping the man in left area
            leftChecking = true;
            StartCoroutine(LeftCheckAgain());
            //after waiting for 2 seconds again check if manOnLeft is >=4 or not. If yes, then change the state of Man on left to attack
            //change the state of all the man
           
        }

        if (manOnRight >= 4 && !rightChecking)
        {
            rightChecking = true;
            StartCoroutine(RightCheckAgain());

        }

        actualScore.text = score.ToString();

        
    }

    IEnumerator LeftCheckAgain()
    {

        yield return new WaitForSeconds(3f);
        if(manOnLeft >= 4)
        {
            Debug.Log("Left man on attack");
            leftManOnAttack = true;
            foreach (var man in LeftManList)
            {
                man.GetComponent<Man>().manState = Man.ManState.attack;

            }
        }
        else
        {
            leftManOnAttack = false;
            leftChecking = false;
        }
    }

    IEnumerator RightCheckAgain()
    {

        yield return new WaitForSeconds(3f);
        if (manOnRight >= 4)
        {
            Debug.Log("Right man on attack");
            rightManOnAttack = true;
            foreach (var man in RightManList)
            {
                man.GetComponent<Man>().manState = Man.ManState.attack;

            }
        }
        else
        {
            rightManOnAttack = false;
            rightChecking = false;
        }
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
        CentrePoint.SetActive(false);
        Gun.SetActive(false);
    }
}
