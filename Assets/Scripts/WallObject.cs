using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile obstacleTile;
    public int MaxHealth = 3;

    private int m_HealthPoints;
    private Tile m_OriginalTile;


    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        m_HealthPoints = MaxHealth;

        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell); // 원래 타일 저장
        GameManager.Instance.BoardManager.SetCellTile(cell, obstacleTile);

    }
    public override bool PlayerWantsToEnter()
    {
        m_HealthPoints-=1; // 플레이어가 벽에 들어가려고 할 때마다 체력 감소

        if (m_HealthPoints>0)

        {
            return false; // 체력이 남아있으면 플레이어가 들어갈 수 없도록 false 반환
        }
        GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile); // 체력이 0이 되면 원래 타일로 복원
        Destroy(gameObject);
        return true; // 체력이 0이 되면 플레이어가 들어갈 수 있도록 true 반환
    }

    
}
