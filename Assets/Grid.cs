using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public int Height = 15;
    public int Width = 10;
    public int YOffset = 10;

    public Transform WaveContainerPrefab;

    private void Start() {
        SetWaves();
    }

    public void SetWaves() {
        // Target wave
        Transform newTransformTarget = Instantiate<Transform>(WaveContainerPrefab);
        newTransformTarget.localScale = new Vector3(Width, Height, 1);
        newTransformTarget.position = new Vector3(0, Height, 0);

        Wave newWaveTarget = Instantiate<Wave>(GameController.Instance.targetWave);
        newWaveTarget.transform.position = newTransformTarget.position + new Vector3((-Width / 2) + 0.5f, 0, -1);

        // 
        for (int i = 0; i < GameController.Instance.gameWaves.Count; i++) {
            Transform newTransform = Instantiate<Transform>(WaveContainerPrefab);
            newTransform.localScale = new Vector3(Width, Height, 1);
            newTransform.position = new Vector3(0, (i * Height) + (i * YOffset), 0);

            Wave newWave = Instantiate<Wave>(GameController.Instance.gameWaves[i]);
            // newWave.transform.position = newTransform.position + new Vector3(-Width / 2, -Height/2, -1);
            newWave.transform.position = newTransform.position + new Vector3((-Width / 2) + 0.5f, 0, -1);
        }
    }
    
    
}

