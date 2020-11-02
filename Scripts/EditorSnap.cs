using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class EditorSnap : MonoBehaviour
{
    Waypoint waypoint;

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }

    void Update()
    {
        SnapToGrid();
        UpdateLabel();
    }

    private void SnapToGrid()
    {
        int gridSize = waypoint.GetGridSize();
        transform.position = new Vector3(
            waypoint.GetGridPos().x * gridSize,
            0f,
            waypoint.GetGridPos().y * gridSize
        );
    }

    private void UpdateLabel()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        string labelText = 
            waypoint.GetGridPos().x + 
            "," + 
            waypoint.GetGridPos().y;
        //textMesh.text = labelText; // TODO: comment out after design complete
        textMesh.text = null; 
        gameObject.name = labelText;
    }
}
