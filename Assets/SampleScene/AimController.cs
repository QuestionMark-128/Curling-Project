using UnityEngine;

public class AimController : MonoBehaviour
{
    public float rotateSpeed = 80f;

    void Update()
    {
        float input = 0;

        if (Input.GetKey(KeyCode.A))
            input = -1;

        if (Input.GetKey(KeyCode.D))
            input = 1;

        transform.Rotate(Vector3.up * input * rotateSpeed * Time.deltaTime);
    }
}