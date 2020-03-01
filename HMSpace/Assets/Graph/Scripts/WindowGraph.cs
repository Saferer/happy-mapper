using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] public Sprite circleSprite;
    private GameObject graph;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashMaxTemplate;
    private RectTransform dashMaxInst = null;
    private List<GameObject> gameObjectList;


    private float yMaximum = 0;
    private float yMinimum = 0;
    [SerializeField] public Camera UICam;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashMaxTemplate = graphContainer.Find("dashMax").GetComponent<RectTransform>();
        gameObjectList = new List<GameObject>();

        List<int> valueList = new List<int>() { 0, 100, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20, 99, 20, 56, 30, 22, 88, 77, 13, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20
        , 100, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20, 99, 20, 56, 30, 22, 88, 77, 13, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20
        , 100, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20, 99, 20, 56, 30, 22, 88, 77, 13, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20};

        ShowGraph(valueList);

    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(graphContainer, Input.mousePosition, UICam, out localPoint);
            float yPos = (localPoint.y / graphContainer.sizeDelta.y) * (yMaximum - yMinimum) + yMinimum;
            Debug.Log(yPos);
            if(yPos < yMaximum && yPos > yMinimum)
            {
                if (dashMaxInst == null)
                {
                    dashMaxInst = Instantiate(dashMaxTemplate);
                    dashMaxInst.SetParent(graphContainer, false);
                    dashMaxInst.gameObject.SetActive(true);
                }


                dashMaxInst.anchoredPosition = new Vector2(0, (yPos - yMinimum) / (yMaximum - yMinimum) * graphContainer.sizeDelta.y);
            }
            
        }
        
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;

    }
    
    private void ShowGraph(List<int> valueList, int maxVisibleValueAmount = -1)
    {

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;
        yMaximum = valueList[0];
        yMinimum = valueList[0];

        
        if(maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }

        foreach(GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);

        }
        gameObjectList.Clear();

        


        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            int value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }
        float yDifference = yMaximum - yMinimum;
        if(yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum = yMaximum + ((yDifference) * 0.1f);
        yMinimum = yMinimum - ((yDifference) * 0.1f);



        int xIndex = 0;
        float xSize = graphWidth/(maxVisibleValueAmount + 1);
        GameObject lastCircle = null;
        for(int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circle = CreateCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(circle);
            if(lastCircle != null)
            {
                GameObject gameObjectDotConnection = CreateDotConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, circle.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(gameObjectDotConnection);
            }
            lastCircle = circle;

            /*RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);
            labelX.GetComponent<Text>().text = i.ToString();
            labelX.localScale = new Vector2(1, 1);
            labelX.transform.localPosition = new Vector3(labelX.transform.localPosition.x, labelX.transform.localPosition.y, 1);
            gameObjectList.Add(labelX.gameObject);*/

            xIndex++;
        }
        int separatorCount = 10;
        for (int j = 0; j <= separatorCount; j++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = j * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-20f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(yMinimum + (normalizedValue * (yMaximum - yMinimum))).ToString();
            labelY.localScale = new Vector2(1, 1);
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(0, normalizedValue * graphHeight);
            dashX.localScale = new Vector2(1, 1);
            gameObjectList.Add(dashX.gameObject);
        }
    }

    private GameObject CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPosB - dotPosA).normalized;
        float distance = Vector2.Distance(dotPosA, dotPosB);
        rectTransform.sizeDelta = new Vector2(distance, 2f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchoredPosition = dotPosA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        return gameObject;
    }
}
