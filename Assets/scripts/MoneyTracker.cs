using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MoneyTracker : MonoBehaviour
{   
    public GameObject Money;
    [SerializeField]
    private int money;
    [SerializeField]
    private TextMeshProUGUI moneyAmount;

    void Start() {
        money = Money.GetComponent<PlayerManager>().currMoney;
    }

    void Update()
    {
        money = Money.GetComponent<PlayerManager>().currMoney;
        moneyAmount.text = money.ToString("f0");
    }
}
