using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool ignoreNextCollision;
    public Rigidbody rb;
    public float impulseForce = 5f;
    private Vector3 startPos;
    public int perfectPass = 0;
    public bool isSuperSpeedActive;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (perfectPass >= 3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision)
        {
            return;
        }

        if (isSuperSpeedActive)
        {
            if (!collision.transform.GetComponent<Goal>())            
            {
                Destroy(collision.transform.parent.gameObject);
            }
        }
        else if(!isSuperSpeedActive)                  //superspeed, carptigi gameoverpart olsa bile yok eder                
        {
            GameOverPart gameOverPart = collision.transform.GetComponent<GameOverPart>();
            if (gameOverPart)
            {
                gameOverPart.HitGameOverPart();
            }
        }

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        ignoreNextCollision = true;
        Invoke("AllowCollision", .2f);

        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }
}
