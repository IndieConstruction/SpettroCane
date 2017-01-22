using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region New Level Types
    public static LevelsStaticData DataLevel;
    public bool EnableNewLevelType = true;
    #endregion

    public Wave targetWavePrefab;
    public List<Wave> gameWavesPrefab;

    public SoundManager sm;

    public int ShiftWaveAmount = 1;

    //[HideInInspector]
    public Wave targetWave, gameWave;

    public static GameController Instance;

    public bool canManualControl = true;

    public SoundManager SoundMngr;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        SoundMngr =  GetComponent<SoundManager>();
        LoadLevelsFile();

        //GameController.Instance.SoundMngr.PlayClip(SoundManager.Clip.Main, false);
        //GameController.Instance.SoundMngr.PlayLayeredMusic(1, 0, 0);
        //GameController.Instance.SoundMngr.PlayLayeredMusic(2, 0, 0);
        //GameController.Instance.SoundMngr.PlayLayeredMusic(3, 0, 0);

        //GameController.Instance.SoundMngr.PlayLayeredMusic(1, 1);
        //GameController.Instance.SoundMngr.PlayLayeredMusic(2, 1);
        //GameController.Instance.SoundMngr.PlayLayeredMusic(3, 1);


    }

    private float rightTime = 0;
    private float leftTime = 0;
    private bool autoRight = false;
    private bool autoLeft = false;
    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space) && canManualControl)
        {
            canManualControl = false;
            targetWave.SumWave(gameWave);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            GameController.Instance.gameWave.ShiftWave(ShiftWaveAmount);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftTime += Time.deltaTime;
            if (leftTime > 0.25f && !autoLeft)
            {
                autoLeft = true;
                StartCoroutine("AutoMoveLeftCO");
            }
        }
        else
        {
            autoLeft = false;
            leftTime = 0;
            StopCoroutine("AutoMoveLeftCO");
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            GameController.Instance.gameWave.ShiftWave(-ShiftWaveAmount);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rightTime += Time.deltaTime;
            if (rightTime > 0.25f && !autoRight)
            {
                autoRight = true;
                StartCoroutine("AutoMoveRightCO");
            }
        }
        else
        {
            autoRight = false;
            rightTime = 0;
            StopCoroutine("AutoMoveRightCO");
        }
    }

    IEnumerator AutoMoveRightCO()
    {
        while (true)
        {
            GameController.Instance.gameWave.ShiftWave(-ShiftWaveAmount);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator AutoMoveLeftCO()
    {
        while (true)
        {
            GameController.Instance.gameWave.ShiftWave(ShiftWaveAmount);
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// Checks the lose.
    /// </summary>
    public void CheckLose(List<int> _elements) {
        // Lose
        if (_elements.FindIndex(b => b != 0) == -1) {
            if (OnLose != null) {
                OnLose();
            }
            Debug.Log("Lose");
        }
    }

    /// <summary>
    /// Checks the win.
    /// </summary>
    public bool CheckWin(List<int> _elements)
    {
        // Lose
        if (_elements.FindIndex(b => b != 0) == -1) {
            if (OnWin != null) {
                OnWin();
            }
            Debug.Log("Win");
            return true;
        }
        return false;
    }

    #region Events

    public delegate void GameFlowEvent();

    public static event GameFlowEvent OnWin;
    public static event GameFlowEvent OnLose;

    #endregion

    #region Level Static Database    
    /// <summary>
    /// Loads the levels file, totally unsafe!
    /// </summary>
    void LoadLevelsFile() {
        TextAsset JsonText = Resources.Load<TextAsset>("LevelsAndPieces/LevelsAndPieces");

        GameController.DataLevel = new LevelsStaticData();
        GameController.DataLevel = JsonUtility.FromJson<LevelsStaticData>(JsonText.text);

    }

    #endregion

}
