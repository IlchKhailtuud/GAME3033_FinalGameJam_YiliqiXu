using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    public List<Transform> targetList=new List<Transform>();
    public List<Vector3> targetLocationList=new List<Vector3>();
    public int index = 0;
    public float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var target in targetList)
        {
            targetLocationList.Add(target.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"{index}");
        var dir = (targetLocationList[index] - transform.position).normalized;
        transform.position += dir * moveSpeed;
        if ((transform.position - targetLocationList[index]).magnitude <= 1.0f)
        {
            if (index >= targetLocationList.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }
}
