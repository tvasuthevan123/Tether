using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrapplingGun : MonoBehaviour
{
    public LayerMask whatIsGrappleable;
    public Transform gunTip, cameraobject, crosshair;
    public float maxDistance = 100f;
    public float reelAccel = 5f;
    public GameObject player;
    private Rigidbody playerRb;
    private bool canGrapple;
    private bool isReeling = false; 
    private SpringJoint joint;
    private Vector3 currentGrapplePosition;
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private RaycastHit hit;
    private List<GameObject> crosshairTips;

    public AudioSource grappleFire;

    public AudioSource grappleReel;
    public bool isTooltipEnabled;
    public TMP_Text[] tooltips;
    private int activeTooltip=0;
    private bool attemptedInvalidGrapple=false;
    private enum crosshairState
    {
        cannotGrapple,
        canGrapple,
        isGrappling,
        isReeling
    }

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        playerRb = player.GetComponent<Rigidbody>();
        StopGrapple();

        crosshairTips = new List<GameObject>();
        foreach(Transform crosshairTipTransform in crosshair)
        {
            crosshairTips.Add(crosshairTipTransform.gameObject);
        }
        Debug.Log(crosshairTips);
    }

    void Update()
    {
        if(PauseMenu.isGamePaused)
            return;
        bool canGrappleCheck = Physics.Raycast(cameraobject.position, cameraobject.forward, out hit, maxDistance, whatIsGrappleable);
        if(canGrappleCheck && !IsGrappling())
        {
            canGrapple = true;
            SetCrosshair(crosshairState.canGrapple);
        }
        else if(!canGrappleCheck && !IsGrappling())
        {
            canGrapple = false;
            SetCrosshair(crosshairState.cannotGrapple);
        }
        else if(IsGrappling())
        {
            SetCrosshair(crosshairState.isGrappling);
        }

        if(!isReeling)
        {
            playerRb.AddForce(Vector3.down * reelAccel);
            grappleReel.Stop();    
        }
        if(IsGrappling() && isReeling)
        {
            Vector3 direction = (grapplePoint - player.transform.position).normalized;
            playerRb.AddForce(direction * reelAccel * 2);
            SetCrosshair(crosshairState.isReeling);
            if(!grappleReel.isPlaying)
            {
                grappleReel.time = 0.25f;
                grappleReel.Play();
            }
            if(grappleReel.isPlaying && grappleReel.time > 2.5f)
            {
                grappleReel.time = 0.3f;
            }
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (!Input.GetMouseButton(0))
        {
            StopGrapple();
        }
        
        // Reel button interaction
        if (Input.GetMouseButtonDown(1))
        {
            if(IsGrappling())
            {
                StartReel();
            }
            
        }
        else if(!Input.GetMouseButton(1))
        {
            StopReel();
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void SetCrosshair(crosshairState state)
    {
        Color crosshairColor = new Color(0,0,0);
        switch(state)
        {
            case crosshairState.cannotGrapple:
                crosshairColor = new Color(255,0,0);
                break;
            case crosshairState.canGrapple:
                crosshairColor = new Color(0,255,0);
                break;
            case crosshairState.isGrappling:
                crosshairColor = new Color(0,255,241);
                break;
            case crosshairState.isReeling:
                crosshairColor = new Color(255,132,0);
                break;
        }
        foreach(GameObject crosshairTip in crosshairTips)
        {
            crosshairTip.GetComponent<Image>().color = crosshairColor;
        }

        if(isTooltipEnabled)
        {
            tooltips[activeTooltip].gameObject.SetActive(false);
            int tooltipId = 0;
            if(attemptedInvalidGrapple)
                tooltipId=4;
            switch(state)
            {
                case crosshairState.canGrapple:
                    tooltipId=1;
                    attemptedInvalidGrapple=false;
                    break;
                case crosshairState.isGrappling:
                    tooltipId=2;
                    attemptedInvalidGrapple=false;
                    break;
                case crosshairState.isReeling:
                    tooltipId=3;
                    break;
            }
            tooltips[tooltipId].gameObject.SetActive(true);
            activeTooltip = tooltipId;
        }
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        if (canGrapple)
        {
            grappleFire.Play();
            grapplePoint = hit.point;
            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
            SetCrosshair(crosshairState.isGrappling);
            // if(!isReeling)
            //     setFixedGrapple();
        }
        else if(isTooltipEnabled){
            tooltips[activeTooltip].gameObject.SetActive(false);
            tooltips[4].gameObject.SetActive(true);
            activeTooltip=4;
            attemptedInvalidGrapple=true;
        }
    }

    void setFixedGrapple()
    {
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        float distanceFromPoint = Vector3.Distance(player.transform.position, grapplePoint);

        //The distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.9f ;
        // joint.minDistance = distanceFromPoint ;

        // Adjust these values to fit your game.
        joint.spring = 20f;
        joint.damper = 1f;
        joint.massScale = 45f;
    }

    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    public void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        
    }

    void StartReel()
    {
        isReeling=true;
        Destroy(joint);
        playerRb.mass = 10;
    }

    public void StopReel()
    {
        isReeling=false;
        // if(IsGrappling())
        //     setFixedGrapple();
        playerRb.mass = 20; 
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!IsGrappling()) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return lr.positionCount == 2;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}