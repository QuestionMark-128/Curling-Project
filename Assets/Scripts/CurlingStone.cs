using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class CurlingStone : MonoBehaviour
{
    // UI

    private TextMeshProUGUI statslbl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private const float MAX_FRICTION = 0.5f;
    private const float MIN_POWER = 5f;
    private const float MAX_POWER = 100f;
    private Collider baseCollider;
    private Collider bodyCollider;
    private float desired_acceleration_x;
    private float desired_acceleration_y;

    // throw data
    private float magnitude;
    private Vector2 direction;
    private float curl; // positive for right, negative for left

    // data for the game manager
    
    private bool stopped; // flag for when the stone has stopped moving
    private Team team; // mostly used by the curling sheet for scoring
    private GameManager gameManager;

    [SerializeField] private Rigidbody rb;

    private bool isThrown = false;
    private bool isCharging = false;

    // Mesh info

    public Material[] redHandleMaterials;
    public Material[] blueHandleMaterials;
    public Material redTopMaterial;
    public Material blueTopMaterial;
    public MeshRenderer topMesh;
    public MeshRenderer handleMesh;

    int stop_timer = 0;
    bool inScoreZone = false;
    Vector3 button_position;

    void Awake()
    {
        //rb = GetComponent<Rigidbody>();

    }
    void Start()
    {
        

        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders) {
            if (col.gameObject.name == "CurlingStone_Col_Base") {
                baseCollider = col;
            }
            else if (col.gameObject.name == "CurlingStone_Col_Body") {
                bodyCollider = col;
            }
        }

        baseCollider.material = new PhysicsMaterial("BaseMaterial");
        bodyCollider.material = new PhysicsMaterial("BodyMaterial");
        
        // should range from 0.5 to 0.02

        baseCollider.material.dynamicFriction = MAX_FRICTION; // change at some point
        baseCollider.material.staticFriction = MAX_FRICTION;

        baseCollider.material.frictionCombine = PhysicsMaterialCombine.Multiply;
        
        // probably add some base value for the friction here        

        desired_acceleration_x = 0;
        desired_acceleration_y = 0;  

        stopped = true;
    }


    public void AimingPhase() // called every frame during the Aiming game phase
    {
        
        // add logic for moving the mouse and adjusting the parameters
        // x is length, z is width
        //Debug.Log("Stone Aiming");
        // AIMING 
        direction.x = -1f; 
        direction.y = (Mouse.current.position.x.value - Screen.width / 2) / 1500;

        // CURLING
        if (Keyboard.current.qKey.isPressed)
        {
            curl -= 0.01f;
        }
        else if (Keyboard.current.eKey.isPressed)
        {
            curl += 0.01f;
        }

        // CHARGING
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isCharging = true;
            magnitude = MIN_POWER;
        }
        if (isCharging && Keyboard.current.spaceKey.isPressed)
        {
            magnitude += Time.deltaTime * 20f;
            magnitude = Mathf.Clamp(magnitude, MIN_POWER, MAX_POWER);
        }
        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            isCharging = false;
         
            Debug.Log("Magnitude: " + magnitude + ", Curl: " + curl + ", Direction (y): " + direction.y);
            Throw();
        }
    }

    public void SweepingPhase() // called every frame during the Sweeping game phase
    {   
        if (stop_timer < 1000)
        {
            stop_timer++;
        }
        if (rb.linearVelocity.magnitude < 0.01f && !stopped && stop_timer >= 1000)
        {
            stopped = true;
            gameManager.OnStoneStopped(inScoreZone, button_position); // 
            isThrown = false;
        }
    }

    // Update is called once per frame
    void Update() // There might be things that are always updated?
    {
        statslbl.text = string.Format("Magnitude: " + magnitude + ", Curl: " + curl + ", Direction (y): " + direction.y +
        "\nStopped: " + stopped + ", Thrown: " + isThrown + "\nx: " + transform.position.x + ", y: " + transform.position.y);

    }
    
    public void Throw()
    {
        if (isThrown) return;

        isThrown = true;

        Vector3 moveDir = new Vector3(direction.x, 0, direction.y);
        Vector3 curlDir = new Vector3(0,curl,0);
        rb.AddForce(moveDir * magnitude, ForceMode.Impulse);
        rb.AddTorque(curlDir, ForceMode.Impulse);
        stopped = false;
        stop_timer = 0;
    }

    public void Initialize(Team t, GameManager g)
    {
        team = t;
        gameManager = g;
        if (team == Team.Red)
        {
            topMesh.material = redTopMaterial;
            handleMesh.materials = redHandleMaterials;
        }
        else if (team == Team.Blue)
        {
            topMesh.material = blueTopMaterial;
            handleMesh.materials = blueHandleMaterials;
        }
    }
    void OnMove(InputValue action) { // will get overwritten
        var movement = action.Get<Vector2>();
        desired_acceleration_x = movement.x;
        desired_acceleration_y = movement.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NearButton") && team == Team.Blue)
        {
            inScoreZone = true;
            button_position = other.transform.position;

        }
        else if (other.CompareTag("FarButton") && team == Team.Red)
        {
            inScoreZone = true;
            button_position = other.transform.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NearButton") && team == Team.Blue)
        {
            inScoreZone = false;
        }
        else if (other.CompareTag("FarButton") && team == Team.Red)
        {
            inScoreZone = false;
        }
    }

    public void SetIceFriction(float f) {
        baseCollider.material.dynamicFriction = f; // should obviously be changed later
        baseCollider.material.staticFriction = f;
    }
    public bool getIsThrown()
    {
        return isThrown;
    }
    public Team getTeam()
    {
        return team;
    }

    public void setText(TextMeshProUGUI t)
    {
        statslbl = t;
    }
    
}
