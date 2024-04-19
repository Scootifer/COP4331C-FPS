using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterSceneData : MonoBehaviour
{
    public int difficulty;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
