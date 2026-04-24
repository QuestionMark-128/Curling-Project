using UnityEngine;
using UnityEngine.InputSystem;

public class CurlingBroom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private LayerMask iceLayerMask;
    private Camera stoneCamera;
    private CurlingStone stone;

    private bool hidden = false;
    void Start()
    {

    }


    public void AimingPhase()
    {
        // put it somewhere that can't be seen
        //Debug.Log("Broom Aiming");
        if (!hidden)
        {
            transform.position = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
            hidden = true;
        }
        
        
    }

    public void SweepingPhase()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = stoneCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, iceLayerMask))
        {
            transform.position = hit.point + Vector3.up * 0.25f;
        }
        else
        {
            // move it somewhere that can't be seen
        }
    }
    // Update is called once per frame
    void Update()
    {} // similar to the stone, there might be something that always gets updated
    public void SetStone(CurlingStone s)
    {
        stone = s;
        stoneCamera = stone.GetComponentInChildren<Camera>();
    }
}
