using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;

    }
    public CellData[,] m_BoardData;
    private Tilemap m_tilemap;
    private Grid m_Grid;

    public int width;
    public int height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    public void Init()
    {
        m_tilemap = GetComponentInChildren<Tilemap>();// 컴포넌트 가져오기
        m_Grid = GetComponentInChildren<Grid>();// 그리드 컴포넌트 가져오기 

        m_BoardData = new CellData[width, height];// 보드 데이터 초기화

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData(); // 셀 데이터 초기화
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    // 벽 타일 배치
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    m_BoardData[x, y].Passable = false; // 벽은 통과 불가능

                }
                else
                {
                    // 바닥 타일 배치
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true; // 바닥은 통과 가능

                }
                m_tilemap.SetTile(new Vector3Int(x, y, 0), tile);// 타일맵에 타일 설정
            }
        }
      
    }
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)(cellIndex));
    
    }
    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= width || cellIndex.y < 0 || cellIndex.y >= height)// 범위 밖일 때
        { 
         return null;
        }
        return m_BoardData[cellIndex.x, cellIndex.y];//  셀 데이터 반환
    }
}
