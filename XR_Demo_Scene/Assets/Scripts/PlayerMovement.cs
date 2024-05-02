using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    // store the right foot for kicking actions
    public GameObject RightShoe;
    public Vector3 PenaltyPosition;
    public Quaternion PenaltyRotation;
    public Vector3 PenaltyBallPosition;
    public Vector3 FreeKickPosition;
    public Quaternion FreeKickRotation;
    public Vector3 FreeKickBallPosition;
    

    // Start is called before the first frame update
    protected Animator animator;
    protected int isWalkingHash;
    protected int isRunningHash;
    protected int isLaceKickingHash;
    protected int isSideKickingHash;

    // Init player input
    PlayerInput input;

    // variable to store player input
    Vector2 currentMovement;
    protected bool movementPressed;
    protected bool runPressed;
    protected bool sideKickPressed;
    protected bool laceKickPressed;
    protected bool randomisePressed;
    protected bool targetDecreasePressed;
    protected bool moveTargetPressed;
    /*protected bool targetIncreasePressed;*/
    protected bool targetSpeedIncreasePressed;
    protected bool targetSpeedDecreasePressed;

    protected bool zoomInPressed;
    protected bool zoomOutPressed;
    protected bool slowGamePressed;
    private float slowdownFactor = 0.75f;
    private float currentTimeScale = 1;

    // string to store restart button pressed
    bool restartPressed;

    // varied distance bool
    bool setPiece1Pressed;
    bool setPiece2Pressed;

    // reference to ball and camera
    public GameObject ball;
    public Rigidbody ballRigidBody;

    // reference to the goal targets
    public TargetController targetController;

    // reference to main camera
    public Camera mainCamera;
    
    // Awake is called when script is being loaded
    private void Awake()
    {
        input = new PlayerInput();

        input.CharacterControls.IncreaseTargetMovement.performed += ctx =>
        {
            targetSpeedIncreasePressed = ctx.ReadValueAsButton();
        };

        input.CharacterControls.DecreaseTargetMovement.performed += ctx =>
        {
            targetSpeedDecreasePressed = ctx.ReadValueAsButton();
        };

        input.CharacterControls.Restart.performed += ctx =>
        {
            restartPressed = ctx.ReadValueAsButton();
        };

        // input for changing distances
        input.CharacterControls.SetPiece1.performed += ctx =>
        {
            Debug.Log("pressed setpiece1");
            setPiece1Pressed = ctx.ReadValueAsButton();

        };

        input.CharacterControls.SetPiece2.performed += ctx =>
        {
            setPiece2Pressed = ctx.ReadValueAsButton();
        };

        // input for altering targets

        input.CharacterControls.MoveTarget.performed += ctx =>
        {
            moveTargetPressed = ctx.ReadValueAsButton();
        };
        
        input.CharacterControls.RandomiseTarget.performed += ctx =>
        {
            randomisePressed = ctx.ReadValueAsButton();
        };
        
        input.CharacterControls.DecreaseTargetSize.performed += ctx =>
        {
            targetDecreasePressed = ctx.ReadValueAsButton();
        };

        input.CharacterControls.Zoomin.performed += ctx =>
        {
            zoomInPressed = ctx.ReadValueAsButton();
        };
        input.CharacterControls.Zoomout.performed += ctx =>
        {
            zoomOutPressed = ctx.ReadValueAsButton();
        };
        
        input.CharacterControls.SlowGame.performed += ctx =>
        {
            slowGamePressed = ctx.ReadValueAsButton();
        };
    }
    void Start()
    {
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        // call handlers for all inputs
        handleRotation();
        
        handleShoot();
        handleRestart();
        HandleInitialPosition();
        handleRandomise();
        handleTargetSizeDecrease();
        handleTargetMovement();
        /*handleTargetSizeIncrease();*/

        handleTargetSpeed();
        handleZoom();
        //handleSlowGame();
        

    }

    void handleRestart()
    {
        if (restartPressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            resetTimeScale();
            
        }
    }

    protected virtual void handleShoot()
    {
        // TODO implement handleshoot
    }
    
    void handleRotation()
    {
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);

        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
    }
   

    private void HandleInitialPosition()
    {
        if (setPiece1Pressed)
        {
           
            ball.transform.position = FreeKickBallPosition;
            ballRigidBody.velocity = new Vector3(0, 0, 0);
            transform.position = FreeKickPosition;
            transform.rotation = FreeKickRotation;
            
            setPiece1Pressed = false;
        }

        if (setPiece2Pressed)
        {
            ball.transform.position = PenaltyBallPosition;
            ballRigidBody.velocity = new Vector3(0, 0, 0);
            transform.localPosition = PenaltyPosition;
            transform.rotation = PenaltyRotation;
            setPiece2Pressed = false;
        }

    }
    
    void handleRandomise()
    {
        // Do the ranodmising here
        
        if (randomisePressed)
        {
            targetController.randomiseTargets();
            randomisePressed = false;
        }
    }

    void handleTargetSpeed()
    {
        if (targetSpeedIncreasePressed)
        {
            targetController.increaseTargetMovementSpeed();
            targetSpeedIncreasePressed = false;
        }

        if (targetSpeedDecreasePressed)
        {
            targetController.decreaseTargetMovementSpeed(); 
            targetSpeedDecreasePressed = false;
        }
    }


    void handleTargetMovement()
    {
        if (moveTargetPressed)
        {
            Debug.Log("MoveTarget");
            targetController.moveTarget();
        }
    }
    
    void handleTargetSizeDecrease()
    {
        // Do the target decreasing here
        if (targetDecreasePressed)
        {
            
            targetController.ShrinkTargets();
            targetDecreasePressed = false;
        }
        
    }
    /*void handleTargetSizeIncrease()
    {
        // Do the target decreasing here
        if (targetIncreasePressed)
        {
            
            targetController.ExpandTargets();
            targetIncreasePressed= false;
        }
        
    }*/

    private void OnEnable()
    {
        // enable character action map
        input.CharacterControls.Enable();

    }
    

    private void OnDisable()
    {
        // enable character action map
        input.CharacterControls.Disable();
    }

    void handleZoom()
    {
        if (zoomInPressed)
        {
            Debug.Log("Zoom in");

            mainCamera.fieldOfView = 40;
            zoomInPressed = false;
        }

        if (zoomOutPressed)
        {
            Debug.Log("Zoom Out");
            mainCamera.fieldOfView = 80;
            zoomOutPressed = false;
        }
    }

    private void setTimeScale()
    {
        currentTimeScale = slowdownFactor * currentTimeScale;
        Debug.Log(slowdownFactor);
        Time.timeScale = currentTimeScale;
        Debug.Log(currentTimeScale);
        Time.fixedDeltaTime = currentTimeScale * 0.02f;
    }

    private void resetTimeScale()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    
    /*void handleSlowGame()
    {
        if (slowGamePressed)
        {
            Debug.Log("slow Game");

            setTimeScale();
            slowGamePressed = false;
        }
    }*/
}


