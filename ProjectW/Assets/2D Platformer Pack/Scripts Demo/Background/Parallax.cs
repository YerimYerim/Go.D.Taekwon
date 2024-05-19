using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Camera cam;
    public float parallaxValue;
    private float originPos;




    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position.x;
 
    }

    // Update is called once per frame
    void Update()
    {
        float newPosition = originPos + cam.transform.position.x * parallaxValue;

        transform.position = new Vector2(newPosition, transform.position.y);

    }
}

