using System.Collections.Generic;
using System.Linq;
using OctanGames.Extensions;
using UnityEngine;
using Random = System.Random;

public class FieldGenerator : MonoBehaviour
{
    private const string HOLDER_NAME = "Generated Field";

    [Header("Prefabs")]
    [SerializeField] private Transform _tilePrefab;
    [SerializeField] private Sheaf _sheafPrefab;

    [Header("Properties")]
    [SerializeField] private Vector2Int _mapSize;
    [SerializeField] private int _seed;
    [SerializeField, Min(0)] private float _minObstacleHeight;
    [SerializeField, Min(0)] private float _maxObstacleHeight;
    [SerializeField, Min(0.1f)] private float _growthDelay = 10f;
    [SerializeField, Range(0, 1)] private float _growthPercent;
    [SerializeField, Range(0, 1)] private float _outlinePercent = 0.04f;
    [SerializeField] private float _tileSize = 1.57f;

    private readonly List<Vector2Int> _allTileCoordinates = new List<Vector2Int>();
    private Queue<Vector2Int> _shuffledTileCoordinates;
    private Transform[,] _tileMap;
    private readonly List<Sheaf> _sheaves = new List<Sheaf>();

    public int CountSheaves => _sheaves.Count;
    public int CountGrownSheaves => _sheaves.Count(s => !s.IsDestroyed);
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

    private void Start()
    {
        GenerateField();
        AnimateFieldGrow();
    }

    private void AnimateFieldGrow()
    {
        for (var i = 0; i < _sheaves.Count; i++)
        {
            _sheaves[i].Grow(1f + 0.1f * i);
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
        _allTileCoordinates.Clear();
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
        if (!ReferenceEquals(childTransform, null))
        {
            DestroyImmediate(childTransform.gameObject);
        }

        Transform generatedFieldHolder = new GameObject(HOLDER_NAME).transform;
        generatedFieldHolder.SetParent(transform, false);
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
                newTile.SetParent(parent, false);
                _tileMap[x, y] = newTile;
            }
        }
    }

    private void SpawnSheaves(Transform generatedFieldHolder)
    {
        var random = new Random(_seed);
        var sheafCount = (int)(_mapSize.x * _mapSize.y * _growthPercent);

        _sheaves.Clear();
        for (var i = 0; i < sheafCount; i++)
        {
            Sheaf sheaf = CreateSheaf(generatedFieldHolder, random);
            sheaf.OnDestroyed += () =>
            {
                float randomHeight = GetRandomHeight(random);
                sheaf.Grow(randomHeight, _growthDelay);
            };
            _sheaves.Add(sheaf);
        }
    }

    private Sheaf CreateSheaf(Transform generatedFieldHolder, Random random)
    {
        Vector2Int randomCoord = GetRandomCoord();

        Vector3 position = CoordToPosition(randomCoord.x, randomCoord.y);
        float randomHeight = GetRandomHeight(random);
        Vector3 positionCorrectedByHeight = position + randomHeight * 0.5f * Vector3.up;

        Sheaf newSheaf = Instantiate(_sheafPrefab, positionCorrectedByHeight, Quaternion.identity);
        newSheaf.name = $"{_sheafPrefab.name} ({randomCoord.x}:{randomCoord.y})";
        Transform sheafTransform = newSheaf.transform;
        sheafTransform.SetParent(generatedFieldHolder, false);
        sheafTransform.localScale = GetTargetScale(randomHeight);

        return newSheaf;
    }

    private float GetRandomHeight(Random random)
    {
        return Mathf.Lerp(_minObstacleHeight, _maxObstacleHeight, (float)random.NextDouble());
    }

    private Vector3 GetTargetScale(float obstacleHeight)
    {
        float outlinedSize = (1 - _outlinePercent) * _tileSize;
        return new Vector3(outlinedSize, obstacleHeight, outlinedSize);
    }

    private Vector3 CoordToPosition(int x, int y)
    {
        float xPosition = -_mapSize.x * 0.5f + 0.5f + x;
        float yPosition = -_mapSize.y * 0.5f + 0.5f + y;
        return new Vector3(xPosition, 0, yPosition) * _tileSize;
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