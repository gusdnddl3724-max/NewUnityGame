using UnityEngine;

public class GameManager : MonoBehaviour
{
   public BoardManager BoardManager;
    public PlayerController PlayerController;

    private TrunManager m_TrunManager;

    void Start()
    {
        m_TrunManager = new TrunManager();

        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));

    }
}
