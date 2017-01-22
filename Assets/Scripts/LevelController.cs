using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public WaveDataPack[] levelPacks;
    public List<Target> TargetList;
    public List<Piece> Pieces;
    private int currentLevel = 0;

    private void Start()
    {
        GameController.OnWin += OnWin;
        SetLevel(0);
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AdvanceLevel();
            }
        }
        if (levelLost)
        {
            if (Input.GetKeyDown(KeyCode.Space))
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

        if (GameController.Instance.EnableNewLevelType) {
            // Create Target
            // GameController.Instance.gameWave.CreateFromWaveDatas(WaveDataProxy("1"));
            WaveData targetWave = WaveDataTargetProxy(levelId.ToString());
            if (targetWave != null) {
                GameController.Instance.targetWave.CreateFromWaveData(targetWave);

                WaveData[] gameWaves = WaveDataProxy(levelId.ToString());
                GameController.Instance.gameWave.CreateFromWaveDatas(gameWaves);
            }
        }

        // ******************************

    }

    #region New static level data

    /// <summary>
    /// Return Wave from Target ID
    /// </summary>
    /// <param name="_targetId">The target identifier.</param>
    /// <returns></returns>
    public WaveData[] WaveDataProxy(string _targetId) {
        List<WaveData> dataList = new List<WaveData>();

        List<Target> targets = GameController.DataLevel.Targets.FindAll(t => t.ID == _targetId && t.TargetLink != "");
        foreach (var target in targets) {
            WaveData data;
            data = ScriptableObject.CreateInstance<WaveData>();
            data.values = new int[10];

            Piece piece = GameController.DataLevel.Pieces.Find(p => p.ID == target.TargetLink);

            int defaultOut = 0;
            
            data.values[0] = int.TryParse(piece.R0, out defaultOut) == false ? defaultOut : -int.Parse(piece.R0);
            data.values[1] = int.TryParse(piece.R1, out defaultOut) == false ? defaultOut : -int.Parse(piece.R1);
            data.values[2] = int.TryParse(piece.R2, out defaultOut) == false ? defaultOut : -int.Parse(piece.R2);
            data.values[3] = int.TryParse(piece.R3, out defaultOut) == false ? defaultOut : -int.Parse(piece.R3);
            data.values[4] = int.TryParse(piece.R4, out defaultOut) == false ? defaultOut : -int.Parse(piece.R4);
            data.values[5] = int.TryParse(piece.R5, out defaultOut) == false ? defaultOut : -int.Parse(piece.R5);
            data.values[6] = int.TryParse(piece.R6, out defaultOut) == false ? defaultOut : -int.Parse(piece.R6);
            data.values[7] = int.TryParse(piece.R7, out defaultOut) == false ? defaultOut : -int.Parse(piece.R7);
            data.values[8] = int.TryParse(piece.R8, out defaultOut) == false ? defaultOut : -int.Parse(piece.R8);
            data.values[9] = int.TryParse(piece.R9, out defaultOut) == false ? defaultOut : -int.Parse(piece.R9);

            dataList.Add(data);
        }
        return dataList.ToArray();
    }

    /// <summary>
    /// Waves the data target proxy.
    /// </summary>
    /// <param name="_targetId">The target identifier.</param>
    /// <returns></returns>
    public WaveData WaveDataTargetProxy(string _targetId) {
        WaveData data;

        Debug.Log(GameController.DataLevel);
        Target target = GameController.DataLevel.Targets.Find(t => t.ID == _targetId && t.TargetLink == "");
        if (target == null)
            return null;
        data = ScriptableObject.CreateInstance<WaveData>();
        data.values = new int[10];
        int defaultOut = 0;
        
        data.values[0] = int.TryParse(target.R0, out defaultOut) == false ? defaultOut : int.Parse(target.R0);
        data.values[1] = int.TryParse(target.R1, out defaultOut) == false ? defaultOut : int.Parse(target.R1);
        data.values[2] = int.TryParse(target.R2, out defaultOut) == false ? defaultOut : int.Parse(target.R2);
        data.values[3] = int.TryParse(target.R3, out defaultOut) == false ? defaultOut : int.Parse(target.R3);
        data.values[4] = int.TryParse(target.R4, out defaultOut) == false ? defaultOut : int.Parse(target.R4);
        data.values[5] = int.TryParse(target.R5, out defaultOut) == false ? defaultOut : int.Parse(target.R5);
        data.values[6] = int.TryParse(target.R6, out defaultOut) == false ? defaultOut : int.Parse(target.R6);
        data.values[7] = int.TryParse(target.R7, out defaultOut) == false ? defaultOut : int.Parse(target.R7);
        data.values[8] = int.TryParse(target.R8, out defaultOut) == false ? defaultOut : int.Parse(target.R8);
        data.values[9] = int.TryParse(target.R9, out defaultOut) == false ? defaultOut : int.Parse(target.R9);


        return data;
    }


    #endregion



}
