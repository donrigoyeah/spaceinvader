using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyManager : MonoBehaviour
{
    // Enemy Stats
    float leftPosition = -10f;
    float rightPosition = 10f;
    public float topPosition = 12f;
    public float deathPosition = 2f;

    public float enemySpeed = 1f;
    public bool moveLeft;
    private int enemyMoveCounter = 0;
    private bool hasMoved;
    public GameObject Enemy;
    public GameManager GameManagerRef;
    private Vector3 refVec = Vector3.zero;


    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (GameManagerRef.isPlaying)
        {
            HandleMovement();
            HandleMoveForward();
        }
    }

    private void HandleMovement()
    {
        Vector3 left = new Vector3(leftPosition, topPosition, 0);
        Vector3 right = new Vector3(rightPosition, topPosition, 0);

        float movementThisFrame = enemySpeed * Time.deltaTime;

        if (this.transform.position.x >= rightPosition) { moveLeft = true; enemyMoveCounter++; hasMoved = false; }
        if (this.transform.position.x < leftPosition) { moveLeft = false; enemyMoveCounter++; hasMoved = false; }

        if (moveLeft)
        {
            rb.velocity = new Vector3(-1, 0, 0) * enemySpeed;
        }
        else
        {
            rb.velocity = new Vector3(1, 0, 0) * enemySpeed;
        }
    }

    private void HandleMoveForward()
    {
        if (enemyMoveCounter % 2 == 0 && hasMoved == false)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position - Vector3.up, 2f);
            hasMoved = true;
        }
        if (this.transform.position.y == deathPosition)
        {
            GameManagerRef.PlayerDeath();
        }
    }

    public void SpawnEnemy(int enemyColumn, int enemyRows)
    {
        for (int i = 1; i < enemyRows + 1; i++)
        {
            for (int j = 0; j < enemyColumn; j++)
            {
                StartCoroutine(DelaySpawnEnemy(i, j));

            }
        }
    }


    IEnumerator DelaySpawnEnemy(int row, int column)
    {
        yield return new WaitForSeconds(0);
        //yield return new WaitForSeconds(row + column / 15);
        GameObject enemyPoolGo = ObjectPool.SharedInstance.GetPooledEnemies();
        if (enemyPoolGo != null)
        {
            Debug.Log("Instantiate Enemy");
            enemyPoolGo.SetActive(true);
            enemyPoolGo.transform.position = new Vector3(column * 2, topPosition + row * 2, 0);
            //enemyPoolGo.transform.position = Vector3.Lerp(Vector3.up * 20, new Vector3(count * 2, topPosition + row * 2, 0), 2);

            //enemyPoolGo.transform.position = Vector3.SmoothDamp(Vector3.up * 20, new Vector3(count * 2, topPosition + row * 2, 0), ref refVec, 2);
            enemyPoolGo.transform.SetParent(this.transform);
        }
    }
}
