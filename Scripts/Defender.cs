using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] GameObject deathVfxPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                CompareColor(collision);
                break;
            case "Defender":
                CombineColor(collision);
                break;
            case "PlayerHealth":
                break;
            default:
                break;
        }        
    }

    private void CombineColor(Collision collision)
    {
        if (GetInstanceID() < collision.gameObject.GetInstanceID())
        {
            Destroy();
            GetCombinedEnemyColor(gameObject.name, collision.gameObject.name, collision.gameObject.transform.position);
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
        if (colorsUsed.Contains("Defender Red(Clone)") && colorsUsed.Contains("Defender Blue(Clone)"))
        {
            tempColor = "Purple";
        }
        // Get Orange
        else if (colorsUsed.Contains("Defender Red(Clone)") && colorsUsed.Contains("Defender Yellow(Clone)"))
        {
            tempColor = "Orange";
        }
        // Get Green
        else if (colorsUsed.Contains("Defender Yellow(Clone)") && colorsUsed.Contains("Defender Blue(Clone)"))
        {
            tempColor = "Green";
        }
        else
        {
            tempColor = CheckForPrimaryColor(colorsUsed);
        }

        DefenderManager defenderManager = GetComponentInParent<DefenderManager>();
        defenderManager.SpawnCombinedDefender(tempColor, collision);
    }

    private string CheckForPrimaryColor(string[] colorsUsed)
    {
        string tempColor;
        if (colorsUsed.Contains("Defender Red(Clone)"))
        {
            tempColor = "Red";
        }
        else if (colorsUsed.Contains("Defender Yellow(Clone)"))
        {
            tempColor = "Yellow";
        }
        else if (colorsUsed.Contains("Defender Blue(Clone)"))
        {
            tempColor = "Blue";
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
        if (transform.GetChild(1).name != collision.transform.GetChild(1).name)
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
    }
}
