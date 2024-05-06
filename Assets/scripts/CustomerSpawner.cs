using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public float minMaxSpawn = 4f;

    [SerializeField]
    private GameObject _customerPrefab;

    [SerializeField]
    private float _minimumSpawnTime;

    static public float _maximumSpawnTime;

    private float _timeUntileSpawn;
    // Start is called before the first frame update
    void Start()
    {
        if (_maximumSpawnTime == 0)
            _maximumSpawnTime = 15;
        if (_maximumSpawnTime > minMaxSpawn)
            _maximumSpawnTime--;
        SetTimeUntilSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        _timeUntileSpawn -= Time.deltaTime;
        if (_timeUntileSpawn <= 0)
        {
            Instantiate(_customerPrefab, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();

        }
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntileSpawn = Random.Range(_minimumSpawnTime, _maximumSpawnTime);
    }
}
