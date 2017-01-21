using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Wave : MonoBehaviour
{
    // OPTIONS
    private bool withColors = true;
    private bool colorsOnly = false;

    public bool testCosine = false;

    public bool canMove = true;

    public WaveData wData;

    public int windowSize = 10;
    public int windowStep = 1;

    private List<int> allHeights = new List<int>();         // all heights, as updated, read from the data initially
    private List<int> windowHeights = new List<int>();      // current heights shown in the window

    [HideInInspector]
    public int windowOffset;   // current window offset, increased at each step

    private List<Bar> bars = new List<Bar>();
    public GameObject barPrefab;

    public float period = 0;

    private float barSize = 1f;

    public int maxHeight = 4;

    private System.Func<float,int> WaveDelegate;

    public void Awake()
    {
        // Init
        windowOffset = 0;

        WaveDelegate += MyData;

        int nValues;
        int[] values;

        if (testCosine)
        {
            nValues = wData.values.Length;
            values = new int[nValues];
            for (int i = 0; i < nValues; i++)
            {
                values[i] = WaveDelegate(i);
            }
        }
        else
        {
            nValues = 100;
            values = new int[nValues];
            for (int i = 0; i < nValues; i++)
            {
                values[i] = MyCos(i*0.1f);
            }
        }

        CreateWave(values);

        Draw();

        if (canMove)
            StartCoroutine(MoveWaveCO());
    }

    int MyData(float value)
    {
        return wData.values[(int)value];
    }

    int MyCos(float value)
    {
       // return (value > 5 && value < 9) ? -maxHeight : ((value > 2 && value < 7) ? maxHeight : 0);
        return (int)(maxHeight * Mathf.Cos(value));
    } 

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
            if (canMove)
            {
                t += Time.deltaTime;
                if (t >= period)
                {
                    t -= period;
                    ShiftWave(windowStep);
                }
            }
            yield return null;
        }
    }

    void StopMovement()
    {
        canMove = false;
    }

    public void ShiftWave(int step)
    {
        // Shift the wave
        windowOffset += step;
        if (windowOffset >= allHeights.Count)
            windowOffset = 0;
        if (windowOffset < 0)
            windowOffset = allHeights.Count - 1;

        //Debug.Log(windowOffset);
        Debug.Log("Shift wave called: " + windowOffset);
        Draw();
    }

    public void SumWave(Wave other)
    {
        // Summing from OTHER to THIS
        Debug.Log("SUMMING OTHER " + other.windowOffset);
        other.StartCoroutine(other.AnimateFallCO(this));
    }

    void UpdateSum(Wave other)
    {
        // We sum the current wave here
        for (int i = 0; i < other.windowSize; i++)
        {
            allHeights[i] += other.windowHeights[i];
            //Debug.Log("all: " +  this.allHeights[i] + " wind: " + other.windowHeights[i]);
        }

        Draw();
    }

    /*public IEnumerator EndSumCO(Wave other)
    {
        yield return StartCoroutine(AnimateFallCO());
    }*/


    // Animate falling over toWave
    private float fallPeriod = 0.2f;
    private float fallDelay = 0.05f;
    Wave toWave;
    public IEnumerator AnimateFallCO(Wave toWave)
    {
        this.toWave = toWave;
        StopMovement();
        for (int i = 0; i < windowSize; i++)
        {
            var bar = bars[i];
            var tweener = bar.transform.DOMoveY(toWave.transform.position.y +
                + windowHeights[i]/ 2f + toWave.allHeights[i] * 1.5f, fallPeriod);
            if (i == windowSize - 1)
            tweener.OnComplete(EndSum);
            yield return new WaitForSeconds(fallDelay);
        }
    }

    void EndSum()
    {
        // Destroy this bars
        for (int i = 0; i < windowSize; i++)
        {
            var bar = bars[i];
            Destroy(bar.gameObject);
        }

        // Make sure the other gets updated now
        this.toWave.UpdateSum(this);
    }


    public bool IsWaveZero()
    {
        return allHeights.Sum() == 0;
    }

    void Draw()
    {
        // Update the window
        windowHeights.Clear();
        for (int i = 0; i < windowSize; i++)
        {
            var j = windowOffset + i;
            if (j >= allHeights.Count) j -= allHeights.Count;
            //Debug.Log("i " + i + ": j " + j);
            windowHeights.Add(allHeights[j]);
        }

        // Draw it
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
