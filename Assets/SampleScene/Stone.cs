using UnityEngine;

public class Stone : MonoBehaviour
{
    public float curlStrength = 1.5f;
    public float spinDirection = 1f; // -1 left, +1 right

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        if (velocity.magnitude > 0.1f)
        {
            // perpendicular force = curling effect
            Vector3 sideForce = Vector3.Cross(Vector3.up, velocity.normalized);

            rb.AddForce(sideForce * curlStrength * spinDirection, ForceMode.Acceleration);
        }
    }
}