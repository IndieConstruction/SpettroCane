using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private List<int> heights = new List<int>();
    private List<Bar> bars = new List<Bar>();
    public GameObject barPrefab;

    public int nValues = 10;

    public float period = 0;

    private float barSize = 1f;
    public int maxHeight = 10;

    private System.Func<float,int> WaveDelegate;

    public void Awake()
    {
        WaveDelegate += MyCos;

        int[] values = new int[nValues];
        for (int i = 0; i < nValues; i++)
        {
            values[i] = WaveDelegate(i);
            //Debug.Log(values[i]);
        }

        CreateWave(values);

        StartCoroutine(MoveWaveCO());
    }

    int MyCos(float value)
    {
        return (value > 5 && value < 9) ? -maxHeight : ((value > 2 && value < 7) ? maxHeight : 0);
       // return (int)(10 * Mathf.Cos(value));
    } 

    void CreateWave(int[] _heights)
    {
        heights.Clear();
        heights.AddRange(_heights);

        for (int i = 0; i < _heights.Length; i++)
        {
            GameObject go = Instantiate(barPrefab);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.right * i * barSize;
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
                    var pos = bars[i].transform.localPosition;
                    var size = bars[i].transform.localScale;
                    pos.y = heights[i] / 2f;
                    size.y = heights[i];
                    bars[i].transform.localScale = size;
                    bars[i].transform.localPosition = pos;

                    if (heights[i] < 0)
                    {
                        bars[i].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                    } else
                    {
                        bars[i].GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                    }
                }

                if (period == 0) yield break;
            }
            yield return null;
        }
    }

    void WaveSum()
    {

    }
}
