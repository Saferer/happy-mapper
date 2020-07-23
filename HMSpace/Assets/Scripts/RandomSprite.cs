using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{

    public Sprite[] plantSprite;
    int selectedSprite;
    float spriteBottom;
    public GameObject background;
    public float timer = 3f;
    private GameObject plantClone;
    public GameObject plant;
    // Start is called before the first frame update
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            selectedSprite = Random.Range(1, 13);
            spriteBottom = -6f + ((plantSprite[selectedSprite].bounds.size).y / 2);
            plantClone = Instantiate(plant, new Vector3(background.transform.position.x + (background.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2), spriteBottom, 0f), Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            plantClone.GetComponent<SpriteRenderer>().sprite = plantSprite[selectedSprite];
            timer = Random.Range(0.5f, 2f);
        }
    }

}
