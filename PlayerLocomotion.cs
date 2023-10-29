using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField]
    public float playerSpeed = 10f;
    public float playerIdleSpeed = 2f;
    public float playerDrag = 0.9f;
    public float playerShootCooldown = .5f;
    public float shootTimer = 0;
    public float areaThreshold = 17f;
    public float redirectTimer = .2f;
    public float redirectTimeDelta = 0;

    public bool is2D = true;

    Rigidbody rb;
    InputHandler inputHandler;

    public GameManager GameManager;
    public Transform bulletSpawnPosition;

    public AudioSource audioSource;
    public AudioClip[] shootingClipsArray;
    public AudioClip[] warpClipsArray;
    public Transform spotLight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        inputHandler = GetComponent<InputHandler>();
    }

    private void FixedUpdate()
    {
        float delta = Time.deltaTime;
        inputHandler.TickInput(delta);
        shootTimer += delta;
        redirectTimeDelta += delta;
        HandleWarp();
        HandleMovement();

        if (shootTimer > playerShootCooldown)
        {
            HandleShooting();
        }
        HandleQuit();
    }

    public void HandleWarp()
    {
        if (this.transform.position.x < -areaThreshold)
        {
            this.transform.position = new Vector3(areaThreshold, this.transform.position.y, this.transform.position.z);
            HandeWarpSound();
        }
        if (this.transform.position.x > areaThreshold)
        {
            this.transform.position = new Vector3(-areaThreshold, this.transform.position.y, this.transform.position.z);
            HandeWarpSound();
        }
    }

    public void HandleMovement()
    {
        if (is2D == true)
        {
            rb.velocity = new Vector3(inputHandler.horizontal, 0, 0) * playerSpeed;

            if (inputHandler.horizontal == 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                spotLight.rotation = Quaternion.Euler(0, 0, 0);
                rb.velocity = new Vector3(0, Mathf.Cos(Time.time * 3) / 5, 0) * playerIdleSpeed;
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, 0, this.transform.position.z), 4);
            }

            if (inputHandler.horizontal > 0)
            {
                this.transform.rotation = Quaternion.Euler(0, -45, 0);
                spotLight.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (inputHandler.horizontal < 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 45, 0);
                spotLight.rotation = Quaternion.Euler(0, 0, 0);

            }


            //if (this.transform.position.x < areaThreshold && this.transform.position.x > -areaThreshold && redirectTimer < redirectTimeDelta)
            //{
            //    rb.velocity = new Vector3(inputHandler.horizontal, 0, 0) * playerSpeed;
            //}
            //else if (this.transform.position.x > areaThreshold)
            //{
            //    rb.velocity = new Vector3(-1, 0, 0) * playerSpeed;
            //    redirectTimeDelta = 0;
            //}
            //else if (this.transform.position.x < -areaThreshold)
            //{
            //    rb.velocity = new Vector3(1, 0, 0) * playerSpeed;
            //    redirectTimeDelta = 0;
            //}
        }
        else
        {
            rb.AddForce(new Vector3(inputHandler.horizontal, inputHandler.vertical, 0));
        }
    }

    public void HandleQuit()
    {
        if (inputHandler.exitFlag)
        {
            Application.Quit();
        }
    }

    public void HandleShooting()
    {
        if (inputHandler.shootFlag)
        {
            shootTimer = 0;

            GameObject bulletPoolingGo = ObjectPool.SharedInstance.GetPooledBullets();
            if (bulletPoolingGo != null)
            {
                bulletPoolingGo.GetComponent<Rigidbody>().velocity = Vector3.zero;
                bulletPoolingGo.transform.position = bulletSpawnPosition.position;
                bulletPoolingGo.transform.rotation = Quaternion.Euler(90, 0, 0);
                //bulletPoolingGo.transform.rotation = bulletSpawnPosition.transform.rotation;
                bulletPoolingGo.SetActive(true);
                HandleBullet HandleCurrentBullet = bulletPoolingGo.GetComponent<HandleBullet>();
                HandleCurrentBullet.playerBullet = true;
                HandleShootingSound();
            }
        }
    }

    public void HandleShootingSound()
    {
        AudioClip RandomClip = shootingClipsArray[Random.Range(0, shootingClipsArray.Length)];
        audioSource.PlayOneShot(RandomClip);
    }

    public void HandeWarpSound()
    {
        AudioClip RandomClip = warpClipsArray[Random.Range(0, warpClipsArray.Length)];
        audioSource.PlayOneShot(RandomClip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("hit by Collision");
            other.gameObject.SetActive(false);

            GameManager.LoseLife();
        }
    }
}
