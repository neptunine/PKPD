using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("test2", 666);
        var a2 = PlayerPrefs.GetInt("test2");
        Debug.Log(a2);
    }

    
}
