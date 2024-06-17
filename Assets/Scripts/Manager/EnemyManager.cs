using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs; // �� ������ �迭
    [SerializeField] private Transform spawnPoint;  // ���� ��ġ
    private GameObject currentEnemy;

    public static int EnemyCount { get; private set; } = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemiesAtIntervals());
    }

    private IEnumerator SpawnEnemiesAtIntervals()
    {
        while (true)
        {
            if (currentEnemy == null) // ���� ���� ���� ���� ���ο� ���� ����
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(3f); // 1�� �������� üũ
        }
    }

    public void SpawnEnemy()
    {
        if (currentEnemy == null)
        {
            EnemyCount++;
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            currentEnemy = Instantiate(enemyPrefabs[randomEnemyIndex],
                                       spawnPoint.position,
                                       spawnPoint.rotation);
            // ���� �׾��� �� �ݹ��� ����
            EnemyCharacter enemyCharacter = currentEnemy.GetComponent<EnemyCharacter>();
            enemyCharacter.OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        if (currentEnemy != null)
        {
            // ���� OnDeath �̺�Ʈ ����
            EnemyCharacter enemyCharacter = currentEnemy.GetComponent<EnemyCharacter>();
            enemyCharacter.OnDeath -= OnEnemyDeath;
            Destroy(currentEnemy);
            EnemyCount--;
            currentEnemy = null;
        }
    }
}
