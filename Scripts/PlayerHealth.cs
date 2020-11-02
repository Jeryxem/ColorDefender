using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    int playerHealth;
    [SerializeField] Text playerHealthTxt;

    private void Start()
    {
        playerHealth = 1;
        UpdatePlayerHealthText();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                playerHealth--;
                UpdatePlayerHealthText();
                CheckRoundLose();
                break;
            case "Defender":
                break;
            case "PlayerHealth":
                break;
            default:
                break;
        }
    }

    private void UpdatePlayerHealthText()
    {
        playerHealthTxt.text = "HP: " + playerHealth.ToString();
    }

    private void CheckRoundLose()
    {
        if (playerHealth <= 0)
        {
            // MoveUI
            GameManager.instance.tryAgainPanel = GameObject.Find("Canvas").transform.GetChild(3).gameObject;
            FindObjectOfType<TweenManager>().MoveInFromLeft(GameManager.instance.tryAgainPanel);
            GameManager.instance.canRestart = true;
        }
    }
}
