using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera : MonoBehaviour
{
    //　プレイヤーからオフセット
    [SerializeField] Vector3 offset = new Vector3(0.0f, 0.0f, -10.0f);
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //  Playerを入れる
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0.0f) + offset;        
    }
}
