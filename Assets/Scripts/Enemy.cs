using UnityEngine;

public class Enemy : CellObject
{
    public int health = 3;

    private int m_currentHealth;

    private void Awake()
    {
        GameManager.Instance.m_TurnManager.OnTick += TurnHappened;
    }

    private void OnDestroy()
    {
        GameManager.Instance.m_TurnManager.OnTick -= TurnHappened;
    }

    public override void Init(Vector2Int coord)
    {
      base.Init(coord);
        m_currentHealth = health;
    }

    public override bool PlayerWantsToEnter()
    {
        m_currentHealth-= 1;
        if (m_currentHealth <= 0)
        { 
         Destroy(gameObject);
        }
        return false;
    }

    bool MoveTo(Vector2Int coord)
    { 
     var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if (targetCell == null
            || !targetCell.Passable
            ||  targetCell.ContainedObject != null)// 이동하려는 셀이 유효하지 않거나 통과 불가능하거나 다른 오브젝트가 있는 경우
        { 
         return false;
        }

        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;// 현재 셀에서 오브젝트 제거

        targetCell.ContainedObject = this;// 이동하려는 셀에 오브젝트 배치
        m_Cell = coord;// 현재 위치 업데이트
        transform.position = board.CellToWorld(coord);// 오브젝트의 위치를 셀의 월드 좌표로 업데이트

        return true;
    }

    void TurnHappened()
    {
        var playerCell = GameManager.Instance.PlayerController.Cell;

        int xDist = playerCell.x - m_Cell.x;
        int yDist = playerCell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1)
            || (yDist == 0 && absXDist == 1))
        {
            GameManager.Instance.ChangeFood(3);
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!tryMoveInY(xDist))
                { 
                  tryMoveInX(yDist);
                }
            }
        }
    }
    bool tryMoveInX(int xDist)
    {
        if (xDist > 0)
        { 
         return MoveTo(m_Cell + Vector2Int.right);
        }
        else if (xDist < 0)
        { 
         return MoveTo(m_Cell + Vector2Int.left);
        }
        return false;
    }
    bool tryMoveInY(int yDist)
    {
        if (yDist > 0)
        { 
         return MoveTo(m_Cell + Vector2Int.up);
        }
        else if (yDist < 0)
        { 
         return MoveTo(m_Cell + Vector2Int.down);
        }
        return false;
    }
}
