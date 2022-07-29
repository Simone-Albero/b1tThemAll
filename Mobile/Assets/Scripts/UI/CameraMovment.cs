using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{

    public Transform playePos;


    // Start is called before the first frame update
    void Start()
    {
        //controllo camera
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playePos.transform.position.x, playePos.transform.position.y, transform.position.z);
    }
}
