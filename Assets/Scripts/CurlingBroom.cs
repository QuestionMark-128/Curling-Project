using UnityEngine;
using UnityEngine.InputSystem;

public class CurlingBroom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private LayerMask iceLayerMask;
    private CurlingStone stone;

    private bool hidden = false;
    private bool active = false;

    private const float BASE_HEIGHT = 0.25f;
    private const float MAX_FRICTION = 0.5f;
    private float height_adjust = 0;

    private int time_since_lifted = 0;
    private float total_amount_brushed = 0;
    private Vector3 lastBroomPos;


    void Start()
    {
        lastBroomPos = transform.position;
    }


    public void AimingPhase()
    {
        // put it somewhere that can't be seen
        //Debug.Log("Broom Aiming");
        if (!hidden)
        {
            hidden = true;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
        
        
    }

    public void SweepingPhase()
    {
        
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, iceLayerMask))
        {
            transform.position = hit.point + Vector3.up * (BASE_HEIGHT - height_adjust);
            if (hidden)
            {
                hidden = false;
                GetComponent<MeshRenderer>().enabled = true;
                GetComponent<BoxCollider>().enabled = true;
            }
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                height_adjust = 0;
            }
            if (Mouse.current.leftButton.isPressed)
            {
                if (height_adjust < 0.1f)
                {
                    height_adjust += 0.01f;
                }
                else
                {
                    active = true;
                }
                
            }
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                active = false;
                height_adjust = 0;  
            }

            frictionAdjust();
        }
        else
        {
            if (!hidden)
            {
                hidden = true;
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
            }
            
        }


    }
 
    void frictionAdjust()
    {
        Vector3 stonePos = stone.transform.position;
        Vector3 broomPos = transform.position;

        float deltaX = Vector3.Distance(stonePos, broomPos);

        if (active)
        {
            time_since_lifted = 0;
            total_amount_brushed += Vector3.Distance(transform.position, lastBroomPos);
        }
        else
        {
            if (time_since_lifted < 2000)
            {
                time_since_lifted++;
            }
            if (total_amount_brushed > 0)
            {
                total_amount_brushed -= 0.1f;
            }

        }

        float newF = MAX_FRICTION - (total_amount_brushed - deltaX - time_since_lifted);
        if (newF < 0)
        {
            newF = 0;
        }
        if (newF > MAX_FRICTION)
        {
            newF = MAX_FRICTION;
        }
        Debug.Log("Friction reduction: " + newF);
        Debug.Log("Total brushed: " + total_amount_brushed + ", delta x: " + deltaX + ", time since lifted: " + time_since_lifted);
        stone.SetIceFriction(newF);
        lastBroomPos = transform.position;

    }
    // Update is called once per frame
    void Update()
    {} // similar to the stone, there might be something that always gets updated
    public void SetStone(CurlingStone s)
    {
        stone = s;
    }
}
