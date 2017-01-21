using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Wave : MonoBehaviour
{
    // OPTIONS
    private bool withColors = true;
    private bool colorsOnly = false;

    public WaveData wData;

    public int windowSize = 10;
    public int windowStep = 1;

    private List<int> allHeights = new List<int>();         // all heights, as updated, read from the data initially
    private List<int> windowHeights = new List<int>();      // current heights shown in the window

    [HideInInspector]
    public int windowOffset = 0;   // current window offset, increased at each step

    private List<Bar> bars = new List<Bar>();
    public GameObject barPrefab;

    public float period = 0;

    private float barSize = 1f;

    public int maxHeight = 4;

    private System.Func<float,int> WaveDelegate;

    public void Awake()
    {
        WaveDelegate += MyData;

        int[] values = new int[wData.values.Length];
        for (int i = 0; i < wData.values.Length; i++)
        {
            values[i] = WaveDelegate(i);
            //Debug.Log(values[i]);
        }
        CreateWave(values);

        StartCoroutine(MoveWaveCO());
    }

    int MyData(float value)
    {
        return wData.values[(int)value];
    }

    /*int MyCos(float value)
    {
        return (value > 5 && value < 9) ? -maxHeight : ((value > 2 && value < 7) ? maxHeight : 0);
       // return (int)(10 * Mathf.Cos(value));
    } */

    void CreateWave(int[] _heights)
    {
        allHeights.Clear();
        allHeights.AddRange(_heights);

        // Instantiate
        for (int i = 0; i < windowSize; i++)
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
                windowOffset += windowStep;
                if (windowOffset >= allHeights.Count)
                    windowOffset = 0;

                //Debug.Log(windowOffset);

                windowHeights.Clear();
                for (int i = 0; i < windowSize; i++)
                {
                    var j = windowOffset + i;
                    if (j >= allHeights.Count) j -= allHeights.Count;
                    //Debug.Log("j at " + i + ": " + j);
                    windowHeights.Add(allHeights[j]);
                }

                Draw();

                if (period == 0) yield break;
            }
            yield return null;
        }
    }

    public void SumWave(Wave other)
    {
        Debug.Log("SUMMING OTHER " + other.windowOffset);

       // StartCoroutine(AnimateFallCO(other));

        // We sum the current wave here
        for (int i = 0; i < other.windowSize; i++)
        {
            allHeights[i] += other.windowHeights[i];
            Debug.Log(other.windowHeights[i]);
        }

        Draw();
    }

    IEnumerator AnimateFallCO()
    {
        while (true)
        {

        }
        //other
    }

    public bool IsWaveZero()
    {
        return allHeights.Sum() == 0;
    }

    void Draw()
    {
        for (int i = 0; i < windowSize; i++)
        {
            //Debug.Log("DRAW " + this.name);
            var pos = bars[i].transform.localPosition;
            var size = bars[i].transform.localScale;
            pos.y = windowHeights[i] / 2f;
            if (windowHeights[i] < 0)
            {
                if (withColors)
                    bars[i].GetComponentInChildren<MeshRenderer>().material.color = Color.red;

                if (colorsOnly)
                    pos.y = -allHeights[i] / 2f;
            }
            else
            {
                if (withColors)
                    bars[i].GetComponentInChildren<MeshRenderer>().material.color = Color.green;
            }

            size.y = windowHeights[i];
            bars[i].transform.localScale = size;
            bars[i].transform.localPosition = pos;
        }

    }
}
