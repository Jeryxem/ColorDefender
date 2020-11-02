using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    Pathfinder pathfinder;
    [SerializeField] List<Waypoint> path;
    [SerializeField] private int currentPathCount;
    public bool canMove = false;
    [SerializeField] GameObject deathVfxPrefab;

    void Start()
    {
        SetupMovement();
    }

    public void SetupMovement()
    {
        pathfinder = GetComponentInParent<Pathfinder>();

        if(pathfinder == null)
        {
            print("NO PARENT DETECTED - NULL");
            return;
        }

        path = pathfinder.GetPath();
        currentPathCount = 0;
        canMove = true;
    }

    private void Update()
    {
        if (path == null || !canMove) { return; }
        EnemyMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                CombineColor(collision);
                break;
            case "Defender":
                CompareColor(collision);
                break;
            case "PlayerHealth":
                Destroy();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Waypoint")
            return;

        Waypoint waypointColor = other.GetComponent<Waypoint>();

        if (waypointColor.GetTopColor() != Color.red &&
            waypointColor.GetTopColor() != Color.blue &&
            waypointColor.GetTopColor() != Color.yellow
            ) { return; }

        switch (other.gameObject.tag)
        {
            case "Waypoint":
                CombineColorWithWaypoint(other);
                break;
            default:
                break;
        }
    }

    // This fix ontrigger not firing when instantiating on top of waypoint
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Waypoint") { return; }

        Waypoint waypointColor = other.GetComponent<Waypoint>();
        if (waypointColor == null) { print(waypointColor + " NULL"); return; } 
        if (waypointColor.name == pathfinder.GetStartWaypoint().name) { return; }

        if (other.gameObject.tag == "Waypoint" && currentPathCount == 0)
            PreventDoubleTriggering(other, waypointColor);
    }

    private void PreventDoubleTriggering(Collider other, Waypoint waypointColor)
    {
        if (waypointColor.GetTopColor() != Color.black)
        {
            ContinueWithCurrentPath(other);
            return;
        }
        else
        {

            // TODO: FIND THE BUG
            //Quick fix for path error, to be fix properly
            if (SceneManager.GetActiveScene().name == "Level 5" ||
                SceneManager.GetActiveScene().name == "Level 6" ||
                SceneManager.GetActiveScene().name == "Level 7" ||
                SceneManager.GetActiveScene().name == "Level 8"  &&
                waypointColor.GetTopColor() == Color.black)
            {
                return;
            }
            else
            {
                ContinueWithCurrentPath(other);
            }
        }
    }

    private void ContinueWithCurrentPath(Collider other)
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].gameObject.name == other.gameObject.name)
            {
                currentPathCount = i;
                return;
            }
        }
    }

    private void CombineColorWithWaypoint(Collider other)
    {
        Color waypointColor = other.gameObject.GetComponent<Waypoint>().GetTopColor();

        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner == null) 
        { 
            print("Null error for enemySpawner"); 
            return; 
        }

        if (waypointColor == Color.red)
        {
            if(gameObject.name == "Enemy Blue(Clone)" || gameObject.name == "Enemy Yellow(Clone)")
                enemySpawner.SpawnPrimaryEnemy("Red", other.transform.position, transform.parent);
        }
        else if (waypointColor == Color.blue)
        {
            if (gameObject.name == "Enemy Red(Clone)" || gameObject.name == "Enemy Yellow(Clone)")
                enemySpawner.SpawnPrimaryEnemy("Blue", other.transform.position, transform.parent);
        }
        else if (waypointColor == Color.yellow)
        {
            if (gameObject.name == "Enemy Blue(Clone)" || gameObject.name == "Enemy Red(Clone)")
                enemySpawner.SpawnPrimaryEnemy("Yellow", other.transform.position, transform.parent);
        }
        else
        {
            // nothing happens
        }
    }

    private void CombineColor(Collision collision)
    {
        if (GetInstanceID() < collision.gameObject.GetInstanceID())
        {
            if(gameObject.name == collision.gameObject.name) { return; }

            GetCombinedEnemyColor(gameObject.name, collision.gameObject.name, collision.contacts[0].point);
            Destroy();
        }
        else
        {
            Destroy();
        }
    }

    private void GetCombinedEnemyColor(string color1, string color2, Vector3 collision)
    {
        string[] colorsUsed = { color1, color2 };
        string tempColor;

        // Get Purple
        if (colorsUsed.Contains("Enemy Red(Clone)") && colorsUsed.Contains("Enemy Blue(Clone)"))
        {
            tempColor = "Purple";
        }
        // Get Orange
        else if (colorsUsed.Contains("Enemy Red(Clone)") && colorsUsed.Contains("Enemy Yellow(Clone)"))
        {
            tempColor = "Orange";
        }
        // Get Green
        else if (colorsUsed.Contains("Enemy Yellow(Clone)") && colorsUsed.Contains("Enemy Blue(Clone)"))
        {
            tempColor = "Green";
        }
        else
        {
            tempColor = CheckForSecondayColor(colorsUsed);
        }

        EnemySpawner parentSpawner = GetComponentInParent<EnemySpawner>();
        parentSpawner.SpawnSecondaryEnemy(tempColor, collision, transform.parent);
    }

    private string CheckForSecondayColor(string[] colorsUsed)
    {
        string tempColor;
        if (colorsUsed.Contains("Enemy Purple(Clone)"))
        {
            tempColor = "Purple";
        }
        else if (colorsUsed.Contains("Enemy Orange(Clone)"))
        {
            tempColor = "Orange";
        }
        else if (colorsUsed.Contains("Enemy Green(Clone)"))
        {
            tempColor = "Green";
        }
        else
        {
            tempColor = null;
            print("No available combined color error.");
        }

        return tempColor;
    }

    private void CompareColor(Collision collision)
    {
        if(transform.GetChild(1).name == collision.transform.GetChild(1).name)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        GameObject vfx = Instantiate(deathVfxPrefab, new Vector3(transform.position.x, transform.position.y + 8f, transform.position.z), Quaternion.identity);
        Destroy(vfx, 2f);

        Destroy(gameObject);
        FindObjectOfType<AudioManager>().Play("DeathSplash");

        GameManager.instance.Invoke("CheckRoundClear", 0.25f);
    }

    private void EnemyMovement()
    {
        if (currentPathCount == path.Count) { return; }

        MoveAndLookAtDestination();
        CheckDistanceToDestination();
    }

    private void CheckDistanceToDestination()
    {
        if (Vector3.Distance(transform.position, path[currentPathCount].transform.position) <= 0.01f ||
                    transform.position == path[currentPathCount].transform.position)
        {
            currentPathCount++;
        }
    }

    private void MoveAndLookAtDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, path[currentPathCount].transform.position, Time.deltaTime * 6f);
        Vector3 target = new Vector3(path[currentPathCount].transform.position.x,
                                        transform.position.y,
                                        path[currentPathCount].transform.position.z);
        transform.LookAt(target);
    }
}
