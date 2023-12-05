using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using System.IO;

public class GameManager : MonoBehaviour
{
    bool stopwatchActive = false;
    float currentTime;
    public Text currentTimeText;
    public int startMinutes;
    public float time2;
    bool isRunning = false;

    public GameObject[] menuPanels;
    public static GameStates currentGameState = GameStates.MainMenu;
    public GameObject Box;
    public static bool started = false;

    public GameObject Goal;
    public Text winner;

    public List<float> list;



    // Start is called before the first frame update


    public void Start()
    {
        if (started)
        {
            SetState(GameStates.Game);
        }
        else
        {
            SetState(GameStates.MainMenu);

        }

        started = true;

        Load();


        if (list.Count > 10)
        {
            list.Sort();
            list.Reverse();
            list.RemoveAt(list.Count - 1);

        }

    }
    private void Update()
    {

        if (isRunning && !Finish.victory)
        {
            if (stopwatchActive == true)
            {
                currentTime = currentTime + Time.deltaTime;
            }
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            currentTimeText.text = time.ToString(@"ss\:ff");
            time2 = MathF.Round(currentTime, 2);
        }

        if (Finish.victory)
        {
            if (stopwatchActive == true)
            {

                Debug.Log("Timer Finished");
                list.Add(time2);
                Save();
                stopwatchActive = false;

            }

            SetState(GameStates.Winner);

        }
    }


    public void RestGame()
    {
        Finish.victory = false;
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        SetState(GameStates.Game);
    }

    public void SetState(GameStates newState)
    {
        currentGameState = newState;
        for (int i = 0; i < menuPanels.Length; i++)
        {
            menuPanels[i].SetActive(false);
        }
        switch (currentGameState)
        {
            case GameStates.MainMenu:
                menuPanels[0].SetActive(true);
                break;
            case GameStates.Winner:
                menuPanels[1].SetActive(true);
                break;
            case GameStates.Game:
                menuPanels[2].SetActive(true);
                GameStart();
                break;
            default:
                currentGameState = GameStates.MainMenu;
                menuPanels[0].SetActive(true);
                break;
        }
    }

    public void GameStart()
    {
        Destroy(Box);
        isRunning = true;
        stopwatchActive = true;

    }
    private void Save()
    {

        TrimListToMaxSize();

        string path = Application.dataPath + "/Highscore.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "FileCreated");
        }

        string content = string.Join(",", list);
        File.WriteAllText(path, content);
        Debug.Log("Save successful");
    }
    private void Load()
    {
        string path = Application.dataPath + "/Highscore.txt";
        if (!File.Exists(path))
        {
            Debug.Log("File not found");
            return;
        }

        string content = File.ReadAllText(path);

        string[] floatStrings = content.Split(',');
        list = new List<float>();

        foreach (string floatString in floatStrings)
        {
            float floatValue;
            if (float.TryParse(floatString, out floatValue))
            {
                list.Add(floatValue);
            }
            else
            {
                Debug.LogError("Failed to parse float value: " + floatString);
            }
        }

        Debug.Log("Load successful");

    }

    private void TrimListToMaxSize()
    {
        if (list.Count > 10)
        {
            list.Sort();

            while (list.Count > 10)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

    }
}
public enum GameStates
{
    MainMenu,
    Game,
    Winner
}

/*
 
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public List<int> list = new List<int> { 0,0,0,0,0,0,0,0,0,0};
    int score = 100;
    // Start is called before the first frame update
    void Start()
    {
        list.Add(score);
        list.Sort();
        if(list.Count > 10)
        {
            list.RemoveAt(list.Count-1);
        }
        //save
    }
}

*/ 