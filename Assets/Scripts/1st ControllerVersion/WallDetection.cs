using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pared"))
        {
            playerMovement.OnWallEnter();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pared"))
        {
            playerMovement.OnWallExit();
        }
    }
}
