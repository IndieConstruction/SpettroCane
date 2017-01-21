using UnityEngine;
using DG.Tweening;

public class DGMoveRotation : MonoBehaviour {

    public Vector3 end;
    public float dur;
    public Ease ease;
    public bool doOnce = false;

	void OnEnable ()
    {
        transform.DORotate(end, dur).SetRelative(true).SetEase(ease).OnComplete(() => this.enabled = doOnce).SetLoops( doOnce ? 2 : -1, LoopType.Yoyo);
	}
	
}
