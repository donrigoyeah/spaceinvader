using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    [Header("Enemies")]
    public int enemyAmount;
    public List<GameObject> EnemyPool;
    public GameObject Enemy;
    public GameObject EnemyContainer;

    [Header("Bullets")]
    public int bulletAmount;
    public List<GameObject> BulletPool;
    public GameObject Bullet;
    public GameObject BulletContainer;

    [Header("Explosions")]
    public int explosionAmunt;
    public List<ParticleSystem> ExplosionPool;
    public ParticleSystem Explosion;
    public GameObject ExplosionsContainer;


    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        EnemyPooling();
        BulletPooling();
        ExplosionPooling();
    }

    public GameObject GetPooledEnemies()
    {
        for (int i = 0; i < enemyAmount; i++)
        {
            if (!EnemyPool[i].activeInHierarchy)
            {
                return EnemyPool[i];
            }
        }
        return null;
    }

    public GameObject GetPooledBullets()
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            if (!BulletPool[i].activeInHierarchy)
            {
                return BulletPool[i];
            }
        }
        return null;
    }

    public ParticleSystem GetPooledExplosions()
    {
        for (int i = 0; i < explosionAmunt; i++)
        {
            if (!ExplosionPool[i].isPlaying)
            {
                return ExplosionPool[i];
            }
        }
        return null;
    }

    private void EnemyPooling()
    {
        EnemyPool = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < enemyAmount; i++)
        {
            tmp = Instantiate(Enemy);
            tmp.transform.SetParent(EnemyContainer.transform);
            tmp.SetActive(false);
            EnemyPool.Add(tmp);
        }
    }

    public void KillAllEnemies()
    {
        for (int i = 0; i < enemyAmount; i++)
        {
            EnemyPool[i].SetActive(false);
        }
        for (int i = 0; i < bulletAmount; i++)
        {
            BulletPool[i].SetActive(false);
        }

    }
    private void BulletPooling()
    {
        BulletPool = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < bulletAmount; i++)
        {
            tmp = Instantiate(Bullet);
            tmp.transform.SetParent(BulletContainer.transform);
            tmp.SetActive(false);
            BulletPool.Add(tmp);
        }
    }

    private void ExplosionPooling()
    {
        ExplosionPool = new List<ParticleSystem>();
        ParticleSystem tmp;
        for (int i = 0; i < explosionAmunt; i++)
        {
            tmp = Instantiate(Explosion);
            tmp.transform.SetParent(ExplosionsContainer.transform);
            //tmp.SetActive(false);
            //tmp.SetActive(false);
            ExplosionPool.Add(tmp);
        }
    }
}
