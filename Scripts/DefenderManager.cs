using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenderManager : MonoBehaviour
{
    [SerializeField] int energyCost = 10;
    [SerializeField] Text energyTxt;

    [Header("Add Defender Prefabs Here")]
    [SerializeField] GameObject defenderYellow;
    [SerializeField] GameObject defenderBlue, defenderRed;

    [Header("Add Secondary Defender Prefabs Here")]
    [SerializeField] GameObject defenderPurple;
    [SerializeField] GameObject defenderGreen, defenderOrange;

    [SerializeField] GameObject[] placeablesList;
    string selectedDefenderName;

    private void Start()
    {
        UpdateCostText();
    }

    public int GetEnergyCost()
    {
        return energyCost;
    }

    public void UpdateCostText()
    {
        energyTxt.text = "Cost: " + energyCost.ToString();
    }

    public void AddDefenderOrTiles(Waypoint waypoint)
    {
        GameObject defenderPrefab;

        switch (selectedDefenderName)
        {
            case "Blue Defender":
                defenderPrefab = defenderBlue;
                break;
            case "Red Defender":
                defenderPrefab = defenderRed;
                break;
            case "Yellow Defender":
                defenderPrefab = defenderYellow;
                break;
            case "Blue Tiles":
                waypoint.SetTopColor(Color.blue);
                defenderPrefab = null;
                break;
            case "Red Tiles":
                waypoint.SetTopColor(Color.red);
                defenderPrefab = null;
                break;
            case "Yellow Tiles":
                waypoint.SetTopColor(Color.yellow);
                defenderPrefab = null;
                break;
            default:
                defenderPrefab = null;
                break;
        }

        if(defenderPrefab == null) // Place Tiles
        {
            energyCost--;
            UpdateCostText();
            AudioManager.instance.Play("PlaceDefender");
            return; 
        }

        if(waypoint.GetTopColor() != Color.white) // Prevents placin defender on non placeable tiles
        {
            AudioManager.instance.Play("CantPlace");
            return;
        }

        AudioManager.instance.Play("PlaceDefender");
        energyCost--;
        UpdateCostText();

        GameObject defender =  Instantiate(defenderPrefab, waypoint.transform.position, Quaternion.identity);
        defender.transform.parent = gameObject.transform;
    }

    public void SelectDefender(string name)
    {
        AudioManager.instance.Play("Click");
        selectedDefenderName = name;

        for (int i = 0; i < placeablesList.Length; i++)
        {
            Color transparancy = placeablesList[i].GetComponent<Image>().color;

            if (placeablesList[i].name == selectedDefenderName)
            {
                transparancy.a = 1f;
            }
            else
            {
                transparancy.a = 0.2f;
            }
            placeablesList[i].GetComponent<Image>().color = transparancy;
        }
    }

    public void SpawnCombinedDefender(string color, Vector3 collisionPoint)
    {
        GameObject newDefender;

        switch (color)
        {
            case "Purple":
                newDefender = Instantiate(defenderPurple, collisionPoint, Quaternion.identity);
                break;
            case "Orange":
                newDefender = Instantiate(defenderOrange, collisionPoint, Quaternion.identity);
                break;
            case "Green":
                newDefender = Instantiate(defenderGreen, collisionPoint, Quaternion.identity);
                break;
            case "Red":
                newDefender = Instantiate(defenderRed, collisionPoint, Quaternion.identity);
                break;
            case "Blue":
                newDefender = Instantiate(defenderBlue, collisionPoint, Quaternion.identity);
                break;
            case "Yellow":
                newDefender = Instantiate(defenderYellow, collisionPoint, Quaternion.identity);
                break;
            default:
                newDefender = null;
                print("Spawn error, unavailable color");
                break;
        }

        newDefender.transform.parent = gameObject.transform;
    }
}
