using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerupContainer;
    // Start is called before the first frame update
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private float[] _spawnEnemiesSpeed = { 5.0f, 4.0f, 3.0f, 2.0f, 1.0f, 0.8f, 0.6f };
    [SerializeField]
    private float _currentSpawnEnemiesSpeed;
    [SerializeField]
    private int _enemiesThreasshold = 50;
    void Start()
    {
        _currentSpawnEnemiesSpeed = _spawnEnemiesSpeed[0];
        _enemiesThreasshold = 50;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {

        yield return new WaitForSeconds(3.0f);

        while(!_stopSpawning)
        {
            var posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_currentSpawnEnemiesSpeed);
        }
        
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            var seconds = Random.Range(3.0f, 7.0f);
            var posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);

            GameObject newPowerUp = Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            newPowerUp.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(seconds);

        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void OnPlayerAddScore(int score)
    {
        CalculateEnemiesSpeed(score);
    }

    private void CalculateEnemiesSpeed(int score)
    {
        var index = score / _enemiesThreasshold;
        if (index < _spawnEnemiesSpeed.Length) { 
            _currentSpawnEnemiesSpeed = _spawnEnemiesSpeed[index];
        }
    }
}
