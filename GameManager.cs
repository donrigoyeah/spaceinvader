using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public bool isPlaying = false;
    public int enemyColumn = 7;
    public int enemyRows = 3;
    public int enemyKillCount = 0;
    public int waveCleared = 0;

    public GameObject enemyContainer;
    EnemyManager EnemyManager;
    public GameObject StartGameObject;
    public Transform startGameObjectPosition;
    public GameObject Player;
    public int playerLifes;
    public int playerScore;
    public int highScore = 0;
    public GameObject BackgroundClose;
    public GameObject BackgroundMiddle;
    public GameObject BackgroundFar;
    public float backGroundSmoothDamp = 2;

    public TextMeshProUGUI playerScoreUI;
    public TextMeshProUGUI playerLifeUI;
    public TextMeshProUGUI HighScoreUI;

    public AudioSource audioSource;
    public AudioClip[] explosionClipArray;

    private Vector3 refVec = Vector3.zero;

    private void Start()
    {
        EnemyManager = enemyContainer.GetComponent<EnemyManager>();
        audioSource = GetComponent<AudioSource>();
        Instantiate(StartGameObject, startGameObjectPosition);
    }
    private void FixedUpdate()
    {
        CheckForEnemies();
        HandleBackgroundMovement();
    }


    public void StartGame()
    {
        playerLifes = 3;
        playerScore = 0;
        playerLifeUI.text = "Lives: " + playerLifes;
        playerScoreUI.text = "Score: " + playerScore;
        EnemyManager.SpawnEnemy(enemyColumn, enemyRows);
        isPlaying = true;
    }

    public void HandleBackgroundMovement()
    {
        if (isPlaying)
        {
            BackgroundClose.transform.position = Vector3.MoveTowards(BackgroundClose.transform.position, -Player.transform.position, 5);
            //BackgroundClose.transform.position = Vector3.SmoothDamp(BackgroundClose.transform.position, -Player.transform.position, ref refVec, backGroundSmoothDamp);
            BackgroundMiddle.transform.position = Vector3.MoveTowards(BackgroundMiddle.transform.position, -Player.transform.position / 2, 5);
            //BackgroundMiddle.transform.position = Vector3.SmoothDamp(BackgroundMiddle.transform.position, -Player.transform.position / 2, ref refVec, backGroundSmoothDamp);
            BackgroundFar.transform.position = Vector3.MoveTowards(BackgroundFar.transform.position, -Player.transform.position / 4, 5);
            //BackgroundFar.transform.position = Vector3.SmoothDamp(BackgroundFar.transform.position, -Player.transform.position / 4, ref refVec, backGroundSmoothDamp);
        }
    }

    private void CheckForEnemies()
    {
        if (enemyKillCount >= enemyRows * enemyColumn)
        {
            EnemyManager.transform.position = new Vector3(0, 14, 0);
            EnemyManager.SpawnEnemy(enemyColumn, enemyRows);
            enemyKillCount = 0;
            waveCleared++;
        }
    }

    public void GainPoints(int points)
    {
        playerScore += points;
        playerScoreUI.text = "Score: " + playerScore;
    }

    public void PlayExplosionSound()
    {
        AudioClip RandomClip = explosionClipArray[Random.Range(0, explosionClipArray.Length)];
        audioSource.PlayOneShot(RandomClip);
    }


    public void LoseLife()
    {
        ParticleSystem explosionPoolingPS = ObjectPool.SharedInstance.GetPooledExplosions();
        if (explosionPoolingPS != null)
        {
            explosionPoolingPS.transform.position = Player.transform.position;
            explosionPoolingPS.Play();
        }

        playerLifes--;
        playerLifeUI.text = "Lives: " + playerLifes;

        if (playerLifes < 1)
        {
            PlayerDeath();
        }
    }

    public void GainLife()
    {
        playerLifes++;
        playerLifeUI.text = "Lives: " + playerLifes;
    }

    public void PlayerDeath()
    {
        if (playerScore > highScore) { highScore = playerScore; }
        if (highScore > 0) { HighScoreUI.text = "HighScore: " + highScore; }

        ParticleSystem explosionPoolingPS = ObjectPool.SharedInstance.GetPooledExplosions();
        if (explosionPoolingPS != null)
        {
            explosionPoolingPS.transform.position = Player.transform.position;
            explosionPoolingPS.Play();
        }

        ObjectPool.SharedInstance.KillAllEnemies();
        // TODO: Make variable for repositinng
        EnemyManager.transform.position = new Vector3(0, 14, 0);
        isPlaying = false;
        waveCleared = 0;
        enemyKillCount = 0;

        StartCoroutine(PlayerDeathAndRespawn());

        Instantiate(StartGameObject, startGameObjectPosition);
    }

    IEnumerator PlayerDeathAndRespawn()
    {
        Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Player.SetActive(false);
        yield return new WaitForSeconds(2);
        Player.SetActive(true);
    }
}
