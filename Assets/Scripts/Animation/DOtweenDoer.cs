using UnityEngine;
using DG.Tweening;

public class DOtweenDoer : MonoBehaviour {

    public Vector3 end;
    public float dur;
    public Ease ease;

	void OnEnable ()
    {
       // transform.DORotate(end, dur).SetEase(ease);
        transform.DOPunchRotation(end, dur).OnComplete(() => this.enabled = false);
	}
	
}
