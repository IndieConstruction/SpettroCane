using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AutoAnimator : MonoBehaviour
{
    public Animator animator;

    public int MinAppearTime = 10;
    public int MaxAppearTime = 15;

    public int MinReappearTime = 5;
    public int MaxReappearTime = 7;

    void Awake ()
    {
        animator.Stop();
	}

    IEnumerator AppearCO()
    {
        animator.Stop();
        yield return new WaitForSeconds(Random.Range(MinAppearTime, MaxAppearTime));
        //animator.Play();
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinReappearTime, MaxReappearTime));
            // animator.Play();
        }
    }

}
