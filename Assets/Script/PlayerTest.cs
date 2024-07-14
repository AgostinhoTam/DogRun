using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float jumpForce = 6.0f;
    [SerializeField] float moveForce = 60.0f;
    bool touch = false; //タッチ判定フラグ
    bool press = false; //押し込んでいる(プレス)ずっとTrueにする
    bool isGround = false;
    Animator animator;
    Rigidbody2D rigidbody2D;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsGround", isGround);
        touch = false;
        press = false;

        if(Input.touchCount >0) //タッチしている指の数が0より大きければ
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began) //タッチはじめ
            {
                touch = true;
            }
        }
        //　エディタの時はマウスボタン
        if (Input.GetMouseButtonDown(0))
        {
            touch = true;
        }
        if (Input.GetMouseButton(0))
        {
            press = true;
        }
        if (touch)
        {
            rigidbody2D.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);
        }
        if (press)
        {
            rigidbody2D.AddForce(Vector2.up * 1000.0f * Time.deltaTime);
        }

        #if UNITY_EDITOR
                //　右に進む
                rigidbody2D.AddForce(Vector2.right * moveForce * Time.deltaTime);
        #else
                rigidbody2D.AddForce(Vector2.right*moveForce*Input.acceleration.x*Time.deltaTime);  //ここモバイルになる
        #endif
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Field") isGround = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Field")isGround = false;
    }

}
