using UnityEngine;
using DG.Tweening;

public class DGPunchRotation : MonoBehaviour {

    public Vector3 end;
    public float dur;
    public Ease ease;

	void OnEnable ()
    {
        transform.DOPunchRotation(end, dur).SetRelative(true).SetEase(ease).OnComplete(() => this.enabled = false);
	}
	
}
