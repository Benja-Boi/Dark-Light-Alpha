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
    public static bool GameIsPaused;
    public GameObject nextLevelUi;
    public GameObject pauseMenuUi;
    public GameManager gameManager;
    public GameObject restartButton, nextLevelButton;
    public GameObject lightPort;
    public GameObject darkPort;
    public CinemachineVirtualCamera fullView;

    private void Start()
    {
        Resume();
        gameManager = FindObjectOfType<GameManager>();
    }

    


    void Update()
    {
        if (gameManager.levelPassed)
        {
            StartCoroutine(ExampleCoroutine());
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
    }
    public void PassPause()
    {
        lightPort.GetComponent<LightTeleport>().startFading = false;
        darkPort.GetComponent<DarkTeleport>().startFading = false;
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        nextLevelUi.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(nextLevelButton);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        pauseMenuUi.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restartButton);
    }

    public void Restart()
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ChooseLevel()
    {
        
        SceneManager.LoadScene("Levels");
        Cursor.visible = true;
        Resume();
    }
    public void LevelRestrictions()
    {

    }

    private IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        fullView.GetComponent<CinemachineVirtualCamera>().Priority = 20;
        yield return new WaitForSeconds(0.4f);
        darkPort.GetComponent<DarkTeleport>().DarkFade();
        lightPort.GetComponent<LightTeleport>().LightFade();
        yield return new WaitForSeconds(0.4f);
        gameManager.GetComponent<ColorAdjustments>().postExposure.value = 0f;
        yield return new WaitForSecondsRealtime(2f);
        PassPause();
    }
}
