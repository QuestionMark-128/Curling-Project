//using System.Numerics;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Transform target;
    private Vector3 offset;

    void Start()
    {
        //transform.rotation = offset.rotation;
        //
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }

    public void setTarget(Transform t, int r)
    {
        if (r % 2 == 0)
        {
            offset = new Vector3(-2, 2f, 0); // 2 behind, one above, and in line
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else 
        {
           
           offset = new Vector3(2, 2f, 0); // 2 behind, one above, and in line
           transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        target = t;
    }

}