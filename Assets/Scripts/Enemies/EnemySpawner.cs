using UnityEngine;
using NavMeshPlus.Components;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using TMPro;

[System.Serializable]
public class EnemyWave
{
    public int count = 15;
    public float spawnDelay = 0.5f;
    public List<Transform> spawnPoints = new();
}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float waveDelay = 3f;
    [SerializeField] NavMeshSurface navMesh;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] CardHand hand;
    [SerializeField] TextMeshProUGUI waveStatusText;
    [SerializeField] List<EnemyWave> enemyWaves = new();
    List<NavMeshAgent> enemies = new();

    void Start()
    {
        StartCoroutine(WaveSpawning());
    }

    int RandomSign()
    {
        return Random.Range(0, 1) == 0 ? 1 : -1;
    }

    int enemyCounter;
    IEnumerator WaveSpawning()
    {
        waveStatusText.text = "Enemies approaching";

        yield return new WaitForSeconds(2 * waveDelay);

        for (int i = 0; i < enemyWaves.Count; i++)
        {
            enemyCounter = enemyWaves[i].count;

            for (int j = 0; j < enemyWaves[i].count; j++)
            {
                yield return new WaitForSeconds(enemyWaves[i].spawnDelay);

                SpawnEnemy(
                    (Vector2)enemyWaves[i].spawnPoints[j % enemyWaves[i].spawnPoints.Count].position
                    + new Vector2(Random.Range(0.5f, 1f) * RandomSign(), Random.Range(0.5f, 1f)) * RandomSign());

                waveStatusText.text = "Enemies remaining: " + enemyCounter;
            }

            while (enemyCounter != 0)
            {
                waveStatusText.text = "Enemies remaining: " + enemyCounter;

                yield return new WaitForSeconds(0.1f);
            }

            waveStatusText.text = "Next wave of enemies approaching";
            hand.DrawCard();

            yield return new WaitForSeconds(waveDelay);
        }
        waveStatusText.text = "You win!";
    }

    void SpawnEnemy(Vector2 position)
    {
        var enemy = Instantiate(enemyPrefab, position, enemyPrefab.transform.rotation).GetComponent<Enemy>();
        if (!enemy)
        {
            return;
        }
        enemy.deathCallback = RemoveEnemy;
        enemy.reachBaseCallback = EnemyReachBase;

        var enemyNavmeshAgent = enemy.GetComponent<NavMeshAgent>();
        if (enemyNavmeshAgent)
        {
            enemyNavmeshAgent.autoBraking = false;
            enemyNavmeshAgent.speed = enemy.speed;
            enemyNavmeshAgent.SetDestination(Vector2.zero);
            enemyNavmeshAgent.isStopped = false;
            enemies.Add(enemyNavmeshAgent);
        }
    }

    public void UpdateNavmesh()
    {
        StartCoroutine(UpdateNavmeshAsync());
    }

    IEnumerator UpdateNavmeshAsync()
    {
        yield return new WaitForEndOfFrame();
        yield return navMesh.BuildNavMeshAsync();

        foreach (var enemy in enemies)
        {
            enemy.SetDestination(Vector2.zero);
        }
    }

    void RemoveEnemy(GameObject enemy)
    {
        var agent = enemy.GetComponent<NavMeshAgent>();
        if (agent)
        {
            enemies.Remove(agent);
        }
        enemyCounter--;
    }

    void EnemyReachBase()
    {

    }
}
