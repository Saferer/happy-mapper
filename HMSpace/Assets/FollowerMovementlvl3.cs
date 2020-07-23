using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMovementlvl3 : MonoBehaviour
{
  public GameObject player;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
    anim = GetComponent<Animator>();
        }

    // Update is called once per frame
    void LateUpdate()
    {
              anim.Play("Run");
      GetComponent<Transform>().position= new Vector3(player.GetComponent<Transform>().position.x-2, player.GetComponent<Transform>().position.y-.5f, 0f);
    }
}
