using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;

    public Text scoreText;
    //public Text restartText;
    public GameObject restartButton;
    public Text gameOverText;

    private bool gameOver;
    private bool restart = false;

    private int score;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    private void Start()
    {
        gameOver = false;
        restart = false;
        restartButton.SetActive(false);
        //restartText.text = "";
        gameOverText.text = "";

        score = 0;
        UpdateScore();
        StartCoroutine (SpawnWaves());
    }

    //private void Update()
    //{
    //    // !!!! THIS IS HOW YOU LOAD A NEW SCENE !!!!
    //    if (restart)
    //        if (restartTouchpad.GetTouchStatus() == true)
    //        {
    //            SceneManager.LoadScene("Main");
    //        }
    //}

    IEnumerator SpawnWaves()
    {
        //short pause after game starts
        yield return new WaitForSeconds(startWait);

        while (gameOver == false)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                if (gameOver)
                    break;

                GameObject hazard = hazards[UnityEngine.Random.Range(0,hazards.Length)];

                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range
                    (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    void UpdateScore()
    {
        scoreText.text = $"Score: {score}";
    }

    public void AddScore(int scoredPoints)
    {
        score += scoredPoints;
        UpdateScore();
    }

    public async void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
        await Task.Delay(TimeSpan.FromSeconds(2));
        restartButton.SetActive(true);
    }

    //called inside button
    public void Restart()
    {
        if (gameOver == true)
        {
            restart = true;
            //restartText.text = "Press \"R\" for restart";
            SceneManager.LoadScene("Main");
        }
    }
}
