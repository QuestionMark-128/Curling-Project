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

    [SerializeField] private CurlingStone stone;
    [SerializeField] private Transform spawnPoint_1;
    [SerializeField] private Transform spawnPoint_2;
    [SerializeField] private CurlingBroom broom;

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
            DontDestroyOnLoad(gameObject);
        }
    }

    public void EndGame()
    {
        
    }


    // MAIN GAME MANAGER FEATURES (arranged as they occur)
    void Start()
    {
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


                break;
            case GamePhase.Sweeping:

                stone.SweepingPhase();
                broom.SweepingPhase();

                break;
           
            case GamePhase.Scoring:



                break;
        }
    }

    private void SpawnStone()
    {
        CurlingStone s = Instantiate(stone, spawnPoint.position, spawnPoint.rotation);
        
        if (stonesThrown < 4)
        {
            s.Initialize(Team.Red); // add logic eventually
        }
        else
        {
            s.Initialize(Team.Blue); // add logic eventually
        }
        CurlingBroom b = Instantiate(broom, spawnPoint.position, spawnPoint.rotation);
        b.SetStone(s);
    }

    public void OnStoneStopped(CurlingStone stone) // will be called in the stone's SweepingPhase method when stone stops
    {
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
            spawnPoint = spawnPoint_2;
            roundNumber++;
        }
    }

    
    
    

   
}
