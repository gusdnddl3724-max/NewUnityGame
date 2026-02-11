using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    { 
       public bool Passable;

    }
    public CellData[,] BoardData;
    private Tilemap tilemap;

    public int width;
    public int height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    void Start()
    {
        tilemap = GetComponentInChildren<Tilemap>();// 컴포넌트 가져오기
        BoardData = new CellData[width, height];// 보드 데이터 초기화

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Tile tile;
                BoardData[x, y] = new CellData(); // 셀 데이터 초기화
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    // 벽 타일 배치
                     tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    BoardData[x, y].Passable = false; // 벽은 통과 불가능

                }
                else
                {
                    // 바닥 타일 배치
                    tile=GroundTiles[Random.Range(0, GroundTiles.Length)];
                    BoardData[x, y].Passable = true; // 바닥은 통과 가능

                }
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);// 타일맵에 타일 설정
            }
        }
    }
}
