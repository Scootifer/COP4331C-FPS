using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    InterSceneData _dataScript;

    [SerializeField] private GameObject _playerPrefab;
    private GameObject _spawnedPlayerGameObject;
    private int _playersPoints = 0;

    private GameObject _playerUIGO;
    private UIScript _playerUI;

    [SerializeField] GameObject _endGameSound;

    [SerializeField] private GameObject _basePrefab;
    private List<GameObject> _spawnedBaseGameObjects;
    private int _baseSpawnCount = 10;
    private int _currentBaseCount;
    public int _maxAiToSpawn = 10;
    private List<Vector3> _spawnPoints;

    private GameObject _meshGameObject;
    private SpawnGenerator _spawnGenerator;
    private MapGenerator _mapGenerator;

    public void Start()
    {
        // Coroutine will start the game without making it unresponsive to the OS. Game will still 'freeze'
        StartCoroutine(StartWorldCoroutine());
    }

    public IEnumerator StartWorldCoroutine() 
    {
        yield return new WaitForSeconds(1.5f);

        GameObject mapGeneratorGO = mapGeneratorGO = GameObject.FindGameObjectWithTag("MapGenerator"); ;

        _meshGameObject = GameObject.FindGameObjectWithTag("WorldMesh");

        _mapGenerator = mapGeneratorGO.GetComponent<MapGenerator>();

        _dataScript = GameObject.Find("DataObject").GetComponent<InterSceneData>();
        _playerUIGO = GameObject.Find("UICanvas");
        _playerUI = _playerUIGO.GetComponent<UIScript>();


        SetDifficulty();
        RegenerateSeed();

        _spawnGenerator = mapGeneratorGO.GetComponent<SpawnGenerator>();
        _spawnPoints = _spawnGenerator.GenerateSpawns();

        if (!CheckPlayerSpawn()) SceneManager.LoadScene(1);

        SpawnBases();

        DestroyLoadingObjects();

        SpawnPlayer();
        
        yield return null;
    }

    private void SetDifficulty()
    {
        Debug.Log($"Difficulty{_dataScript.difficulty}");

        switch (_dataScript.difficulty) 
        {
            case 1:
                _baseSpawnCount = 5;
                _maxAiToSpawn = 10;
                break;
            case 2:
                _baseSpawnCount = 10;
                _maxAiToSpawn = 20;
                break;
            case 3:
                _baseSpawnCount = 15;
                _maxAiToSpawn = 999;
                break;

        }
    }

    public void RegenerateSeed()
    {
        // pick random seed and generate map
        _mapGenerator.noiseData.seed = UnityEngine.Random.Range(200, 300);
        _mapGenerator.GenerateMap();

        // Bake nav mesh surface
        NavMeshSurface NMS = _meshGameObject.GetComponent<NavMeshSurface>();
        NMS.BuildNavMesh();
    }
    public void SpawnPlayer()
    {
        _spawnedPlayerGameObject = Instantiate(_playerPrefab);
        // spawn player slightly above the ground
        _spawnedPlayerGameObject.transform.position = _spawnPoints[0] + new Vector3(0, 2, 0);

        // player UI
        _spawnedPlayerGameObject.SetActive(true);
        _playerUIGO.GetComponent<Canvas>().enabled = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
    private bool CheckPlayerSpawn()
    {
        return Physics.Raycast(_spawnPoints[0], Vector3.down, 5) && !Physics.Raycast(_spawnPoints[0], Vector3.up, 100);
    }

    public void SpawnBases()
    {
        _spawnedBaseGameObjects = new List<GameObject>();
        
        Vector3 point;
        for (int i = 0; i < _baseSpawnCount; i++)
        {
            // Validate spawn location
            RaycastHit hit;
            point = _spawnPoints[i + 1];
            point += new Vector3(0, 200, 0);
            if (Physics.Raycast(point, -Vector3.up,out hit)) point = hit.point;

            // Spawn, name, and set parent
            GameObject go = Instantiate(_basePrefab);
            go.transform.name = $"base{i + 1}";
            go.transform.position = point;
            go.transform.SetParent(_meshGameObject.transform);

            _spawnedBaseGameObjects.Add(go);
        }

        // Set UI text
        _currentBaseCount = _baseSpawnCount;
    }

    public void AddPoints(int p)
    { 
        _playersPoints += p;
        //update score text
        _playerUI.SetScoreText(_playersPoints, (_baseSpawnCount - _currentBaseCount), _baseSpawnCount);
    }

    public void BaseDestroyed() 
    {
        _currentBaseCount--;

        if (_currentBaseCount <= 0)
        {
            EndGame(true);
        }
    }

    public void DestroyLoadingObjects() 
    {
        GameObject.Find("DeathCamera").GetComponent<Camera>().enabled = false;
        Destroy(GameObject.Find("LoadingCanvas"));
    }

    private IEnumerator DestroyEntities()
    {
        Destroy(_meshGameObject);

        foreach( GameObject entity in _spawnedBaseGameObjects)  
        {
            yield return null;
            Destroy(entity);
        }
    
    }

    private IEnumerator DelayEndSound()
    {
        yield return new WaitForSeconds(1.25f);
        Instantiate(_endGameSound);

    }

    public void EndGame(bool didPlayerWin) 
    {
        StartCoroutine(DestroyEntities());
        
        GameObject deathCamera = GameObject.Find("DeathCamera");
        deathCamera.GetComponent<Camera>().enabled = true;
        deathCamera.GetComponent<AudioListener>().enabled = true;
        StartCoroutine(DelayEndSound());
        _playerUIGO.SetActive(false);

        GameObject pauseMenu = GameObject.Find("Pause");
        pauseMenu.GetComponent<Canvas>().enabled = true;

        PauseMenu pauseMenuScript = pauseMenu.GetComponent<PauseMenu>();
        pauseMenuScript.SetScoreText(_playersPoints);
        pauseMenuScript.SetPauseText(didPlayerWin);

        
    
    }
}
