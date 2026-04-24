using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class CurlingStone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private const float MAX_FRICTION = 0.5f;
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
    private bool team; // true for team 1, false for team 2
    [SerializeField] private GameManager gameManager;

    
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
    }

    // Update is called once per frame
    void Update()
    {

        float dx = (Mouse.current.position.x.value - Screen.width / 2) / 1500;

        if (Mathf.Abs(dx) > 0.01f) {
        
            transform.Rotate(0,dx,0);
        }

        GetComponent<Rigidbody>().AddRelativeForce(desired_acceleration_x * 10, 0, desired_acceleration_y * 10);
        if (GetComponent<Rigidbody>().linearVelocity.magnitude < 0.01f)
        {
            stopped = true;
            gameManager.OnStoneStopped(this);
        }
    }

    void OnMove(InputValue action) {
        var movement = action.Get<Vector2>();
        desired_acceleration_x = movement.x;
        desired_acceleration_y = movement.y;
    }
    
    public void SetIceFriction(float f) {
        baseCollider.material.dynamicFriction = f; // should obviously be changed later
        baseCollider.material.staticFriction = f;
    }

}
