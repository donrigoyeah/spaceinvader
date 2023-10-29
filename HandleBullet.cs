using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HandleBullet : MonoBehaviour
{
    private float timer;
    private float lifeTime = 5;

    public float deathZone = 20f;
    public bool playerBullet;

    Rigidbody rb;
    public float bulletSpeed = 10f;

    public Material enemyMat;
    public Material playerMat;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }


    public void FixedUpdate()
    {
        // Handle Direction
        if (playerBullet == true)
        {
            rb.velocity = new Vector3(0, 1, 0) * bulletSpeed;
            GetComponent<Renderer>().material = playerMat;
        }
        else
        {
            rb.velocity = new Vector3(0, -1, 0) * bulletSpeed;
            GetComponent<Renderer>().material = enemyMat;
        }

        // Handle Lifespan
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
            timer = 0;
        }
    }
}


