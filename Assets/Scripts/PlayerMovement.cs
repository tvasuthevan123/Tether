using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    public Transform respawnPoint;

    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

   

   
    public float maxSlopeAngle = 35f;

  
    private Vector3 playerScale;

  

    //Input
    float x, y;
   

  
    public AudioSource pickupSound;
    public AudioSource respawn;
    public AudioSource finishLevel;
    public Timer timer;
    public SceneFader sceneFader;
    public String levelName;
    public String nextLevel;
    
    public GrapplingGun grappleGun;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCam.transform.rotation = respawnPoint.rotation;
        orientation.transform.rotation = respawnPoint.rotation;
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().LSStopMusic();
        Time.timeScale = 1f;
    }


    private void FixedUpdate()
    {
        if(PauseMenu.isGamePaused)
            return;
        Movement();
    }

    private void Update()
    {
        if(PauseMenu.isGamePaused)
            return;
        MyInput();
        Look();
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().IGMPlayMusic();
        
    }


    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }


    private void Movement()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;
        
    }

    


    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

 
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish")) 
        {
            finishLevel.Play();
            LevelComplete.levelName = levelName;
            LevelComplete.nextLevel = nextLevel;
            LevelComplete.timeTaken = timer.timeElapsed;
            sceneFader.FadeTo("LevelComplete");
            Debug.Log("Unlocking level: " + Mathf.Max(SceneManager.GetActiveScene().buildIndex+1, PlayerPrefs.GetInt("levelReached", 1)));
            PlayerPrefs.SetInt("levelReached", Mathf.Max(SceneManager.GetActiveScene().buildIndex+1, PlayerPrefs.GetInt("levelReached", 1)));
        }
        if (other.gameObject.CompareTag("Respawn"))
        {
            respawn.Play();
            // Death Flash Overlay
            transform.position = respawnPoint.position;
            playerCam.transform.rotation = respawnPoint.rotation;
            orientation.transform.rotation = respawnPoint.rotation;
            grappleGun.StopReel();
            grappleGun.StopGrapple();
            

            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        }
        if  (other.gameObject.CompareTag("Rewind"))
        {
            //TIME REDUCTION HERE
            pickupSound.Play();
            timer.ReduceTime(5f);
            Destroy(other.gameObject);
        }
    }
    

}