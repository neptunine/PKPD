using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialog : MonoBehaviour
{
    [Header("UI")]
    public Text textLabel;
    public Image faceImage;

    [Header("text")]
    public TextAsset textFile;
    public int index;



    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        GetTextFormFile(textFile);

    }
   
    private void OnEnable()
    {
        textLabel.text = textList[index];
        index++;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            return;


        }
        if (Input.GetKeyDown(KeyCode.R))//< wrong
        {
            textLabel.text = textList[index];
            index++;
        }
        void GetTextFormFile(TextAsset file)
        {
            textList.Clear();
            index = 0;

            var LineDate = file.text.Split('\n');


            foreach (var line in LineDate)
            {
                textList.Add(line);
            }
        }
    }
    
