using System;
using TMPro;
using UnityEngine;

namespace Menus {
    public abstract class AEnterAnimation : MonoBehaviour {

        private TextMeshPro _buttonText;

        private void Start() {
            _buttonText = GetComponent<TextMeshPro>();
        }

        private void OnMouseEnter() {
            _buttonText.fontSize = 10;
        }
        private void OnMouseExit() {
            _buttonText.fontSize = 12;
        }
    }
}
