﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.RenderGraphModule;


public class ManagerOfScenes : MonoBehaviour
{
    [SerializeField] float darkener = 0.8f;
    public static bool GameIsPaused;
    bool startFade = false;
    public GameObject nextLevelUi;
    public GameObject pauseMenuUi;
    public GameManager gameManager;
    public GameObject restartButton, nextLevelButton;
    public GameObject lightPort;
    public GameObject darkPort;
    public GameObject fader;
    public CinemachineVirtualCamera fullView;
    float darkness = 0f;

    private int imageOfRank;
    private int currentSceneIndex;
    public GameObject Bronze;
    public GameObject Silver;
    public GameObject Gold;


    private void Start()
    {
        Resume();
        gameManager = FindObjectOfType<GameManager>();
        fader.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        startFade = false;
    }

    void Update()
    {
        if (gameManager.levelPassed)
        {
            StartCoroutine(ExampleCoroutine());
            if (startFade)
            {
                darkness += Time.deltaTime * darkener;
                Debug.Log(darkness);
                fader.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, darkness);

            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void Resume()
    {
        nextLevelUi.SetActive(false);
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Bronze.SetActive(false);
        Silver.SetActive(false);
        Gold.SetActive(false);

    }
    public void PassPause()
    {
        lightPort.GetComponent<LightTeleport>().startFading = false;
        darkPort.GetComponent<DarkTeleport>().startFading = false;
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        nextLevelUi.SetActive(true);
        levelRankSprite();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(nextLevelButton);




    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        pauseMenuUi.SetActive(true);
        levelRankSprite();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restartButton);

    }

    public void Restart()
    {
        fader.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        Resume();
    }
    public void NextLevel()
    {
        Resume();
        Time.timeScale = 1f;
        fader.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ChooseLevel()
    {

        SceneManager.LoadScene("Levels");
        Cursor.visible = true;
        Resume();
    }


    private IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        fullView.GetComponent<CinemachineVirtualCamera>().Priority = 20;
        yield return new WaitForSeconds(0.4f);
        darkPort.GetComponent<DarkTeleport>().DarkFade();
        lightPort.GetComponent<LightTeleport>().LightFade();
        yield return new WaitForSecondsRealtime(0.7f);
        startFade = true;
        yield return new WaitForSecondsRealtime(1.3f);
        PassPause();
    }


    public void levelRankSprite()
    {
        imageOfRank = 0;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //checks what is the best rank so far of this level

        if (PlayerPrefs.GetInt("Level " + currentSceneIndex + " completion", 0) == 1)
        {
            imageOfRank = 2;
        }
        if (PlayerPrefs.GetInt("Level " + currentSceneIndex + " notBestNumOfSwitches", 0) == 1)
        {
            imageOfRank = 3;
        }
        if (PlayerPrefs.GetInt("Level " + currentSceneIndex + " bestNumOfSwitches", 0) == 1)
        {
            imageOfRank = 4;
        }

        //according to the imageOfRank var we can select the sprite that represent the rank of this level
        if (imageOfRank == 2)
        {
            Bronze.SetActive(true);
        }
        else if (imageOfRank == 3)
        {
            Silver.SetActive(true);
        }
        else if (imageOfRank == 4)
        {
            Gold.SetActive(true);

        }

    }
}
