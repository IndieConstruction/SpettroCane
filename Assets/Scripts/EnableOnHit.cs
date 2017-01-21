using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnHit : MonoBehaviour {

    public MonoBehaviour target;

    void OnMouseUpAsButton()
    {
        target.enabled = true;
    }
}
