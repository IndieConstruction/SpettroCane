using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private List<int> heights = new List<int>();
    private float period = 1f;
    private List<Bar> bars = new List<Bar>();
    public GameObject barPrefab;

    private float barSize = 1f;

    public void Awake()
    {
        CreateWave(new int[]
        {
            0,1,2,3,4,
            5,5,4,3,2,
        },
        0.25f
        );

        StartCoroutine(MoveWaveCO());
    }

    void CreateWave(int[] _heights, float period)
    {
        heights.Clear();
        heights.AddRange(_heights);

        for (int i = 0; i < _heights.Length; i++)
        {
            GameObject go = Instantiate(barPrefab);
            go.transform.SetParent(transform);
            go.transform.position = Vector3.right * i * barSize;
            bars.Add(go.GetComponent<Bar>());
        }
    }

    IEnumerator MoveWaveCO()
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime;
            if (t >= period)
            {
                t -= period;

                // Shift the wave
                List<int> newHeights = new List<int>();
                for (int i = 0; i < heights.Count; i++)
                {
                    var j = i + 1;
                    if (j >= heights.Count) j = 0;
                    newHeights.Add(heights[j]);
                }

                // Show the wave
                for (int i = 0; i < heights.Count; i++)
                {
                    heights[i] = newHeights[i];
                    var pos = bars[i].transform.position;
                    pos.y = heights[i];
                    bars[i].transform.position = pos;
                }
            }
            yield return null;
        }
    }
}
