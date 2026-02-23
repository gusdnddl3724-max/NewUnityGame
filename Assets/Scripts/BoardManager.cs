using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public FoodObject[] FoodPrefab;
    public WallObject WallPrefab;
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;    
    }
    public CellData[,] m_BoardData;
    private Tilemap m_tilemap;
    private Grid m_Grid;
    public List<Vector2Int> m_EmptyCellsList;

    public int width;
    public int height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    public void Init()
    {
        m_tilemap = GetComponentInChildren<Tilemap>();// 컴포넌트 가져오기
        m_Grid = GetComponentInChildren<Grid>();// 그리드 컴포넌트 가져오기 

        m_EmptyCellsList = new List<Vector2Int>();

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

                    m_EmptyCellsList.Add(new Vector2Int(x, y)); // 빈 셀 리스트에 추가

                }
                m_tilemap.SetTile(new Vector3Int(x, y, 0), tile);// 타일맵에 타일 설정
            }
        }
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));
        GenerateWall();
        GenerateFood();

    }
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)(cellIndex));
    
    }
    public CellData GetCellData(Vector2Int cellIndex)// 셀 데이터 가져오기
    {
        if (cellIndex.x < 0 || cellIndex.x >= width || cellIndex.y < 0 || cellIndex.y >= height)// 범위 밖일 때
        { 
         return null;
        }
        return m_BoardData[cellIndex.x, cellIndex.y];//  셀 데이터 반환
    }

    
    void GenerateFood()
    {
        int foodCount = 5;
        for (int i = 0; i < foodCount; ++i)
        {
          int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            

            int prefabIndex = Random.Range(0, FoodPrefab.Length);
            FoodObject newFood = Instantiate(FoodPrefab[prefabIndex]);

            newFood.transform.position = CellToWorld(coord);
            AddObject(newFood, coord);




        }
    }
    void GenerateWall()
    {
        int wallCount = Random.Range(6, 10);
        for (int i = 0; i < wallCount; ++i)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);// 빈 셀 리스트에서 랜덤 인덱스 선택
            Vector2Int coord= m_EmptyCellsList[randomIndex];// 해당 인덱스의 좌표 가져오기

            m_EmptyCellsList.RemoveAt(randomIndex);// 빈 셀 리스트에서 해당 좌표 제거
            
            WallObject newWall = Instantiate(WallPrefab);// 벽 오브젝트 생성

            AddObject(newWall, coord);// 벽 오브젝트 추가

        }
    }
    public void SetCellTile(Vector2Int cellIndex, Tile tile) // 셀 타일 설정
    {
        m_tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile); // 타일맵에 타일 설정
    }
    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0)); // 타일맵에서 타일 가져오기
    }

    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y]; // 셀 데이터 가져오기
        obj.transform.position = CellToWorld(coord); // 오브젝트 위치 설정
        data.ContainedObject = obj; // 셀 데이터에 오브젝트 저장
        obj.Init(coord);// 오브젝트 초기화
    }
        
}
