using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public List<Transform> targetList=new List<Transform>();
    public List<Vector3> targetLocationList=new List<Vector3>();
    public float moveSpeed;
    public int randomIndex;
    private bool canGenerateRandom;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateRandom();
        Debug.Log("index: " + randomIndex);
        
        foreach (var target in targetList)
        {
            targetLocationList.Add(target.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var dir = (targetLocationList[randomIndex] - transform.position).normalized;
        transform.position += dir * moveSpeed;
        
        if ((transform.position - targetLocationList[randomIndex]).magnitude <= 1.0f)
        {
            canGenerateRandom = true;
        }

        if (canGenerateRandom)
        {
            GenerateRandom();
            canGenerateRandom = false;
        }
    }

    void GenerateRandom()
    {
        randomIndex = UnityEngine.Random.Range(0, targetList.Count);
    }
}
