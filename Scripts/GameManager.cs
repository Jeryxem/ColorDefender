using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool canRestart = false;
    public bool canProceed = false;
    private int sceneBuildIndex = 0;
    public int enemyCount = 0;

    [Header("UI Tweening")]
    public GameObject gameTitle, playButton;
    public GameObject tryAgainPanel, stageClearPanel;
    public GameObject levelName;
    public GameObject backgroundMovingPlane;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (canRestart)
        {
            Restart();
        }

        if (canProceed)
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canProceed = false;
            sceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
            stageClearPanel = GameObject.Find("Canvas").transform.GetChild(4).gameObject;

            ResumeGame();
            FindObjectOfType<TweenManager>().MoveOutToRight(stageClearPanel);
        }
    }

    private void Restart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            canRestart = false;
            sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            tryAgainPanel = GameObject.Find("Canvas").transform.GetChild(3).gameObject;

            ResumeGame();
            FindObjectOfType<TweenManager>().MoveOutToRight(tryAgainPanel);
        }
    }

    public void RestartLevel()
    {
        enemyCount = 0;
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public void LoadLoadingScene()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void CheckRoundClear()
    {
        if(enemyCount == 0)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            if(enemies == null || enemies.Length == 0)
            {
                stageClearPanel = GameObject.Find("Canvas").transform.GetChild(4).gameObject;
                FindObjectOfType<TweenManager>().MoveInFromLeft(stageClearPanel);
                canProceed = true;
                AudioManager.instance.Play("Win");
            }
        }
    }

    public void StartGameAnimation()
    {
        gameTitle = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        FindObjectOfType<TweenManager>().MoveOutToTop(gameTitle);

        playButton = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        FindObjectOfType<TweenManager>().MoveOutToBottom(playButton);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void RemoveLevelNameOnGameStart()
    {
        levelName = GameObject.Find("Canvas").transform.GetChild(5).gameObject;
        FindObjectOfType<TweenManager>().MoveOutToTop(levelName);
    }

    public void MoveBackground()
    {
        GameObject bg = GameObject.FindGameObjectWithTag("Bg");
        if (bg == null)
            return;

        bg.GetComponent<TweenManager>().BackgroundMoveLeft();
    }
}
