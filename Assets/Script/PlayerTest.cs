using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float jumpForce = 6.0f;
    [SerializeField] float moveForce = 60.0f;
    [SerializeField] AudioClip jumpAudio;
    [SerializeField] AudioClip landingAudio;
    [SerializeField] AudioClip boneAudio;
    [SerializeField] GameObject effexplosion;
    [SerializeField] Scores score;
    private BGMMgr BGM;

    AudioSource audioSource;
    bool touch = false; //�^�b�`����t���O
    bool press = false; //��������ł���(�v���X)������True�ɂ���
    bool isGround = false;
    private bool turn = false;
    private bool turnCoolDown = false;
    public float turnCoolDownTime = 1.0f;
    Animator animator;
    Rigidbody2D rigidbody2D;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        GameObject bgmobject = GameObject.Find("BGM");
        if(bgmobject != null )
        {
            BGM = bgmobject.GetComponent<BGMMgr>();
        }
        else
        {
            Debug.Log("Music nullptr");
        }
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
        if (touch && isGround)
        {
            Debug.Log("Pressed");
            rigidbody2D.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpAudio);
        }
        //if (press)
        //{
        //    rigidbody2D.AddForce(Vector2.up * 1000.0f * Time.deltaTime);
        //}

        #if UNITY_EDITOR
                //�@�E�ɐi��
                if(!turn)rigidbody2D.AddForce(Vector2.right * moveForce * Time.deltaTime);
                if(turn)rigidbody2D.AddForce(Vector2.right*-moveForce*Time.deltaTime);
        #else
                rigidbody2D.AddForce(Vector2.right*moveForce*Input.acceleration.x*Time.deltaTime);  //�������o�C���ɂȂ�
        #endif
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Field")
    //    {
    //        isGround = true;
    //        audioSource.PlayOneShot(landingAudio);
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Field")
    //    {
    //        isGround = false;
    //        audioSource.PlayOneShot(jumpAudio);
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bone")
        {
            if (!turn) { 
                transform.localScale *= 0.8f;
            }
            else
            {
                transform.localScale *= 1.2f;
            }
            audioSource.PlayOneShot(boneAudio);
            GameObject explosion = GameObject.Instantiate(effexplosion, collision.gameObject.transform.position, Quaternion.identity);
            GameObject.Destroy(explosion, 1.0f);
            GameObject.Destroy(collision.gameObject);
            score.AddScore(5);
        }
        if (collision.gameObject.tag == "Goal")
        {
            Time.timeScale = 0;     //�@���Ԓ�~

            animator.updateMode = AnimatorUpdateMode.UnscaledTime;      //�A�j���[�V�����͓���

            StartCoroutine("Wait", 3);  //�񓯊����[�h�ҋ@�p

            PlayerPrefs.SetInt("Scores",score.score);
            PlayerPrefs.Save();
        }
        if(collision.gameObject.tag == "Field")
        {
            Debug.Log("isGround");
            isGround = true;
            audioSource.PlayOneShot(landingAudio);
        }
        if(collision.gameObject.tag == "Turn" && !turnCoolDown)
        {

            StartCoroutine(TurnCoolDown());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Field") { 
            Debug.Log("isOff");
            isGround = false;
           
        }
    }
    IEnumerator Wait(float sec)
    {
        yield return new WaitForSecondsRealtime(sec);
        Time.timeScale = 1;     //�W�����x�ɖ߂��i1�{���j
        SceneManager.LoadScene("Result");
    }

    IEnumerator TurnCoolDown()
    {
        turnCoolDown = true;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        if (!turn)
        {
            turn = true;
            BGM.ChangeRevBGM();
        }
        else
        {
            turn = false;
            BGM.ChangeBGM();
        }
        yield return new WaitForSeconds(turnCoolDownTime);

        turnCoolDown = false;
    }
}
