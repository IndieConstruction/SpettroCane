using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Wave targetWave;
    public List<Wave> gameWaves;

    public static GameController Instance;

    void Start ()
    {
        if (Instance == null)
            Instance = this;
	}
	
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            targetWave.SumWave(gameWaves[0]);

            if (targetWave.IsWaveZero())
            {
                Debug.Log("ZEROED!");
            }
        }
	}

}
