using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward); // 카메라와 같은 방향을 보게 함
    }
}
