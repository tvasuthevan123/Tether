using UnityEngine;

public class GrapplingGun : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera;
    public float maxDistance = 100f;
    
    private SpringJoint joint;
    public float reelAccel = 5f;
    private bool isReeling = false; 

    private Vector3 currentGrapplePosition;

    public GameObject player;
    public Rigidbody playerRb;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        playerRb = player.GetComponent<Rigidbody>();
        StopGrapple();
    }

    void Update()
    {
        if(IsGrappling() && isReeling)
        {
            Debug.Log("Reelings?");
            Vector3 direction = (grapplePoint - player.transform.position).normalized;
            playerRb.AddForce(direction * reelAccel);
        }
        // TODO: Possible refactor using input system onPress?
        // Grapple
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
        
        // Reel button interaction
        if (Input.GetMouseButtonDown(1))
        {
            StartReel();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            StopReel();
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;

            // if(!isReeling)
            //     setFixedGrapple();
        }
    }

    void setFixedGrapple()
    {
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        float distanceFromPoint = Vector3.Distance(player.transform.position, grapplePoint);

        //The distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        // Adjust these values to fit your game.
        joint.spring = 4.5f;
        joint.damper = 0f;
        joint.massScale = 4.5f;
    }

    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    void StartReel()
    {
        isReeling=true;
        Destroy(joint);
    }

    void StopReel()
    {
        isReeling=false;
        // if(IsGrappling())
        //     setFixedGrapple();
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