using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tremata : MonoBehaviour {

    void Start()
    {
        startPos = this.transform.localPosition;
        GameController.OnLose += StartTremble;
        LevelController.OnNewLevel += StopTremble;
    }

    void StartTremble() { 
        StartCoroutine(TrembleCO());
	}

    void StopTremble()
    {
        this.transform.localPosition = startPos;
        StopAllCoroutines();
    }

    public float magnitudo = 10;
    public float period = 0.05f;
    private Vector3 startPos;

    IEnumerator TrembleCO()
    {
	    while (true)
	    {
	        yield return new WaitForSeconds(period);
	        this.transform.localPosition = startPos + new Vector3(
                Random.Range(-magnitudo, magnitudo), Random.Range(-magnitudo, magnitudo), 0);
	    }
	}
}
