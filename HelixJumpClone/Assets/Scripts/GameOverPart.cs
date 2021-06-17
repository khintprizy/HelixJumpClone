using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPart : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void HitGameOverPart()
    {
        GameManager.singleton.RestartLevel();
    }
}
