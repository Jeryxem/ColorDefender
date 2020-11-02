using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartLevel : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.RestartLevel();
    }
}
