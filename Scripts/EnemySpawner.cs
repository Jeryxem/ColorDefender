using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] ListOfEnemyToSpawn; // Add using editor
    private float spawnDelay = 2.5f;

    [SerializeField] GameObject enemyRed, enemyBlue, enemyYellow;
    [SerializeField] GameObject secondaryEnemyOrange, secondaryEnemyPurple, secondaryEnemyGreen;


    private void Start()
    {
        GameManager.instance.Invoke("RemoveLevelNameOnGameStart", 2f); // TODO: uncomment after debug

        GameManager.instance.enemyCount += ListOfEnemyToSpawn.Length;
        StartCoroutine(SpawnEnemyWave());
    }

    IEnumerator SpawnEnemyWave()
    {
        // This spawn enemy only after UI level name disappear by waiting 1 sec longer
        yield return new WaitForSeconds(3f);

        for(int i = 0; i < ListOfEnemyToSpawn.Length; i++)
        {
            GameObject newEnemy = Instantiate(ListOfEnemyToSpawn[i], transform.position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;
            GameManager.instance.enemyCount -= 1;
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SpawnPrimaryEnemy(string color, Vector3 collisionPoint, Transform parent)
    {
        GameObject newEnemy;
        switch (color)
        {
            case "Red":
                newEnemy = Instantiate(enemyRed, collisionPoint, Quaternion.identity);
                break;
            case "Blue":
                newEnemy = Instantiate(enemyBlue, collisionPoint, Quaternion.identity);
                break;
            case "Yellow":
                newEnemy = Instantiate(enemyYellow, collisionPoint, Quaternion.identity);
                break;
            default:
                newEnemy = null;
                print("Spawn error, unavailable color");
                break;
        }
        newEnemy.transform.parent = parent;
    }

    public void SpawnSecondaryEnemy(string color, Vector3 collisionPoint, Transform parent)
    {
        GameObject newEnemy;

        switch (color)
        {
            case "Purple":
                newEnemy = Instantiate(secondaryEnemyPurple, collisionPoint, Quaternion.identity);
                break;
            case "Orange":
                newEnemy = Instantiate(secondaryEnemyOrange, collisionPoint, Quaternion.identity);
                break;
            case "Green":
                newEnemy = Instantiate(secondaryEnemyGreen, collisionPoint, Quaternion.identity);
                break;
            default:
                newEnemy = null;
                print("Spawn error, unavailable color");
                break;
        }

        newEnemy.transform.position = new Vector3(collisionPoint.x, 0f, collisionPoint.z);
        newEnemy.transform.parent = parent;
    }
}
