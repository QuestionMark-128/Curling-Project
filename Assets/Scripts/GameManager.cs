using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;
public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // State management variables
    private enum GamePhase
    {
        Aiming, 
        Sweeping,
        Scoring  
    };
    private GamePhase currentPhase;
    private int roundNumber;
    private int stonesThrown;

    private int RedScore;
    private int BlueScore;


    // Object references 

    [SerializeField] private CurlingStone stone_prefab;
    [SerializeField] private Transform spawnPoint_1;
    [SerializeField] private Transform spawnPoint_2;
    [SerializeField] private CurlingBroom broom_prefab;
    [SerializeField] private FollowCamera followCamera;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI scoredata;
 
    private List<CurlingStone> stones = new List<CurlingStone>();
    private List<float> distances = new List<float>();
    private CurlingStone stone;
    private CurlingBroom broom;

    private Transform spawnPoint;

    public static GameManager Instance {get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void EndGame()
    {
        
    }


    // MAIN GAME MANAGER FEATURES (arranged as they occur)
    void Start()
    {
        currentPhase = GamePhase.Aiming;
        stonesThrown = 0;
        roundNumber = 1;
        spawnPoint = spawnPoint_1;
        SpawnStone();

    }

    public void Update()
    {
        string currteam;
        if (stonesThrown < 4)
        {
            currteam = "Red";
        }
        else
        {
            currteam = "Blue";
        }
        scoredata.text = string.Format("Round: " + roundNumber + ", Current Team: " + currteam + 
        ", Stones thrown: " + stonesThrown); // stones thrown, round number, current team
        switch(currentPhase)
        {
            case GamePhase.Aiming:

                stone.AimingPhase();
                broom.AimingPhase();
                

                if (stone.getIsThrown())
                {
                    currentPhase = GamePhase.Sweeping;
                }

            break;
            case GamePhase.Sweeping:

                stone.SweepingPhase();
                broom.SweepingPhase();

            break;
           
            case GamePhase.Scoring:

                spawnPoint = spawnPoint_2;
                roundNumber++;

                stonesThrown = 0;

                float lowest = Mathf.Infinity;
                int winning_index = 0;
                Team winners;
                for (int i = 0; i < 8; i++)
                {
                    if (distances[i] < lowest)
                    {
                        lowest = distances[i];
                        winning_index = i;
                    }
                }
                winners = stones[winning_index].getTeam();

                if (winners == Team.Red)
                {
                    RedScore++;
                }
                else if (winners == Team.Blue)
                {
                    BlueScore++;
                }

                distances.Clear();
                foreach (CurlingStone s in stones)
                {
                    Destroy(s.gameObject);
                }
                stones.Clear();

                currentPhase = GamePhase.Aiming;
                SpawnStone();

            break;
        }
    }

    private void SpawnStone()
    {
        //Debug.Log("Stone spawned");
        stone = Instantiate(stone_prefab, spawnPoint.position, spawnPoint.rotation);
        
        if (stonesThrown < 4)
        {
            stone.Initialize(Team.Red, this); // add logic eventually
        }
        else
        {
            stone.Initialize(Team.Blue, this); // add logic eventually
        }
        if (!broom)
        {
            broom = Instantiate(broom_prefab, spawnPoint.position, spawnPoint.rotation);
        }
        
        broom.SetStone(stone);
        followCamera.setTarget(stone.transform, roundNumber);
        stone.setText(text);
        
    }

    public void OnStoneStopped(bool inScoreZone, Vector3 button_position) // will be called in the stone's SweepingPhase method when stone stops
    {   // no need to pass a stone, the game manager already owns it
        
        currentPhase = GamePhase.Aiming;
        if (inScoreZone)
        {
            stonesThrown++; // only if we have entered the scoring zone
        }
        else
        {
            return;
        }
        
        float dist = Vector3.Distance(stone.transform.position, button_position);
        distances.Add(dist);
        //Destroy(broom);
        stones.Add(stone);
        stone = null;
        
        if (stonesThrown >= 8)
        {
            currentPhase = GamePhase.Scoring; // This might be bad practice to put here
            return;
        }
        
        SpawnStone();
    
    }

    public CurlingStone getActiveStone()
    {
        return stone;
    }
    public int getRound()
    {
        return roundNumber;
    }

    
}
