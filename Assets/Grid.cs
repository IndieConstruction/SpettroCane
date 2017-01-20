using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public int Height = 15;
    public int Width = 10;
    public int YOffset = 10;

    public Transform WaveContainerPrefab;
    public int NumberOfWaves;

    private void Start() {
        SetWaves();
    }

    public void SetWaves() {
        for (int i = 0; i < NumberOfWaves; i++) {
            Transform newTransform =  Instantiate<Transform>(WaveContainerPrefab);
            newTransform.localScale = new Vector3(Width, Height, 1);
            newTransform.position = new Vector3(0, (i * Height) + (i * YOffset), 0);
        }
    }

}

