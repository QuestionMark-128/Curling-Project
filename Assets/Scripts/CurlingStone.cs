using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CurlingStone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Collider baseCollider;
    private Collider bodyCollider;
    private float desired_acceleration_x;
    private float desired_acceleration_y;

    // throw data
    private float magnitude;
    private Vector2 direction;
    private float curl; // positive for right, negative for left
    
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
        
        bodyCollider.material.dynamicFriction = 0.3f; // change at some point
        bodyCollider.material.staticFriction = 0.3f;
        
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

        GetComponent<Rigidbody>().AddRelativeForce(desired_acceleration_x * 5, 0, desired_acceleration_y * 5);
        
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
