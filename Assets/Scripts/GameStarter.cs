using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField]
        GameController
            controller;

        public int
            mode;

        public void StartGame()
        {
            controller.InitializeGame(mode);
        }
    }
}