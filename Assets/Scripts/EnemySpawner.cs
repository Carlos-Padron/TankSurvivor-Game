using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameManager _gameManager;
    [SerializeField] GameObject[] _spawnPoints;

    // stores the enemy prefab
    [SerializeField] GameObject _enemy;

    float _spawnTime = 2f;
    float _spawnRateIncrease = 5f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNextEnemy());
        StartCoroutine(SpawnRateIncrese());
    }

    IEnumerator SpawnNextEnemy()
    {
        // spawn a new enemy
        int spawnPointIndex = Random.Range(0, _spawnPoints.Length);
        Instantiate(_enemy, _spawnPoints[spawnPointIndex].transform.position, Quaternion.identity);

        // wait for X seconds
        yield return new WaitForSeconds(_spawnTime);

        // then if gameover is false, spawn new enemy
        if (!_gameManager.gameOver)
        {
            StartCoroutine(SpawnNextEnemy());
        }
    }



    IEnumerator SpawnRateIncrese()
    {
        // waits X seconds before increasing the spawn rate
        yield return new WaitForSeconds(_spawnRateIncrease);

        // decrese spawnTime
        // limit spawn time is 0.5f
        if (_spawnTime >= 0.5f)
        {
            _spawnTime -= 0.2f;
        }

        StartCoroutine(SpawnRateIncrese());

    }

}
