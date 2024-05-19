using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    private PlayerController playerController;

    public float cameraSmoothingX;
    public float cameraSmoothingY;
    public bool smoothCamOnDashSlide;
    public float smoothAmountDashSlide = 0.3f;
    private float camSmoothOGX;
    private bool tempSmooth = false;

    private float timeElapsed;
    private float lerpDuration = 1f;


    private Vector3 currentVelocity =  Vector3.zero;


    [SerializeField] float camOffsetX;
    [SerializeField] float camOffsetY;
    [SerializeField] float camOffsetZ;


    void Start()
    {
        //Ensures Camera is at player position + offset on Start
        gameObject.transform.position = player.transform.position + new Vector3(camOffsetX, camOffsetY, camOffsetZ);

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        camSmoothOGX = cameraSmoothingX;
    }

 
    void LateUpdate()
    {

        //Updates camera position in relation to player object set in Inspector
        Vector3 playerPos = player.transform.position + new Vector3(camOffsetX, camOffsetY, camOffsetZ);

        //General camera smoothing in X and Y can be set in Inspector
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(playerPos.x, transform.position.y, transform.position.z), ref currentVelocity, cameraSmoothingX);
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, playerPos.y, transform.position.z), ref currentVelocity, cameraSmoothingY);
    }


    private void Update()
    {
        //Gives Camera smoothing on Dash or Slide
        if ((playerController.isDashing || playerController.isSliding) && !tempSmooth && smoothCamOnDashSlide)
        {
            tempSmooth = true;
        }

        if (tempSmooth)
        {
            cameraSmoothingX = smoothAmountDashSlide;

            if (timeElapsed < lerpDuration)
            {        
                cameraSmoothingX = Mathf.Lerp(smoothAmountDashSlide, camSmoothOGX, timeElapsed / lerpDuration);
            
                timeElapsed += Time.deltaTime;
 

                if (timeElapsed > lerpDuration)
                {
                    tempSmooth = false;
                    cameraSmoothingX = camSmoothOGX;
                    timeElapsed = 0;
                }
            }

        }
    }


}
