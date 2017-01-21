using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Wave targetWave;
    public List<Wave> gameWaves;

    public static GameController Instance;

    private void Awake() {
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

        if (Input.GetKeyDown(KeyCode.A)) {
            GameController.Instance.gameWaves[0].ShiftWave(GameController.Instance.gameWaves[0].windowStep - GameController.Instance.gameWaves[0].windowSize);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            GameController.Instance.gameWaves[0].ShiftWave(GameController.Instance.gameWaves[0].windowStep + GameController.Instance.gameWaves[0].windowSize);
        }
    }

}
