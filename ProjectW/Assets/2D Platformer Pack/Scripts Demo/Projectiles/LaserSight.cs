using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    private float playerDirection;

    PlayerController playerController;
    Animator playerAnim;

    public Camera mainCam;
    [HideInInspector] public Vector2 mousePos;
    [HideInInspector] public Vector2 mouseDirection;

    [HideInInspector] public LineRenderer line;
    [HideInInspector] public Vector2 laserDirection;
    Vector2 inputDirection;
    public float laserDistance;
    public Transform originPos;
    public Transform aimPivot;
    public LayerMask ignore;


    void Start()
    {

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        playerAnim = GameObject.Find("Player").GetComponent<Animator>();

        line = gameObject.GetComponent<LineRenderer>();

        line.enabled = false;

    }


    void Update()
    {

        playerDirection = playerController.transform.localScale.x;

        //Joystick Aiming 360
        if (playerController.aimingJoy)
        {

            inputDirection = new Vector2(playerController.horizontalInput, playerController.verticalInput);

            if (inputDirection == Vector2.zero)
            {
                laserDirection = Vector2.right * playerDirection;

            }
            else
            {
                laserDirection = inputDirection;
            }

            //Rotates origin point of aiming using the gameobject aimPivot as a pivot point
            if (playerDirection == -1)
            {
                aimPivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-playerController.verticalInput, -playerController.horizontalInput) * 180 / Mathf.PI);

                if (inputDirection == Vector2.zero)
                {
                    aimPivot.transform.Rotate(0, 0, 180);
                }
            }
            else
            {
                aimPivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(playerController.verticalInput, playerController.horizontalInput) * 180 / Mathf.PI);
            }


            //Aim laser sight positions and raycast
            RaycastHit2D hit = Physics2D.Raycast(originPos.position, laserDirection.normalized, laserDistance, ~ignore);

            line.SetPosition(0, originPos.position);

            if (hit)
            {
                line.SetPosition(1, hit.point);

            }

            else if (!hit)
            {
                line.SetPosition(1, new Vector2(originPos.position.x, originPos.position.y) + (laserDirection.normalized * laserDistance));
            }

        }


        //Keyboard and mouse aiming 360
        if (playerController.aimingKB)
        {

            Vector3 pos = Input.mousePosition;
            pos.z = Camera.main.nearClipPlane;

            mousePos = mainCam.ScreenToWorldPoint(pos);

            Vector2 aimPos = new Vector2(aimPivot.transform.position.x, aimPivot.transform.position.y);

            mouseDirection = mousePos - aimPos;


            if (playerDirection == -1)
            {
                aimPivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-mouseDirection.y, -mouseDirection.x) * 180 / Mathf.PI);
            }
            else
            {
                aimPivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(mouseDirection.y, mouseDirection.x) * 180 / Mathf.PI);
            }


            //Aim laser sight positions and raycast
            RaycastHit2D hit = Physics2D.Raycast(originPos.position, mouseDirection, laserDistance, ~ignore);
            Debug.DrawLine(originPos.position, mousePos, Color.green);

            line.SetPosition(0, originPos.position);

            if (hit)
            {
                line.SetPosition(1, hit.point);
            }

            else if (!hit)
            {
                line.SetPosition(1, new Vector2(originPos.position.x, originPos.position.y) + (mouseDirection.normalized * laserDistance));
            }
        }

        //Enables and disables laser sight based on when the aiming animation blend tree is active
        if (playerAnim.GetCurrentAnimatorStateInfo(1).IsName("Gun 360 Blend"))
        {
            if (!line.enabled)
            {
                line.enabled = true;
            }
        }
        else if (line.enabled)
        {
            line.enabled = false;
        }

    }

}
