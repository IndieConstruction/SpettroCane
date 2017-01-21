using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Wave : MonoBehaviour
{
    public bool IsTargetWave = false;

    // OPTIONS
    private bool withColors = true;
    private bool colorsOnly = false;

    public bool testCosine = false;

    public bool autoMove = true;
    public bool manualMove = true;
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

    private float endWait = 0.3f;


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

    void CreateWave(List<int> _heights)
    {
        allHeights.Clear();
        allHeights.AddRange(_heights);

        if (allHeights.Count < lookWindowSize)
        {
            throw new System.Exception("ALL HEIGHTS MUST BE LONGER THAN LOOK WINDOW SIZE!");
        }

        // Instantiate
        for (int i = 0; i < lookWindowSize; i++)
        {
            GameObject go = Instantiate(barPrefab);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.right * i * barPlayWidth;
            Bar newBar = go.GetComponent<Bar>();
            newBar.Initialise(maxHeight);
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

    void StopMovement()
    {
        autoMove = false;
        manualMove = false;
    }

    void EnableMovement()
    {
        manualMove = true;
        GameController.Instance.canManualControl = true;
    }

    public void ShiftWave(int step) {

        if (manualMove == false) return;

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

    void UpdateSum(Wave other)
    {
        // We sum the current wave here
        for (int i = other.PlayWindowStart; i < other.PlayWindowEnd; i++)
        {
            //Debug.Log(i + " to " + (i - other.PlayWindowStart));
            allHeights[i] += other.windowHeights[i];
            allHeights[i] = Mathf.Clamp(allHeights[i], -maxHeight, maxHeight);

            //Debug.Log("all: " +  this.allHeights[i] + " wind: " + other.windowHeights[i]);
        }

        Draw();
    }

    // Animate falling over toWave
    public float fallPeriod = 0.2f;
    public float fallDelay = 0.05f;
    public Ease fallEase;
    Wave toWave;
    public IEnumerator AnimateFallCO(Wave toWave) {
        this.toWave = toWave;
        StopMovement();
        Tweener lastTweener = null;
        for (int i = PlayWindowStart; i < PlayWindowEnd; i++)
        {
            if (windowHeights[i] == 0) continue;
            
            var bar = bars[i];
            Tweener tweener = bar.transform.DOMoveY(toWave.transform.position.y +
                + 0.5f
               // + windowHeights[i] / 2f 
                + toWave.allHeights[i] * 1f, fallPeriod).SetEase(fallEase);
            lastTweener = tweener;
            yield return new WaitForSeconds(fallDelay);
        }

        if (lastTweener == null) {
            EndSum();
        }
        else {
            lastTweener.OnComplete(EndSum);
        }
    }

    void EndSum() {
        StartCoroutine(EndSumCO());
    }

    IEnumerator EndSumCO() {
        yield return new WaitForSeconds(endWait);

        // Destroy this bars
        for (int i = PlayWindowStart; i < PlayWindowEnd; i++)
        {
            var bar = bars[i];

            //bar.GetComponentInChildren<MeshRenderer>().material.DOColor(Color.white, 0.2f).SetLoops(2, LoopType.Yoyo);

            var data_i = i + windowOffset;
            if (data_i >= allHeights.Count) data_i -= allHeights.Count;
            SoftDestroyBar(bar, i, data_i, lastone: i == PlayWindowEnd-1);
        }

        // Make sure the other gets updated now
        toWave.UpdateSum(this);

        //StartCoroutine(toWave.WinCO());
    }

    void SoftDestroyBar(Bar _barToDestroy, int windowIndex, int dataIndex, bool lastone = false)
    {
        float effectDuration = 0.2f;

        foreach (var cube in _barToDestroy.cubes)
        {
            cube.transform.DOScaleY(0, effectDuration).OnComplete(() =>
            {
                var tmp = _barToDestroy.transform.localPosition;
                tmp.y = 0;
                _barToDestroy.transform.localPosition = tmp;
                cube.gameObject.SetActive(false);
                cube.transform.localScale = new Vector3(1.7f, 0.8f, 0.1f);

                //Debug.Log("SET TO ZERO " + windowIndex);
                windowHeights[windowIndex] = 0;
                allHeights[dataIndex] = 0;
                //  var col = cube.GetComponent<MeshRenderer>().material.color;
                //  col.a = 1;
                //  cube.GetComponent<MeshRenderer>().material.color = col;

                if (lastone)
                {
                    // Re-enable movement
                    EnableMovement();

                    if (!IsTargetWave)
                    {
                        if (!GameController.Instance.CheckWin(toWave.allHeights))
                            GameController.Instance.CheckLose(allHeights);
                    }
                }
            });
        }

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
            pos.y = 1f / 2f;// windowHeights[i] / 2f;

            if (windowHeights[i] < 0)
            {
                if (withColors)
                {
                    if (i >= PlayWindowStart && i < PlayWindowEnd)
                    {
                        bars[i].SetColor(Color.blue, Color.cyan);
                       // bars[i].SetColor(new Color(1, 0.5f, 0.5f, 1), new Color(0.6f, 0, 0, 1));
                        //bars[i].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                        bars[i].name = "PLAY";
                    }
                    else
                    {
                        bars[i].SetColor(new Color(0.2f, 0, 0, 1), new Color(0.2f, 0, 0, 1));
                        //bars[i].GetComponentInChildren<MeshRenderer>().material.color = new Color(0.2f, 0, 0, 1);
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
                        bars[i].SetColor(Color.yellow, new Color(1f, 0.5f, 0, 1));
                        bars[i].name = "PLAY";
                    }
                    else
                    {
                        bars[i].SetColor(new Color(0, 0.2f, 0, 1), new Color(0, 0.2f, 0, 1));
                        bars[i].name = "look";
                    }
                }
            }

            bars[i].SetHeight(windowHeights[i]);

            bars[i].gameObject.SetActive(windowHeights[i] != 0);
            bars[i].transform.localPosition = pos;
        }

    }

    void SetupLine()
    {
        float lineOffset = barPlayWidth * lookWindowSize;
        zeroLine.transform.localPosition = new Vector3(lineOffset / 2f - barPlayWidth/2f, 0, 0);
        zeroLine.transform.localScale = new Vector3(lineOffset + 2*barPlayWidth, 0.1f, 1);

        var tmpLocPos = transform.localPosition;
        tmpLocPos.x = -barPlayWidth * lookWindowSize / 2 + barPlayWidth/2;
        transform.localPosition = tmpLocPos;
    }

    IEnumerator WinCO()
    {
        autoPeriod = 0.1f;
        autoMove = true;

        for (int i = 0; i < bars.Count; i++)
        {
            bars[i].Initialise(10);
        }

        while (true)
        {
            for (int i = 0; i < bars.Count; i++)
            {
                Color color1 = Color.HSVToRGB(Mathf.Repeat(Time.time*2f + i * 0.1f, 1f), 1, 1);
                Color color2 = Color.HSVToRGB(Mathf.Repeat(Time.time * 5f + i *0.1f, 1f), 1, 1);
                bars[i].SetColor(color1, color2);
                bars[i].SetHeight((int)Mathf.Round(10*(Mathf.PerlinNoise(Time.time * 4f + i*2f, 0))));
            }
            yield return null;
        }
    }
}
