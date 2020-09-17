using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlantSpawner : MonoBehaviour
{
    [System.Serializable]
    public class TimerRange
    {
        public float start;
        public float end;
        public float timer = 3f;
    }
    public List<TimerRange> timerRange;
    public List<BackgroundPlantTransform> listOfSprites;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < listOfSprites.Count; i++)
        {
            timerRange[i].timer -= Time.deltaTime;
            if (timerRange[i].timer <= 0)
            {
                BackgroundPlantTransform sprite = Instantiate(listOfSprites[i], new Vector3(18f, 0f, 0f), listOfSprites[i].transform.rotation);
                sprite.gameObject.SetActive(true);
                timerRange[i].timer = Random.Range(timerRange[i].start, timerRange[i].end);
            }
        }
    }
}
