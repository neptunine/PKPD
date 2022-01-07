using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LevelController : MonoBehaviour
    {
        public TextAsset
            wordFile;

        public int
            mode;

        [SerializeField]
        Text text;

        private string[]
            words;

        private string
            targetWord;

        private void Awake()
        {
            words = wordFile.text.Split("\n"[0]);
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
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