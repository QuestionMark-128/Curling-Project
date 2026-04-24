using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Teams")]
    public List<GameObject> teamAStones = new List<GameObject>();
    public List<GameObject> teamBStones = new List<GameObject>();

    [Header("Turn System")]
    public bool teamATurn = true;
    public int stonesThrown = 0;
    public int stonesPerTeam = 4;

    [Header("House")]
    public Transform houseCenter;

    void Awake()
    {
        instance = this;
    }

    public void RegisterStone(GameObject stone, bool isTeamA)
    {
        if (isTeamA)
            teamAStones.Add(stone);
        else
            teamBStones.Add(stone);

        stonesThrown++;

        SwitchTurn();

        if (stonesThrown >= stonesPerTeam * 2)
        {
            EndRound();
        }
    }

    void SwitchTurn()
    {
        teamATurn = !teamATurn;
    }

    void EndRound()
    {
        Debug.Log("END COMPLETE");

        float scoreA = GetBestDistance(teamAStones);
        float scoreB = GetBestDistance(teamBStones);

        if (scoreA < scoreB)
            Debug.Log("Team A wins the end!");
        else if (scoreB < scoreA)
            Debug.Log("Team B wins the end!");
        else
            Debug.Log("Tie end!");

        // Reset for next round
        ResetRound();
    }

    float GetBestDistance(List<GameObject> stones)
    {
        float best = Mathf.Infinity;

        foreach (var s in stones)
        {
            if (s == null) continue;

            float d = Vector3.Distance(s.transform.position, houseCenter.position);

            if (d < best)
                best = d;
        }

        return best;
    }

    void ResetRound()
    {
        teamAStones.Clear();
        teamBStones.Clear();
        stonesThrown = 0;
        teamATurn = true;
    }
}