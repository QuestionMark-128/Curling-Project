using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    public Rigidbody stonePrefab;
    public Transform spawnPoint;

    public float minPower = 5f;
    public float maxPower = 30f;
    public bool isTeamA = true;

    float currentPower;
    bool isCharging;

    void Update()
    {
        if (!GameController.instance.teamATurn && isTeamA)
            return;

        if (GameController.instance.teamATurn && !isTeamA)
            return;
        // Start charging
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isCharging = true;
            currentPower = minPower;
        }

        // Charge power
        if (isCharging && Keyboard.current.spaceKey.isPressed)
        {
            currentPower += Time.deltaTime * 20f;
            currentPower = Mathf.Clamp(currentPower, minPower, maxPower);
        }

        // Release throw
        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            isCharging = false;
            ThrowStone();
        }
    }

    void ThrowStone()
    {
        Rigidbody stone = Instantiate(stonePrefab, spawnPoint.position, spawnPoint.rotation);

        Stone s = stone.GetComponent<Stone>();

        float curlInput = 0;

        if (Input.GetKey(KeyCode.Q)) curlInput = -1;
        if (Input.GetKey(KeyCode.E)) curlInput = 1;

        if (s != null)
            s.spinDirection = curlInput;

        stone.AddForce(spawnPoint.forward * currentPower, ForceMode.Impulse);

        GameController.instance.RegisterStone(stone.gameObject, isTeamA);
    }
}