using UnityEngine;
using DG.Tweening;

public class DGMovePosition : MonoBehaviour {

    public Vector3 end;
    public float dur;
    public Ease ease;
    public bool doOnce = false;

	void OnEnable ()
    {
        transform.DOMove(end, dur).SetRelative(true).SetEase(ease).OnComplete(() => this.enabled = doOnce).SetLoops( doOnce ? 2 : -1, LoopType.Yoyo);
	}
	
}
