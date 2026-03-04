using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_board;
    private Vector2Int m_cellPosition;
    private bool m_GameOver;
    private Animator m_Animator;
    private bool m_IsMoving;
    private Vector3 m_MoveTaret;
    public float MoveSpeed = 5f;
    private void Awake()
        {
            m_Animator = GetComponent<Animator>();
    }

    
    public void GameOver()
    { 
    m_GameOver = true;
    }
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
       m_board= boardManager;
        MoveTo(cell,false);
    }
    public void MoveTo(Vector2Int cell, bool immediate)
    {
        m_cellPosition = cell;
        if (immediate)
        {
            m_IsMoving = false;
            transform.position = m_board.CellToWorld(m_cellPosition);
        }
        else
        { 
          m_IsMoving = true;
            m_MoveTaret= m_board.CellToWorld(m_cellPosition);
        }    
        
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
                 MoveTo(newCellTarget, false); // 셀에 오브젝트가 없으면 이동
                }
                else if(cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget, false); // 셀에 오브젝트가 있지만 PlayerWantsToEnter가 true를 반환하면 이동
                    cellData.ContainedObject.PlayerEnterd(); // PlayerWantsToEnter가 true를 반환하면 PlayerEnterd 호출
                }    

                

            }
        }
        if (m_IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_MoveTaret, MoveSpeed*Time.deltaTime);
            if (transform.position==m_MoveTaret)
            {
               m_IsMoving = false;
                m_Animator.SetBool("Moving", false);
                var cellData = m_board.GetCellData(m_cellPosition);
                if (cellData.ContainedObject != null)
                {
                    cellData.ContainedObject.PlayerEnterd();
                }// 이동이 완료되면 PlayerEnterd 호출
            }
        } return;
    }
    public Vector2Int Cell
    {
        get { return m_cellPosition; }
    }

}
