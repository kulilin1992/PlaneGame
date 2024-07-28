using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0? null : enemyList[Random.Range(0, enemyList.Count)];

    public int WaveNumber => waveNumber;

    public float TimeBetweenWaves => timeBetweenWaves;


    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float timeBetweenCreate = 2f;
    [SerializeField] float timeBetweenWaves = 4;
    [SerializeField] int minEnemyAmount = 4;
    [SerializeField] int maxEnemyAmount = 8;
    
    [SerializeField] bool createEnemy = true;

    [SerializeField] GameObject waveUI;
    int waveNumber = 1;
    int enemyAmount;

    List<GameObject> enemyList;

    WaitForSeconds waitTimeBetweenCreate;
    WaitForSeconds waitTimeBetweenWaves;

    WaitUntil waitUntilNoEnemy;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenCreate = new WaitForSeconds(timeBetweenCreate);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);
    }

    IEnumerator Start()
    {
        while (createEnemy)
        {
            //yield return waitUntilNoEnemy;

            waveUI.SetActive(true);
            yield return waitTimeBetweenWaves;

            waveUI.SetActive(false);
            yield return StartCoroutine(nameof(RandomSpawnCoroutine));
        }
    }

    IEnumerator RandomSpawnCoroutine()
    {
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);
        for (int i = 0; i < enemyAmount; i++)
        {
            var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            enemyList.Add(PoolManager.Release(enemy));
            yield return waitTimeBetweenCreate;
        }

        yield return waitUntilNoEnemy;
        waveNumber++;
    }

    public void RemoveEnemy(GameObject enemy) => enemyList.Remove(enemy);
}
