using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject _playerPrefab;
    private GameObject _spawnedPlayerGameObject;
    private int _playersPoints = 0;

    [SerializeField] GameObject _endGameSound;

    [SerializeField] private GameObject _basePrefab;
    private List<GameObject> _spawnedBaseGameObjects;
    private int _baseSpawnCount = 10;
    private List<Vector3> _spawnPoints;

    private GameObject _meshGameObject;
    private SpawnGenerator _spawnGenerator;
    private MapGenerator _mapGenerator;

    int[] _goodSeeds = { 297, 298, 290, 275, 291, 232, 294};

    private void Start()
    {
        // Game Spawning and Start

        GameObject mapGeneratorGO = GameObject.FindGameObjectWithTag("MapGenerator");
        _meshGameObject = GameObject.FindGameObjectWithTag("WorldMesh");

        _spawnGenerator = mapGeneratorGO.GetComponent<SpawnGenerator>();
        _spawnPoints = _spawnGenerator.GenerateSpawns();

        _mapGenerator = mapGeneratorGO.GetComponent<MapGenerator>();

        RegenerateSeed();


        SpawnBases();

        SpawnPlayer();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    public void RegenerateSeed()
    {
        _mapGenerator.noiseData.seed = UnityEngine.Random.Range(200, 300);
        _mapGenerator.GenerateMap();

        NavMeshSurface NMS = _meshGameObject.GetComponent<NavMeshSurface>();
        NMS.BuildNavMesh();
    }
    public void SpawnPlayer()
    {
        RaycastHit hit;
        Vector3 point = _spawnPoints[0];

        if (Physics.Raycast(point, Vector3.up, out hit)) point = hit.point;
        else if (Physics.Raycast(point, -Vector3.up, out hit)) point = hit.point;


        _spawnedPlayerGameObject = Instantiate(_playerPrefab);
        // spawn player and ensure they are above the ground
        _spawnedPlayerGameObject.transform.position = point + new Vector3(0, 100, 0);
        _spawnedPlayerGameObject.SetActive(true);
    }

    public void SpawnBases()
    {
        _spawnedBaseGameObjects = new List<GameObject>();
        Vector3 point;

        for (int i = 0; i < _baseSpawnCount; i++)
        {
            RaycastHit hit;
            point = _spawnPoints[i + 1];

            if (Physics.Raycast(point, Vector3.up,out hit)) point = hit.point;

            if (Physics.Raycast(point, -Vector3.up,out hit)) point = hit.point;

            GameObject go = Instantiate(_basePrefab);
            go.transform.position = point;
            go.transform.SetParent(_meshGameObject.transform);

            _spawnedBaseGameObjects.Add(go);
        }
    }

    public void AddPoints(int p)
    { _playersPoints += p; }

    public void EndGame() 
    {
        Instantiate(_endGameSound);
    
    }
}
