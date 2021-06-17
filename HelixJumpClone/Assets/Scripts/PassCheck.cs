using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCheck : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameManager.singleton.AddScore(10);
        transform.gameObject.GetComponent<Collider>().enabled = false;

        FindObjectOfType<BallController>().perfectPass++;
    }
}
