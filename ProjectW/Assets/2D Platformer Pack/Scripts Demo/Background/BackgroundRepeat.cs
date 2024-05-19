using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeat : MonoBehaviour
{
    private float length;
    private float height;
    private float originPos;

    [SerializeField] Camera cam;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] float parallax;

    
    void Awake()
    {

        length = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;

        originPos = (gameObject.transform.position.x + spawnPoint.transform.position.x);
        
        transform.position = new Vector2(originPos, transform.position.y);

    }

    void Update()
    {
        
        Vector2 position = cam.transform.position;

        float parallaxTemp = position.x * (1 - parallax);
        float distance = position.x * parallax;

        transform.position = new Vector2(originPos + distance, transform.position.y);

        if (parallaxTemp > originPos + length/2)
        {
            originPos += length;
        }

        else if (parallaxTemp < originPos - length/2)
        {  
            originPos -= length;
        }

    }
}
