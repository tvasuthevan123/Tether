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
    }

    void Update()
    {
        if(IsGrappling() && isReeling)
        {
            // reelAmount = reelAccel*reelAmount;
            // joint.minDistance =- reelAmount;
            // Debug.Log("Reel Amount = " + reelAmount);
            // Debug.Log("Joint Max Distance = " + joint.minDistance);
            Vector3 direction = Vector3.MoveTowards(player.transform.position, grapplePoint, reelAccel * Time.deltaTime);
            playerRb.MovePosition(direction);
            playerRb.useGravity = false;
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
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.transform.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
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
    }

    void StopReel()
    {
        isReeling=false;
        // playerRb.velocity = Vector3.zero;
        playerRb.useGravity = true;
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}