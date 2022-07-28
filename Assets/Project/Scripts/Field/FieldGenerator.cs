using System.Collections.Generic;
using OctanGames.Extensions;
using UnityEngine;
using Random = System.Random;

public class FieldGenerator : MonoBehaviour
{
    private const string HOLDER_NAME = "Generated Field";

    [Header("Prefabs")]
    [SerializeField] private Transform _tilePrefab;
    [SerializeField] private Transform _sheafPrefab;

    [Header("Properties")]
    [SerializeField] private Vector2Int _mapSize;
    [SerializeField] private int _seed;
    [SerializeField, Min(0)] private float _minObstacleHeight;
    [SerializeField, Min(0)] private float _maxObstacleHeight;
    [SerializeField, Range(0, 1)] private float _growthPercent;
    [SerializeField, Range(0, 1)] private float _outlinePercent = 0.04f;
    [SerializeField] private float _tileSize = 1.57f;

    private List<Vector2Int> _allTileCoordinates;
    private Queue<Vector2Int> _shuffledTileCoordinates;
    private Transform[,] _tileMap;

    private Vector2Int MapCenter => new Vector2Int(_mapSize.x / 2, _mapSize.y / 2);

    private void OnValidate()
    {
        if (_mapSize.x <= 0)
        {
            _mapSize.x = 1;
        }
        if (_mapSize.y <= 0)
        {
            _mapSize.y = 1;
        }
    }

    public void GenerateField()
    {
        InitCoordinates();

        Transform generatedFieldHolder = GeneratedFieldHolder();
        GenerateTiles(generatedFieldHolder);
        SpawnSheaves(generatedFieldHolder);
    }

    private void InitCoordinates()
    {
        _allTileCoordinates = new List<Vector2Int>();
        for (var x = 0; x < _mapSize.x; x++)
        {
            for (var y = 0; y < _mapSize.y; y++)
            {
                _allTileCoordinates.Add(new Vector2Int(x, y));
            }
        }
        _shuffledTileCoordinates = new Queue<Vector2Int>(_allTileCoordinates.Shuffle(_seed));
    }

    private Transform GeneratedFieldHolder()
    {
        Transform childTransform = transform.GetChild(0);
        if (childTransform != null)
        {
            DestroyImmediate(childTransform.gameObject);
        }

        Transform generatedFieldHolder = new GameObject(HOLDER_NAME).transform;
        generatedFieldHolder.SetParent(transform);
        return generatedFieldHolder;
    }

    private void GenerateTiles(Transform parent)
    {
        _tileMap = new Transform[_mapSize.x, _mapSize.y];
        for (var x = 0; x < _mapSize.x; x++)
        {
            for (var y = 0; y < _mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(_tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));

                newTile.name = $"{_tilePrefab.name} ({x}:{y})";
                newTile.localScale = (1 - _outlinePercent) * _tileSize * Vector3.one;
                newTile.parent = parent;
                _tileMap[x, y] = newTile;
            }
        }
    }

    private void SpawnSheaves(Transform generatedFieldHolder)
    {
        var random = new Random(_seed);
        var sheafCount = (int)(_mapSize.x * _mapSize.y * _growthPercent);

        for (var i = 0; i < sheafCount; i++)
        {
            float sheafHeight = Mathf.Lerp(_minObstacleHeight, _maxObstacleHeight, (float)random.NextDouble());
            CreateSheaf(generatedFieldHolder, sheafHeight);
        }
    }

    private void CreateSheaf(Transform generatedFieldHolder, float obstacleHeight)
    {
        Vector2Int randomCoord = GetRandomCoord();
        Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

        Transform newSheaf = Instantiate(_sheafPrefab,
            obstaclePosition + obstacleHeight * 0.5f * Vector3.up,
            Quaternion.identity);

        newSheaf.name = $"{_sheafPrefab.name} ({randomCoord.x}:{randomCoord.y})";
        newSheaf.parent = generatedFieldHolder;
        newSheaf.localScale = new Vector3((1 - _outlinePercent) * _tileSize, obstacleHeight,
            (1 - _outlinePercent) * _tileSize);
    }

    private Vector3 CoordToPosition(int x, int y)
    {
        float xPosition = -_mapSize.x * 0.5f + 0.5f + x;
        float yPosition = -_mapSize.y * 0.5f + 0.5f + y;
        return new Vector3(xPosition, transform.position.y, yPosition) * _tileSize;
    }

    public Transform GetTileFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / _tileSize + (_mapSize.x - 1) * 0.5f);
        int y = Mathf.RoundToInt(position.z / _tileSize + (_mapSize.y - 1) * 0.5f);
        x = Mathf.Clamp(x, 0, _tileMap.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, _tileMap.GetLength(1) - 1);

        return _tileMap[x, y];
    }

    private Vector2Int GetRandomCoord()
    {
        Vector2Int randomCoord = _shuffledTileCoordinates.Dequeue();
        _shuffledTileCoordinates.Enqueue(randomCoord);
        return randomCoord;
    }
}