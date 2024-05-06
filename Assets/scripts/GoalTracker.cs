using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GoalTracker : MonoBehaviour
{   
    public GameObject Goal;
    [SerializeField]
    private int goal;
    [SerializeField]
    private TextMeshProUGUI goalAmount;

    void Start() {
        goal = Goal.GetComponent<CountdownTimer>().currGoal;
    }

    void Update() {
        goal = Goal.GetComponent<CountdownTimer>().currGoal;
        goalAmount.text = goal.ToString("f0");
    }
}
