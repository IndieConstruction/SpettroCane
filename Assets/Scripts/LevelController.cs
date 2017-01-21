using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public WaveDataPack[] levelPacks;

    private void Start()
    {
        SetLevel(0);
    }
    
	void SetLevel(int levelId)
    {
        GameController.Instance.gameWaves[0].CreateFromWaveDatas(levelPacks[levelId].inputs);
        GameController.Instance.targetWave.CreateFromWaveData(levelPacks[levelId].target);

        // test: auto level generator
        /*var targetWaveData = WaveData.CreateInstance<WaveData>();
        targetWaveData.values = new int[GameController.Instance.gameWaves[0].HeightsNum];
        for (int i = 0; i < levelPacks[levelId].inputs.Length; i++)
        {

        }
       GameController.Instance.targetWave*/
    }

}
