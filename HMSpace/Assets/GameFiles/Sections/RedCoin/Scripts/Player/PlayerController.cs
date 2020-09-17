using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 ScreenTopPosition;

    private Vector3 ScreenBotPosition;
    private float MinimumPositionOffset = 1.2f;
    private float MaximumPositionOffset = 0.5f;
    private float Speed = 10.0f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        StaticEMG.Run();
        ScreenTopPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        ScreenBotPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            RedCoinScore.Instance.increment();
            Destroy(other.gameObject);
        }
    }

    void MovePlayer()
    {
        //Use camera as ratio and offset by amount to allow for flooring the character
        float newYPos = (ScreenTopPosition.y - ScreenBotPosition.y - MinimumPositionOffset) * 0.5f * (float)StaticEMG.Instance.GetPercentage() + ScreenBotPosition.y + MinimumPositionOffset;
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(transform.position.x, newYPos, transform.position.z);
        SetAnimation(newPos.y - oldPos.y);
        float dist = Vector3.Distance(newPos, oldPos);
        float distToCover = (Time.deltaTime) * Speed;
        transform.position = Vector3.Lerp(oldPos, newPos, distToCover / dist);
    }

    void SetAnimation(float yDir)
    {
        if (StaticEMG.Instance.GetPercentage() < 0.07f)
        {
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("RedAnimation/Run");
        }
        else if (yDir > 0)
        {
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("RedAnimation/Jump");
        }
        else
        {
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("RedAnimation/Fall");
        }
    }
}
