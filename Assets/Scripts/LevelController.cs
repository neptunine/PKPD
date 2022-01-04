using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class LevelController : MonoBehaviour
    {
        public TextAsset
            wordListFile;

        [SerializeField]
        Text text;

        private string[]
            words;

        private string
            targetWord;

        private void Awake()
        {
            words = wordListFile.text.Split("\n"[0]);
        }

        void Start()
        {
            
        }

        void Update()
        {

        }

        public void Initialize()
        {
            targetWord = words[Random.Range(0, words.Length)];
            text.text = targetWord;
            Debug.Log($"[{this.name}] picked '{targetWord}'");
        }
    }
}