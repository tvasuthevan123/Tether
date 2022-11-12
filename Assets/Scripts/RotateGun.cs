using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateGun : MonoBehaviour
{
    public GrapplingGun grappling;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!grappling.IsGrappling()) return;
        transform.LookAt(grappling.GetGrapplePoint());
  
    }
}
