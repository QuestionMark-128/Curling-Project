using UnityEngine;

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

    // Object references 

    [SerializeField] private CurlingStone stone_prefab;
    [SerializeField] private Transform spawnPoint_1;
    [SerializeField] private Transform spawnPoint_2;
    [SerializeField] private CurlingBroom broom_prefab;

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
        roundNumber = 0;
        spawnPoint = spawnPoint_1;
        SpawnStone();

    }

    public void Update()
    {
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
        broom = Instantiate(broom_prefab, spawnPoint.position, spawnPoint.rotation);
        broom.SetStone(stone);
        
    }

    public void OnStoneStopped() // will be called in the stone's SweepingPhase method when stone stops
    {   // no need to pass a stone, the game manager already owns it
        stonesThrown++;

        // check the stone's team
        // add stone to correct list

        if (stonesThrown == 4)
        {
            // change teams
            
        }
        else if (stonesThrown == 8)
        {
            // score and new round
            currentPhase = GamePhase.Scoring; // This might be bad practice to put here
        }
    }

    
}
