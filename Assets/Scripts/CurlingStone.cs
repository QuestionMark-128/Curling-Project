using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CurlingStone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private float desired_acceleration_x;
    private float desired_acceleration_y;

    void Start()
    {
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

}
