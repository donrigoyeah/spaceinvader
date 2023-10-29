using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartObject : MonoBehaviour
{
    GameManager gameManager;
    public int spinSpeed = 85;

    private void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.Euler(45, Time.time * spinSpeed, 45);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            gameManager.StartGame();
            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
