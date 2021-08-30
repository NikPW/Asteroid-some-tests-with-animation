using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus {
    public class Play : AEnterAnimation {
        private void OnMouseDown() {
            if (Input.GetMouseButtonDown(0)) {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
