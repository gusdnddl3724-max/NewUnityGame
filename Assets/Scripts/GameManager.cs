using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }   

    public BoardManager BoardManager;
    public PlayerController PlayerController;

    public TurnManager m_TurnManager;

    public UIDocument UIDoc;
    private Label m_FoodLabel;

    private int m_FoodAmount = 100;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
            Instance = this;
        
    }
    void Start()

    {
        m_TurnManager = new TurnManager();
        m_TurnManager.OnTick += OnTurnHappen;

        m_FoodLabel= UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food: "+ m_FoodAmount;

        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));

    }

    void OnTurnHappen()
    {
        m_FoodAmount -= 1;
        m_FoodLabel.text= "Food:" + m_FoodAmount;
    }
}
