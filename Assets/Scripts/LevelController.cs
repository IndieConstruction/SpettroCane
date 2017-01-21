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
        GameController.Instance.gameWave.CreateFromWaveDatas(levelPacks[levelId].inputs);
        //GameController.Instance.targetWave.CreateFromWaveData(levelPacks[levelId].target);

        // test: auto level generator
        var targetWaveData = WaveData.CreateInstance<WaveData>();
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
        GameController.Instance.targetWave.CreateFromWaveData(targetWaveData);
    }

}
