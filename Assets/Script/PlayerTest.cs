using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float jumpForce = 6.0f;
    [SerializeField] float moveForce = 60.0f;
    bool touch = false; //�^�b�`����t���O
    bool press = false; //��������ł���(�v���X)������True�ɂ���
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

        if(Input.touchCount >0) //�^�b�`���Ă���w�̐���0���傫�����
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began) //�^�b�`�͂���
            {
                touch = true;
            }
        }
        //�@�G�f�B�^�̎��̓}�E�X�{�^��
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
                //�@�E�ɐi��
                rigidbody2D.AddForce(Vector2.right * moveForce * Time.deltaTime);
        #else
                rigidbody2D.AddForce(Vector2.right*moveForce*Input.acceleration.x*Time.deltaTime);  //�������o�C���ɂȂ�
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
