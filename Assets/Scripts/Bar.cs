using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public SpriteRenderer barSprite;

    public List<GameObject> cubes = new List<GameObject>();
    public GameObject cubePrefab;

    public void Initialise(int maxHeight)
    {
        cubes.Clear();
        for (int i = 0; i < maxHeight; i++)
        {
            var go = Instantiate(cubePrefab);
            go.transform.SetParent(this.transform);
            go.transform.localPosition = Vector3.up * i;
            go.transform.localScale = new Vector3(1.7f, 0.8f, 0.1f);
            cubes.Add(go);
            go.SetActive(false);
        }

    }


    public void SetHeight(int h)
    {
        int abs_h = Mathf.Abs(h);

        for (int i = 0; i < abs_h; i++)
        {
            //Debug.Log(i);
            cubes[i].gameObject.SetActive(true);
            cubes[i].transform.localPosition = Vector3.up * Mathf.Sign(h) * i;
            if (Mathf.Sign(h) < 0) cubes[i].transform.localPosition += Vector3.up * (-1);
        }

        for (int i = abs_h; i < cubes.Count; i++)
        {
            // Debug.Log(i);
            cubes[i].gameObject.SetActive(false);
        }

        /*var size = this.transform.localScale;
        size.y = windowHeights[i];
        this.transform.localScale = size;*/
     }

    public void SetColor(Color minColor, Color maxColor)
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            var tmpColor = Color.Lerp(minColor, maxColor, i * 1f/ cubes.Count);
            cubes[i].GetComponent<MeshRenderer>().material.color = tmpColor;
        }

        //foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        //    meshRenderer.material.color = color;
    }
}
