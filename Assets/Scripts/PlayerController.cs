using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_board;
    private Vector2Int m_cellPosition;
    private bool m_GameOver;

    public void GameOver()
    { 
    m_GameOver = true;
    }
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
       m_board= boardManager;
        MoveTo(cell);
    }
    public void MoveTo(Vector2Int cell)
    { 
    m_cellPosition = cell;
        transform.position = m_board.CellToWorld(cell);
    }
    public void Init()
    {
        m_GameOver = false;

    }
    private void Update()
    {
        if (m_GameOver)
        { 
          if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
          return;
        }
        Vector2Int newCellTarget = m_cellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        if (hasMoved)
        {
            BoardManager.CellData cellData = m_board.GetCellData(newCellTarget);

            if (cellData != null && cellData.Passable)
            {
               GameManager.Instance.m_TurnManager.Tick(); // 턴 매니저에 턴이 지났음을 알림
                if (cellData.ContainedObject == null)
                { 
                 MoveTo(newCellTarget); // 셀에 오브젝트가 없으면 이동
                }
                else if(cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget); // 셀에 오브젝트가 있지만 PlayerWantsToEnter가 true를 반환하면 이동
                    cellData.ContainedObject.PlayerEnterd(); // PlayerWantsToEnter가 true를 반환하면 PlayerEnterd 호출
                }    

                

            }
        }
    }
}
