using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseAI : MonoBehaviour
{
    private GameObject _navMesh;

    [SerializeField] GameObject _aiPrefab;
    int _maxSpawnCount = 10;
    int _spawnDelay = 5;
    int _spawnCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            transform.position = hit.point;
        }

        _navMesh = GameObject.FindGameObjectWithTag("WorldMesh");

        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _maxSpawnCount = gm._maxAiToSpawn;

        StartCoroutine(SpawnAICoroutine());
    }

    private IEnumerator SpawnAICoroutine()
    {
        while (_spawnCounter < _maxSpawnCount)
        {
            //Find new Spawn point 
            Vector3 spawnPoint = GetSpawnPoint();

            GameObject newAi = Instantiate(_aiPrefab, spawnPoint, Quaternion.identity);
            newAi.transform.SetParent(_navMesh.transform);

            float scale = UnityEngine.Random.Range(1, 3);
            newAi.transform.localScale *= scale;
            newAi.SetActive(true);
            _spawnCounter++;


            yield return new WaitForSeconds(_spawnDelay + UnityEngine.Random.Range(1,7));
        }
    }

    private Vector3 GetSpawnPoint()
    {
        Vector2 randomPoint2d = UnityEngine.Random.insideUnitCircle * 10;
        Vector3 randomPoint3d = new Vector3(randomPoint2d.x, 200, randomPoint2d.y) + transform.position;
        

        RaycastHit hit;

        if (Physics.Raycast(randomPoint3d, Vector3.down, out hit))
        {
            return hit.point;
        }

        else return Vector3.zero;
    }
}
