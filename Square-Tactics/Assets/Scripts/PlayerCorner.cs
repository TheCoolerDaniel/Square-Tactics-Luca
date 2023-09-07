using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCorner : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += direction * Time.deltaTime;
    }
}
