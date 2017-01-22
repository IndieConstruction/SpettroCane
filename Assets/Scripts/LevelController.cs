using System;
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

    public Transform showWindowBackgroundTr;


    public static void ResetStatic()
    {
        OnNewLevel = null;
    }

    private void Start()
    {
        GameController.OnWin += OnWin;
        GameController.OnLose += OnLose;
        LevelController.OnNewLevel += HOnNewLevel;
        SetLevel(1);
    }

    void HOnNewLevel()
    {
        levelWon = false;
        levelLost = false;

        GameController.Instance.sm.PlayClip(SoundManager.Clip.Main);
    }

    bool levelWon = false;

    void OnWin()
    {
        levelWon = true;
        levelLost = false;

        GameController.Instance.sm.PlayClip(SoundManager.Clip.WinJingle);
    }

    bool levelLost = false;

    void OnLose()
    {
        levelLost = true;

        GameController.Instance.sm.PlayClip(SoundManager.Clip.LoseJingle);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetLevel(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetLevel(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetLevel(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetLevel(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SetLevel(5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SetLevel(6);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SetLevel(7);
        if (Input.GetKeyDown(KeyCode.Alpha8)) SetLevel(8);
        if (Input.GetKeyDown(KeyCode.Alpha9)) SetLevel(9);
        if (Input.GetKeyDown(KeyCode.Alpha0)) SetLevel(10);

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


    public WaveData[] dataForRandom;

    void SetLevel(int levelId)
    {
        this.currentLevel = levelId;
        //GameController.Instance.gameWave.CreateFromWaveDatas(levelPacks[levelId].inputs);
        //GameController.Instance.targetWave.CreateFromWaveData(levelPacks[levelId].target);

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


        // ******************************
        // New Level Type Generation

        if (levelId >= 8)
        {
            // RANDOM GENERATION

            int nData = UnityEngine.Random.Range(2, 2);

            int targetSize = 8;
            var targetWaveData = WaveData.CreateInstance<WaveData>();
            WaveData[] usedDatas = new WaveData[nData];
            targetWaveData.values = new int[targetSize];
            string s;
            for (int i = 0; i < nData; i++)
            {
                var usedInput = dataForRandom[UnityEngine.Random.Range(0, dataForRandom.Length)];
                for (int j = 0; j < usedInput.values.Length; j++)
                {
                    int rndOffset = UnityEngine.Random.Range(0,targetSize);
                    targetWaveData.values[(int) Mathf.Repeat(rndOffset + j, targetSize)] += -usedInput.values[j];
                }
                Debug.Log("USING INPUT OF LENGTH " + usedInput.values.Length);
                s = "";

                usedDatas[i] = usedInput;
                for (int j = 0; j < usedDatas[i].values.Length; j++)
                {
                    s += usedDatas[i].values[j] + " ";
                }
                Debug.Log(s);
            }

            s = "TARGET: ";
            for (int j = 0; j < targetWaveData.values.Length; j++)
            {
                s += targetWaveData.values[j] + " ";
            }
            Debug.Log(s);

            GameController.Instance.gameWave.playWindowSize = targetSize;
            GameController.Instance.targetWave.playWindowSize = targetSize;
            GameController.Instance.targetWave.CreateFromWaveData(targetWaveData, 0);
            GameController.Instance.gameWave.CreateFromWaveDatas(usedDatas, 2);
        }
        else
        {
            if (GameController.Instance.EnableNewLevelType)
            {
                // Create Target
                // GameController.Instance.gameWave.CreateFromWaveDatas(WaveDataProxy("1"));
                WaveData targetWave = WaveDataTargetProxy(levelId.ToString());
                if (targetWave != null)
                {
                    int window = GetWindow(levelId.ToString());
                    GameController.Instance.gameWave.playWindowSize = window;
                    GameController.Instance.targetWave.playWindowSize = window;

                    int target_span = 0;    // automatic addition anyway
                    GameController.Instance.targetWave.CreateFromWaveData(targetWave, target_span);

                    int span = GetSpan(levelId.ToString());
                    WaveData[] gameWaves = WaveDataInputsProxy(levelId.ToString());
                    GameController.Instance.gameWave.CreateFromWaveDatas(gameWaves, span);

                    float windowBoxSize = 1;
                    switch (window)
                    {
                        case 6:
                            windowBoxSize = 20;
                            break;
                        case 8:
                            windowBoxSize = 28;
                            break;
                        case 10:
                            windowBoxSize = 35;
                            break;
                    }

                    var tmpScale = showWindowBackgroundTr.transform.localScale;
                    tmpScale.x = windowBoxSize;
                    showWindowBackgroundTr.transform.localScale = tmpScale;
                }
            }

        }

        // ******************************

        if (OnNewLevel != null)
        {
            OnNewLevel();
        }

    }

    #region New static level data

    /// <summary>
    /// Return Wave from Target ID
    /// </summary>
    /// <param name="_targetId">The target identifier.</param>
    /// <returns></returns>
    public WaveData[] WaveDataInputsProxy(string _targetId)
    {
        List<WaveData> dataList = new List<WaveData>();

        // Find the target by ID
        List<Target> targets = GameController.DataLevel.Targets.FindAll(t => t.ID == _targetId && t.TargetLink != "");
        foreach (var target in targets)
        {
            WaveData data;
            data = ScriptableObject.CreateInstance<WaveData>();

            bool negate = target.Negate != "";

            Piece piece = GameController.DataLevel.Pieces.Find(p => p.ID == target.TargetLink);

            if (piece == null)
                throw new Exception("Could not find piece " + target.TargetLink);

            int MAX_VALUES = 10;
            int data_length = 0;
            for (int i = MAX_VALUES - 1; i >= 0; i--)
            {
                if (piece.GetR(i) != "")
                {
                    data_length = i + 1;
                    break;
                }
            }
            Debug.Log("Piece  " + piece.ID + " with size " + data_length);

            data.values = new int[data_length];
            int defaultOut = 0;
            for (int i = 0; i < data_length; i++)
            {
                //Debug.Log(i);
                //Debug.Log(piece.GetR(i));
                data.values[i] = int.TryParse(piece.GetR(i), out defaultOut) ? defaultOut : -int.Parse(piece.GetR(i));
                if (negate) data.values[i] *= -1;
            }

            dataList.Add(data);
        }
        return dataList.ToArray();
    }

    /// <summary>
    /// Waves the data target proxy.
    /// </summary>
    /// <param name="_targetId">The target identifier.</param>
    /// <returns></returns>
    public WaveData WaveDataTargetProxy(string _targetId)
    {
        WaveData data;

        //Debug.Log(GameController.DataLevel);
        Target target = GameController.DataLevel.Targets.Find(t => t.ID == _targetId && t.TargetLink == "");
        if (target == null)
            return null;

        data = ScriptableObject.CreateInstance<WaveData>();

        int MAX_VALUES = 10;
        int data_length = 0;
        for (int i = MAX_VALUES - 1; i >= 0; i--)
        {
            if (target.GetR(i) != "") {
                data_length = i + 1;
                break;
            }
        }
        Debug.Log("Target " + target.ID + " has size " + data_length);

        data.values = new int[data_length];
        int defaultOut = 0;
        for (int i = 0; i < data_length; i++)
        {
            data.values[i] = int.TryParse(target.GetR(i), out defaultOut) ? defaultOut : -int.Parse(target.GetR(i));
           // Debug.Log("Target " +  i + ": "  + data.values[i]);
        }

        return data;
    }

    public int GetWindow(string _targetId)
    {
        Target target = GameController.DataLevel.Targets.Find(t => t.ID == _targetId && t.TargetLink == "");
        if (target == null)
            return 0;
        return int.Parse(target.Window);
    }

    public int GetSpan(string _targetId)
    {
        Target target = GameController.DataLevel.Targets.Find(t => t.ID == _targetId && t.TargetLink == "");
        if (target == null)
            return 0;
        return int.Parse(target.Span);
    }

    #endregion
}
