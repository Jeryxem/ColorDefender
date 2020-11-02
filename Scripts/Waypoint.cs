using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] MeshRenderer topMeshRenderer;

    public bool isExplored = false;
    public Waypoint exploredFrom;
    public bool isPlaceable = true;
    public bool setYellow = false;

    const int gridSize = 10;

    private void Start()
    {
        topMeshRenderer = transform.Find("Top").GetComponent<MeshRenderer>();

        // Added due to color code bug
        if (setYellow)
        {
            SetTopColor(Color.yellow);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DefenderManager defenderManager = FindObjectOfType<DefenderManager>();
            if (isPlaceable)
            {
                if (defenderManager.GetEnergyCost() <= 0)
                {
                    AudioManager.instance.Play("CantPlace");
                    GameObject costText = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
                    costText.GetComponent<TweenManager>().ScaleUp(costText);
                }
                else
                {
                    defenderManager.AddDefenderOrTiles(this);
                }
            }
            else
            {
                AudioManager.instance.Play("CantPlace");
            }
        }
    }

    public int GetGridSize()
    {
        return gridSize;
    }

    public Vector2Int GetGridPos()
    {
        return new Vector2Int(
            Mathf.RoundToInt(transform.position.x / gridSize),
            Mathf.RoundToInt(transform.position.z / gridSize)
        );
    }

    public Color GetTopColor()
    {
        return topMeshRenderer.material.color;
    }

    public void SetTopColor(Color color)
    {
        topMeshRenderer.material.color = color;
    }
}
