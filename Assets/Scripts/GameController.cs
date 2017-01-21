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

    /// <summary>
    /// Checks the lose.
    /// </summary>
    public void CheckLose(List<int> _elements) {
        // Lose
        if (_elements.FindIndex(b => b != 0) == -1) {
            if (OnLose != null) {
                OnLose();
            }
            Debug.Log("Lose");
        }
    }

    /// <summary>
    /// Checks the win.
    /// </summary>
    public bool CheckWin(List<int> _elements) {
        // Lose
        if (_elements.FindIndex(b => b != 0) == -1) {
            if (OnWin != null) {
                OnWin();
            }
            Debug.Log("Win");
            return true;
        }
        return false;
    }

    #region Events

    public delegate void GameFlowEvent();

    public static event GameFlowEvent OnWin;
    public static event GameFlowEvent OnLose;

    #endregion

}
