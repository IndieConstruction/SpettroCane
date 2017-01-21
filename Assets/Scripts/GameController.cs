using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Wave targetWave;
    public Wave gameWave;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            targetWave.SumWave(gameWave);

            if (targetWave.IsWaveZero())
            {
                Debug.Log("ZEROED!");
            }
        }
	}

}
