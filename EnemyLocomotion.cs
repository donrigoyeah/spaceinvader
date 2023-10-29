using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyLocomotion : MonoBehaviour
{
    bool canShoot = true;
    float shootTimer = 0;
    float enemyShootCountdown = 2f;

    public Transform bulletSpawnPosition;
    public GameObject Bullet;

    public GameManager GameManager;
    public AudioSource audioSource;
    public AudioClip[] shootingClipsArray;

    private void Awake()
    {
        GameManager = FindAnyObjectByType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        HandleCanShoot();
        if (canShoot)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer > enemyShootCountdown)
            {
                enemyShootCountdown = Random.Range(10, 30) / 10;

                int fiftyfifty = Random.Range(0, 2);
                if (fiftyfifty == 1) { HandleShooting(); }
                shootTimer = 0;
            }
        }
    }

    public void HandleShootingSound()
    {
        AudioClip RandomClip = shootingClipsArray[Random.Range(0, shootingClipsArray.Length)];
        audioSource.PlayOneShot(RandomClip);
    }

    private void HandleCanShoot()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, 2))
        {
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }
    }

    public void HandleShooting()
    {
        GameObject bulletPoolingGo = ObjectPool.SharedInstance.GetPooledBullets();
        if (bulletPoolingGo != null)
        {
            HandleShootingSound();
            bulletPoolingGo.GetComponent<Rigidbody>().velocity = Vector3.zero;
            HandleBullet HandleCurrentBullet = bulletPoolingGo.GetComponent<HandleBullet>();
            HandleCurrentBullet.playerBullet = false;
            bulletPoolingGo.transform.position = bulletSpawnPosition.position;
            bulletPoolingGo.transform.rotation = Quaternion.Euler(90, 0, 0);
            bulletPoolingGo.SetActive(true);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameManager.enemyKillCount++;
            GameManager.GainPoints(100);
            GameManager.PlayExplosionSound();

            ParticleSystem explosionPoolingPS = ObjectPool.SharedInstance.GetPooledExplosions();
            if (explosionPoolingPS != null)
            {
                explosionPoolingPS.transform.position = this.transform.position;
                explosionPoolingPS.Play();
            }

            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

        }

    }
}
