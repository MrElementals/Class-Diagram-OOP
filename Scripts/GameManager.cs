using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
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

    // Start is called before the first frame update
    private void Start()
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

        HighScoreTable.Instance.Load();

        if (HighScoreTable.Instance.Count > 10)
        {
            HighScoreTable.Instance.TrimListToMaxSize();
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
                HighScoreTable.Instance.AddScore(time2);
                HighScoreTable.Instance.Save();
                stopwatchActive = false;
            }

            SetState(GameStates.Winner);
        }
    }

    public void RestGame()
    {
        Finish.victory = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
}

public enum GameStates
{
    MainMenu,
    Game,
    Winner
}

public class HighScoreTable : IScoreSaveLoad
{
    private static HighScoreTable _instance;

    public static HighScoreTable Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new HighScoreTable();
            }
            return _instance;
        }
    }

    private List<float> list;

    private HighScoreTable(int maxSize)
    {
        list = new List<float>(maxSize);
    }

    public HighScoreTable() : this(10)
    {
    }

    public void Save()
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

    public void Load()
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

    public void AddScore(float score)
    {
        list.Add(score);
    }

    public void TrimListToMaxSize()
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

    public int Count
    {
        get { return list.Count; }
    }
}

public interface IScoreSaveLoad
{
    void Save();
    void Load();
}