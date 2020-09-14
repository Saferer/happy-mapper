using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using I4HUSB;
using System.Threading;
//using System;

public class PlayerMovementlvl2 : MonoBehaviour
{
    public CharacterController2D controller;
    float verticalMove;
    private int coinsCount;
    public Text countText;
    Animator anim;
    Transform transform;
    public float timer = -1f;
    private GameObject coinClone;
    public GameObject coin;
    public GameObject background;
    public GameObject reader;
    float desired = 10;
    private float moveahead = 0f;
    float yposition;


    void Start()
    {


        StaticEMG.Run();
        controller = GetComponent<CharacterController2D>();
        coinsCount = 0;
        SetCountText();
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        // EMGReader readIn = new EMGReader();
        yposition = -8f;//background.GetComponent<Transform>().position.y - (background.GetComponent<SpriteRenderer>().sprite.bounds.size.y /2);
        //Debug.Log(yposition);
        float bgHeight = background.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
    }

    void Update()
    {

        /// Moving
        verticalMove = (float)StaticEMG.Instance.EMG.getPercentage();
        Debug.Log(verticalMove + "EMG");
        // Debug.Log(verticalMove);
        // if(verticalMove > 1f)
        // {
        anim.Play("Run");
        //     controller.Move(40f * Time.fixedDeltaTime);
        //     transform.eulerAngles = new Vector3(0, 0, 0); // Flipped
        // }
        // else if (verticalMove < 1f)
        // {
        //     anim.Play("Run");
        //     controller.Fly(40f * Time.fixedDeltaTime);
        //     transform.eulerAngles = new Vector3(180, 0, 0); // Flipped
        //     GetComponent<Rigidbody2D>().AddForce(Vector3.up * 10f);
        // }

        float actualMove = (background.GetComponent<SpriteRenderer>().sprite.bounds.size.y * verticalMove) - 5.5f;

        if (actualMove < -5f)
        {

            GetComponent<Transform>().position = new Vector3(-1.94f + moveahead, -5f, 0f);

        }
        else if (actualMove < 5f /*&& actualMove >- 6f*/)
        {
            //
            //Debug.Log(actualMove);
            GetComponent<Transform>().position = Vector3.Lerp(GetComponent<Transform>().position, new Vector3(-1.94f + moveahead, actualMove, 0f), .1f);
        }

        else { GetComponent<Transform>().position = new Vector3(-1.94f + moveahead, 5f, 0f); }


        moveahead = moveahead + 0.1f;
        timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("pressed");
            coinClone = Instantiate(coin);
            coinClone.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
            //coin = coinClone;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("map");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("PickUpCoin"))
        {
            other.gameObject.transform.localScale = new Vector3(0, 0, 0);
            coinsCount = coinsCount + 1;
            SetCountText();
            //Destroy(other.gameObject);
        }
    }

    void SetCountText()
    {
        countText.text = "Score: " + coinsCount.ToString();
    }

}
