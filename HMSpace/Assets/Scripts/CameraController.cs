using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{

    public GameObject player;
    private Vector3 offset = new Vector3(0f, 0f, -3f);

    // Start is called before the first frame update
    void Start()
    {
        //offset = (0f, 0f,-3f);
}

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y,-3f);
    }
}
