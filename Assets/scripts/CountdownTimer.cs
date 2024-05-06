using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownTimer : MonoBehaviour
{   
    public static int goal;
    public int currGoal;

    public GameObject Money;

    [SerializeField]
    private float timer;
    [SerializeField]
    private TextMeshProUGUI timerSeconds;

    void Start() {
        if (goal == 0) {
            goal = 20;
        }
        currGoal = goal;
        timer = 120;
        goal += 12;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timerSeconds.text = timer.ToString("f0");
        if (timer <= 0)
        {   
            if (Money.GetComponent<PlayerManager>().currMoney >= currGoal) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Debug.Log("reset");
            }
            else if (Money.GetComponent<PlayerManager>().ended == false && Money.GetComponent<PlayerManager>().currMoney < goal) {
                Debug.Log("ended");
                SceneManager.LoadSceneAsync(0);
                timer = 1;
                goal = 0;
                Money.GetComponent<PlayerManager>().ended = true;
            }
        }
    }
}
