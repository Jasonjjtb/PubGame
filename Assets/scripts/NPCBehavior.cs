using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{

    [SerializeField]
    public float speed;
    [SerializeField]
    float range;

    public AudioClip soundEffect;

    Vector2 wayPoint;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        SetNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDestination();
        }
    } 

    public void SetNewDestination()
    {
        wayPoint = new Vector2(Random.Range(-11, 11), Random.Range(-13, 6));
    }

}
