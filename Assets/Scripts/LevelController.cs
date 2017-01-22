using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public WaveDataPack[] levelPacks;
    public List<Target> TargetList;
    public List<Piece> Pieces;
    private int currentLevel = 0;

    public delegate void GameFlowEvent();
    public static event GameFlowEvent OnNewLevel;

    private void Start()
    {
        GameController.OnWin += OnWin;
        GameController.OnLose += OnLose;
        LevelController.OnNewLevel += HOnNewLevel;
        SetLevel(0);
    }

    void HOnNewLevel()
    {
        levelWon = false;
        levelLost = false;
    }

    bool levelWon = false;
    void OnWin()
    {
        levelWon = true;
        levelLost = false;
    }

    bool levelLost = false;
    void OnLose()
    {
        levelLost = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetLevel(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetLevel(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetLevel(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetLevel(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SetLevel(5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SetLevel(6);

        if (levelWon)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AdvanceLevel();
            }
        }
        if (levelLost)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                RetryLevel();
            }
        }

    }

    public void AdvanceLevel()
    {
        SetLevel(currentLevel + 1);
    }

    public void RetryLevel()
    {
        SetLevel(currentLevel);
    }


    void SetLevel(int levelId)
    {
        this.currentLevel = levelId;
        GameController.Instance.gameWave.CreateFromWaveDatas(levelPacks[levelId].inputs);
        GameController.Instance.targetWave.CreateFromWaveData(levelPacks[levelId].target);

        if (OnNewLevel != null)
        {
            OnNewLevel();
        }

        // test: auto level generator
        /*var targetWaveData = WaveData.CreateInstance<WaveData>();
        int targetSize = 20;
        targetWaveData.values = new int[targetSize];    
        for (int i = 0; i < levelPacks[levelId].inputs.Length; i++)
        {
            int rndOffset = Random.Range(0, targetSize);
            for (int j = 0; j < levelPacks[levelId].inputs[i].values.Length; j++)
            {
                targetWaveData.values[(int)Mathf.Repeat(rndOffset + j, targetSize)] += -levelPacks[levelId].inputs[i].values[j];
            }
        }
        GameController.Instance.targetWave.CreateFromWaveData(targetWaveData);*/


    }

    



}
