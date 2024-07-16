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
    bool touch = false; //タッチ判定フラグ
    bool press = false; //押し込んでいる(プレス)ずっとTrueにする
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
                //　右に進む
                if(!turn)rigidbody2D.AddForce(Vector2.right * moveForce * Time.deltaTime);
                if(turn)rigidbody2D.AddForce(Vector2.right*-moveForce*Time.deltaTime);
        #else
                rigidbody2D.AddForce(Vector2.right*moveForce*Input.acceleration.x*Time.deltaTime);  //ここモバイルになる
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
            Time.timeScale = 0;     //　時間停止

            animator.updateMode = AnimatorUpdateMode.UnscaledTime;      //アニメーションは動く

            StartCoroutine("Wait", 3);  //非同期モード待機用

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
        Time.timeScale = 1;     //標準速度に戻し（1倍速）
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
