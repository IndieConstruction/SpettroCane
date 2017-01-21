using UnityEngine;
using DG.Tweening;

public class DGPunchScale : MonoBehaviour {

    public Vector3 end;
    public float dur;
    public Ease ease;

	void OnEnable ()
    {
        transform.DOPunchScale(end, dur).SetEase(ease).OnComplete(() => this.enabled = false);
	}
	
}
