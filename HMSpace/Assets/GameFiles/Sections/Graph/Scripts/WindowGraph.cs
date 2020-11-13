using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BoolWrapper
{
    public bool value
    {
        get; set;
    }
    public BoolWrapper(bool value)
    {
        this.value = value;
    }
}

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
    private BoolWrapper recording;
    private List<float> valueList;
    //public bool startRecording = false;
    private float yMaximum = 0;
    private float yMinimum = 0;
    [SerializeField] public Camera UICam;
    public TMP_InputField TimeToRecord;
    public TMP_InputField MaxPercentageInput;
    public TMP_Dropdown COMPortDropDown;

    private void Awake()
    {

        recording = new BoolWrapper(false);
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashMaxTemplate = graphContainer.Find("dashMax").GetComponent<RectTransform>();
        gameObjectList = new List<GameObject>();
        //valueList = new List<float>() { 0, 100, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20, 99, 20, 56, 30, 22, 88, 77, 13, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20
        // , 100, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20, 99, 20, 56, 30, 22, 88, 77, 13, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20
        // , 100, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20, 99, 20, 56, 30, 22, 88, 77, 13, 20, 99, 20, 56, 30, 22, 88, 77, 13, 95, 86, 60, 66, 22, 59, 75, 5, 20};
        valueList = new List<float>();
    }

    private void Start()
    {
        //List<float> valueList = new List<float>(MathConversionUtil.DoubleArrayToFloat(StaticEMG.Instance.EMG.getCalibrationArray()));
        addDropDownOptions();
        string[] ports = StaticEMG.Instance.EMG.GetPortNames();
        if(ports.Length > 0)
        {
            StaticEMG.Instance.EMG.SetPort(ports[0]);
        }


    }

    private void Update()
    {
        // if (startRecording)
        // {
        //     Record(15);
        // }
        Debug.Log("WindowGraph: Update: Current Goal is " + StaticEMG.Instance.EMG.getGoal());
        if (recording.value)
            ShowGraph(StaticEMG.Instance.GetRecordedValues());
        if (Input.GetMouseButton(0))
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(graphContainer, Input.mousePosition, UICam, out localPoint);
            float yPos = ((localPoint.y) / graphContainer.sizeDelta.y) * (yMaximum - yMinimum) + yMinimum;
            //Debug.Log(yPos);
            if (yPos < yMaximum && yPos > yMinimum)
            {
                if (dashMaxInst == null)
                {
                    dashMaxInst = Instantiate(dashMaxTemplate);
                    dashMaxInst.SetParent(graphContainer, false);
                    dashMaxInst.gameObject.SetActive(true);
                }
                float percentageLabel = ((yPos - yMinimum) / (yMaximum - yMinimum)) * 100f;
                MaxPercentageInput.text = percentageLabel.ToString("0.00");
                StaticEMG.Instance.EMG.setGoal(yPos);
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

    private void ShowGraph(List<float> valueList, int maxVisibleValueAmount = -1)
    {
        if (valueList.Count < 1)
        {
            return;
        }
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;
        yMaximum = valueList[0];
        yMinimum = valueList[0];


        if (maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }

        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);

        }
        gameObjectList.Clear();




        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float value = valueList[i];
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
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        //yMaximum = yMaximum + ((yDifference) * 0.1f);
        //yMinimum = yMinimum - ((yDifference) * 0.1f);



        int xIndex = 0;
        float xSize = graphWidth / (maxVisibleValueAmount + 1);
        GameObject lastCircle = null;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i+=3)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circle = CreateCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(circle);
            if (lastCircle != null)
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

            xIndex+=3;
        }
        int separatorCount = 10;
        for (int j = 0; j <= separatorCount; j++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = j * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-20f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = string.Format("{0:0.00}", (float)j / separatorCount);//string.Format("{0:0.00}", yMinimum + (normalizedValue * (yMaximum - yMinimum)));
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

    public void Record()
    {
        StaticEMG.Run();
        recording.value = true;
        //startRecording = false;
        int timeSeconds;
        try
        {
            timeSeconds = int.Parse(TimeToRecord.text);
        }
        catch
        {
            timeSeconds = 0;
        }
        Debug.Log("WindowGraph: Record: Starting Recording");
        StaticEMG.Instance.StartRecord(timeSeconds, recording);
    }

    public void setMaxButton()
    {
        StaticEMG.Instance.EMG.setGoal((yMaximum - yMinimum) * (float.Parse(MaxPercentageInput.text) * 0.01f));
        //Debug.Log(StaticEMG.Instance.EMG.getPercentage());
        if (dashMaxInst == null)
        {
            dashMaxInst = Instantiate(dashMaxTemplate);
            dashMaxInst.SetParent(graphContainer, false);
            dashMaxInst.gameObject.SetActive(true);
        }
        dashMaxInst.anchoredPosition = new Vector2(0, ((float.Parse(MaxPercentageInput.text) * 0.01f) * graphContainer.sizeDelta.y));
    }

    public void addDropDownOptions()
    {   
        COMPortDropDown.ClearOptions();
        COMPortDropDown.AddOptions(new List<string>(StaticEMG.Instance.EMG.GetPortNames()));
    }

    public void setCOMPort()
    {
        Debug.Log("WindowGraph: setCOMPort: Set called");
        StaticEMG.Stop();
        StaticEMG.Instance.EMG.SetPort(COMPortDropDown.options[COMPortDropDown.value].text);
    }

    public void accept()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
