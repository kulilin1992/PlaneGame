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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += scrollerVelocity * Time.deltaTime;
    }
}
