using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class SideMenu : MonoBehaviour
    {
        void Start()
        {
            gameObject.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}