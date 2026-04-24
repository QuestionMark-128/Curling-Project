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

    public void OnStoneStopped(CurlingStone stone)
    {
        // if enough stones thrown, start scoring
        // else throw another stone
    }

    private void SpawnStone()
    {
        CurlingStone s = Instantiate(stone, spawnPoint.position, spawnPoint.rotation);
        s.Initialize(Team.Red); // add logic eventually
        CurlingBroom b = Instantiate(broom, spawnPoint.position, spawnPoint.rotation);
        b.SetStone(s);
    }
    
    void Start()
    {
        spawnPoint = spawnPoint_1;
        SpawnStone();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
