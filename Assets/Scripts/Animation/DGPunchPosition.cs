using UnityEngine;
using DG.Tweening;

public class DGPunchPosition : MonoBehaviour {

    public Vector3 end;
    public float dur;
    public Ease ease;

	void OnEnable ()
    {
        transform.DOPunchPosition(end, dur).SetRelative(true).SetEase(ease).OnComplete(() => this.enabled = false);
	}
	
}
