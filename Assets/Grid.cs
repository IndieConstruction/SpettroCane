using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public int Height = 15;
    public int Width = 10;
    public int YOffset = 10;

    public Transform WaveContainerPrefab;
    public List<Wave> Waves;

    private void Start() {
        SetWaves();
    }

    public void SetWaves() {
        for (int i = 0; i < Waves.Count; i++) {
            Transform newTransform =  Instantiate<Transform>(WaveContainerPrefab);
            newTransform.localScale = new Vector3(Width, Height, 1);
            newTransform.position = new Vector3(0, (i * Height) + (i * YOffset), 0);

            Wave newWave = Instantiate<Wave>(Waves[i]);
            // newWave.transform.position = newTransform.position + new Vector3(-Width / 2, -Height/2, -1);
            newWave.transform.position = newTransform.position + new Vector3((-Width / 2) + 0.5f, 0, -1);
        }
    }
    
    
}

