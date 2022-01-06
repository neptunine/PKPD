using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        GameObject
            MainUI,
            gameUI,
            optionsUI,
            creditsUI;

        private RectTransform
            _mainRect,
            _gameRect,
            _optionsRect,
            _creditsRect;

        private void Start()
        {
            _mainRect = MainUI.GetComponent<RectTransform>();
            _gameRect = gameUI.GetComponent<RectTransform>();
            _optionsRect = optionsUI.GetComponent<RectTransform>();
            _creditsRect = creditsUI.GetComponent<RectTransform>();
        }

        private void Update()
        {

        }
      

        public void Exit()
        {
            Application.Quit();
        }


    }
}