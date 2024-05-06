using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public AudioClip soundEffect;
    public AudioClip soundEffect2;
    public AudioClip soundEffect3;
    public int beerCount;
    public static int money;
    public bool ended;
    public int currMoney;
    public int price;
    public GameObject interactBeer;
    public GameObject interactNotification;
    Animator anim;
    public GameObject Goal;

    void Start() {
        ended = false;
        anim = gameObject.GetComponent<Animator>();
        interactBeer.SetActive(false);
        interactNotification.SetActive(false);
        if (money != 0) {
            money -= Goal.GetComponent<CountdownTimer>().currGoal - 12;
        }
    }
    public void PickupBeer()
    {
        if (beerCount == 1) {}
        else {
            interactBeer.SetActive(true);
            anim.SetTrigger("Holding");
            AudioSource.PlayClipAtPoint(soundEffect, transform.position);
            beerCount++;
            Debug.Log("Picked up drink");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (beerCount == 1) {
            if (other.gameObject.name != "Interactable circle") {
                interactBeer.SetActive(false);
                anim.SetTrigger("Holding");
                AudioSource.PlayClipAtPoint(soundEffect2, transform.position);
                beerCount--;
                Debug.Log("Dropped drink");
            }
        }
    }

    public void GiveDrink()
    {
        interactBeer.SetActive(false);
        anim.SetTrigger("Holding");
        beerCount--;
        Debug.Log("Gave away drink");
        Debug.Log(money);
        money += price;
        AudioSource.PlayClipAtPoint(soundEffect3, transform.position);
    }

    public void NotifyPlayer()
    {
        
        interactNotification.SetActive(true);
    }

    public void DeNotifyPlayer()
    {
        interactNotification.SetActive(false);

    }

    void Update() {
        currMoney = money;
        if (ended == true) {
            money = 0;
        }
    }
}
