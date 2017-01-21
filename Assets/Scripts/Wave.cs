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

    public bool autoMove = true;
    public float autoPeriod = 0;

    // What the player can drop down (active size)
    public int windowStep = 1;
    public int lookWindowSize = 20;
    public int playWindowSize = 10;

    private List<int> allHeights = new List<int>();         // all heights, as updated, read from the data initially
    private List<int> windowHeights = new List<int>();      // current heights shown in the window

    [HideInInspector]
    public int windowOffset;   // current window offset, increased at each step

    public Transform zeroLine;

    // Bar
    private List<Bar> bars = new List<Bar>();
    public GameObject barPrefab;
    private float barPlayWidth = 2f;
    //private float barActualWidth = 1.8f;

    public int maxHeight = 4;

    //private System.Func<float,int> WaveDelegate;

    public int HeightsNum { get { return allHeights.Count; } }

    private int PlayWindowStart { get { return (lookWindowSize / 2 - playWindowSize / 2); } }
    private int PlayWindowEnd { get { return (lookWindowSize / 2 + playWindowSize / 2); } }

    #region Wave data
    public void CreateFromWaveData(WaveData waveData) {
        List<int> values = new List<int>();
        values.AddRange(waveData.values);

        CreateWave(values);
        Draw();
    }

    public void CreateFromWaveDatas(WaveData[] waveDatas) {
        List<int> values = new List<int>();
        for (int i = 0; i < waveDatas.Length; i++)
            values.AddRange(waveDatas[i].values);

        CreateWave(values);
        Draw();
    }

    public void CreateWithTestData() {
        int nValues = 100;
        List<int> values = new List<int>();
        for (int i = 0; i < nValues; i++)
            values[i] = (int)(maxHeight * Mathf.Cos(i * 0.1f));

        CreateWave(values);
        Draw();
    }
    #endregion

    public void Awake() {
        // Init
        windowOffset = 0;

        if (testCosine) {
            CreateWithTestData();
        }

        if (autoMove)
            StartCoroutine(MoveWaveCO());
    }

    void CreateWave(List<int> _heights) {
        allHeights.Clear();
        allHeights.AddRange(_heights);

        // Instantiate
        for (int i = 0; i < lookWindowSize; i++)
        {
            GameObject go = Instantiate(barPrefab);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.right * i * barPlayWidth;
            Bar newBar = go.GetComponent<Bar>();
            bars.Add(newBar);
        }
    }

    IEnumerator MoveWaveCO() {
        float t = 0;
        while (true) {
            if (autoMove) {
                t += Time.deltaTime;
                if (t >= autoPeriod)
                {
                    t -= autoPeriod;
                    ShiftWave(windowStep);
                }
            }
            yield return null;
        }
    }

    void StopMovement() {
        autoMove = false;
    }

    public void ShiftWave(int step) {
        // Shift the wave
        windowOffset += step;
        if (windowOffset >= allHeights.Count)
            windowOffset = 0;
        if (windowOffset < 0)
            windowOffset = allHeights.Count;

        //Debug.Log(windowOffset);
        //Debug.Log("Shift wave called: " + windowOffset);
        Draw();
    }

    public void SumWave(Wave other) {
        // Summing from OTHER to THIS
        //Debug.Log("SUMMING OTHER " + other.windowOffset);
        other.StartCoroutine(other.AnimateFallCO(this));
    }

    void UpdateSum(Wave other) {
        // We sum the current wave here
        for (int i = other.PlayWindowStart; i < other.PlayWindowEnd; i++)
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
    public float fallPeriod = 0.2f;
    public float fallDelay = 0.05f;
    public Ease fallEase;
    Wave toWave;

    public IEnumerator AnimateFallCO(Wave toWave) {
        this.toWave = toWave;
        StopMovement();
        for (int i = PlayWindowStart; i < PlayWindowEnd; i++)
        {
            if (allHeights[i] != 0) {
                var bar = bars[i];
                var tweener = bar.transform.DOMoveY(toWave.transform.position.y +
                    +windowHeights[i] / 2f
                    + toWave.allHeights[i] * 1f, fallPeriod).SetEase(fallEase);
                if (i == playWindowSize - 1)
                    tweener.OnComplete(EndSum);
                yield return new WaitForSeconds(fallDelay);
            }
            yield return null;
        }
    }

    void EndSum() {
        StartCoroutine(EndSumCO());
    }

    IEnumerator EndSumCO() {
        yield return new WaitForSeconds(0.5f);

        // Destroy this bars
        for (int i = PlayWindowStart; i < PlayWindowEnd; i++)
        {
            var bar = bars[i];
            var data_i = i + windowOffset;
            if (data_i >= allHeights.Count) data_i -= allHeights.Count;
            SoftDestroyBar(bar,data_i);
        }

        // Make sure the other gets updated now
        toWave.UpdateSum(this);
    }

    void SoftDestroyBar(Bar _barToDestroy, int index) {
        float effectDuration = 0.2f;
        _barToDestroy.barSprite.DOFade(0, effectDuration).OnComplete(() => {
            var tmp = _barToDestroy.transform.localPosition;
            tmp.y = 0;
            _barToDestroy.transform.localPosition = tmp;

            Debug.Log("SET TO ZERO " + index);
            allHeights[index] = 0;
        });
        _barToDestroy.transform.DOScaleY(0, effectDuration);
    }


    public bool IsWaveZero()
    {
        return allHeights.Sum() == 0;
    }

    void Draw()
    {
        SetupLine();

        // Update the window
        windowHeights.Clear();
        for (int i = 0; i < lookWindowSize; i++)
        {
            var j = windowOffset + i;
            while (j >= allHeights.Count) j -= allHeights.Count;
            if (j < 0) j += allHeights.Count;
            windowHeights.Add(allHeights[j]);
        }

        // Draw it
        for (int i = 0; i < lookWindowSize; i++)
        {
            //Debug.Log("DRAW " + this.name);
            var pos = bars[i].transform.localPosition;
            var size = bars[i].transform.localScale;
            pos.y = windowHeights[i] / 2f;

            if (windowHeights[i] < 0)
            {
                if (withColors)
                {
                    if (i >= PlayWindowStart && i < PlayWindowEnd)
                    {
                        bars[i].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                        bars[i].name = "PLAY";
                    }
                    else
                    {
                        bars[i].GetComponentInChildren<MeshRenderer>().material.color = new Color(0.2f, 0, 0, 1);
                        bars[i].name = "look";
                    }
                }

                if (colorsOnly)
                    pos.y = -allHeights[i] / 2f;
            }
            else
            {
                if (withColors)
                {
                    if (i >= PlayWindowStart && i < PlayWindowEnd)
                    {
                        bars[i].GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                        bars[i].name = "PLAY";
                    }
                    else
                    {
                        bars[i].GetComponentInChildren<MeshRenderer>().material.color = new Color(0, 0.2f, 0, 1);
                        bars[i].name = "look";
                    }
                }
            }

            size.y = windowHeights[i];
            bars[i].transform.localScale = size;
            bars[i].transform.localPosition = pos;
        }

    }

    void SetupLine()
    {
        float lineOffset = barPlayWidth * lookWindowSize;
        zeroLine.transform.localPosition = new Vector3(lineOffset / 2f - barPlayWidth, 0, 0);
        zeroLine.transform.localScale = new Vector3(lineOffset + 2*barPlayWidth, 0.1f, 1);

        var tmpLocPos = transform.localPosition;
        tmpLocPos.x = -barPlayWidth * lookWindowSize / 2 + barPlayWidth/2;
        transform.localPosition = tmpLocPos;
    }
}
