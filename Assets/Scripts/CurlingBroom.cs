using UnityEngine;
using UnityEngine.InputSystem;

public class CurlingBroom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private LayerMask iceLayerMask;
    private CurlingStone stone;

    private bool hidden = false;
    private bool active = false;
    void Start()
    {

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
            transform.position = hit.point + Vector3.up * 0.25f;
            if (hidden)
            {
                hidden = false;
                GetComponent<MeshRenderer>().enabled = true;
                GetComponent<BoxCollider>().enabled = true;
            }
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
    // Update is called once per frame
    void Update()
    {} // similar to the stone, there might be something that always gets updated
    public void SetStone(CurlingStone s)
    {
        stone = s;
    }
}
