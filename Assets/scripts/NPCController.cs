using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public AudioClip soundEffect;
    public AudioClip soundEffect1;
    public AudioClip soundEffect2;
    public bool isDrunk;
    public bool isMad;
    Animator anim;
    public float patienceLevel;

    [SerializeField]
    PlayerManager manager;

    [SerializeField]
    float patience;
    
    void Start() {
        anim = gameObject.GetComponent<Animator>();
        isDrunk = false;
        isMad = false;
    }

    public void gotDrink()
    {
        if(!isMad)
        {
            if(!isDrunk)
            {    
                PlayerManager manager = FindObjectOfType<PlayerManager>(true);
                if(manager)
                {
                    if(manager.beerCount > 0)
                    {
                        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
                        isDrunk = true;
                        manager.GiveDrink();
                        anim.SetTrigger("Drunk");
                        int rando = Random.Range(0, 2);
                        if (rando == 1)
                        {
                            NPCBehavior Speed = gameObject.GetComponent<NPCBehavior>();
                            Speed.speed = 1;
                        }
                        else 
                        {
                            anim.SetTrigger("Despawn");
                            StartCoroutine(despawn());
                        }
                    }
                    else 
                    {
                        AudioSource.PlayClipAtPoint(soundEffect1, transform.position);
                    }
                }
            }   
            else 
            {
                AudioSource.PlayClipAtPoint(soundEffect1, transform.position);
            } 
        }
        else 
        {
            AudioSource.PlayClipAtPoint(soundEffect1, transform.position);
        } 
    }

    void Update() {
        if (!isDrunk) {
            if (patience < patienceLevel)
            {
                patience += Time.deltaTime;
            }
            else
            {
                isMad = true;
            }
        }

        if (isMad) {
            anim.SetTrigger("Mad");
            NPCBehavior Speed = gameObject.GetComponent<NPCBehavior>();
            Speed.speed = 4;
        }
        
    }

    IEnumerator despawn() {
        yield return new WaitForSeconds(1);
        AudioSource.PlayClipAtPoint(soundEffect2, transform.position);
        Destroy (gameObject);
        
    }

}
