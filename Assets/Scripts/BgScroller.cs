using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScroller : MonoBehaviour
{

    [SerializeField] Vector2 scrollerVelocity;
    Material material;
    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver)
        {
            material.mainTextureOffset += scrollerVelocity * Time.deltaTime;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //material.mainTextureOffset += scrollerVelocity * Time.deltaTime;
    }
}
