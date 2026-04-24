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

    public void setTarget(Transform t, Team team)
    {
        if (team == Team.Red)
        {
            offset = new Vector3(2, 1, 0); // 2 behind, one above, and in line
        }
        else if (team == Team.Blue)
        {
            offset = new Vector3(-2, 1, 0); // 2 behind, one above, and in line
        }
        target = t;
    }

}