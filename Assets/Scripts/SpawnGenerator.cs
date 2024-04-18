using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpawnGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [Header("Raycast Settings")]
    [SerializeField] int numberOfSpawnPoints;
    [SerializeField] bool printDebugCubes;

    [Space]

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] Vector2 xRange;
    [SerializeField] Vector2 zRange;

    GameManager gameManager;

    public List<Vector3> GenerateSpawns()
    {

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        ClearDebugCubes();

        List<Vector3> ValidSpawnPoints = new List<Vector3>();
        int pointsFound = 0;

        while (pointsFound < numberOfSpawnPoints)
        {
            float sampleX = Random.Range(xRange.x, xRange.y);
            float sampleY = Random.Range(zRange.x, zRange.y);
            Vector3 rayStart = new Vector3(sampleX, maxHeight, sampleY);

            if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity)) continue;

            if (hit.point.y < minHeight) continue;

            ValidSpawnPoints.Add(hit.point);
            
            if (printDebugCubes)
            {
                Debug.Log("Valid spawn point found: " + ValidSpawnPoints[pointsFound]);
                //GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(this.prefab, transform);
                //instantiatedPrefab.transform.position = ValidSpawnPoints[pointsFound];
            }

            pointsFound++;
        }

        return ValidSpawnPoints;
    }
    public void ClearDebugCubes()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
