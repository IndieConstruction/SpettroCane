using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Wave targetWavePrefab;
    public List<Wave> gameWavesPrefab;

    public int ShiftWaveAmount = 1;

    //[HideInInspector]
    public Wave targetWave, gameWave;

    public static GameController Instance;

    public bool canManualControl = true;

    private void Awake() {
        if (Instance == null)
            Instance = this;
    }
    
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space) && canManualControl)
        {
            canManualControl = false;
            targetWave.SumWave(gameWave);

            if (targetWave.IsWaveZero())
            {
                Debug.Log("ZEROED!");
            }
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            GameController.Instance.gameWave.ShiftWave(ShiftWaveAmount);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            GameController.Instance.gameWave.ShiftWave(-ShiftWaveAmount);
        }
    }

}
