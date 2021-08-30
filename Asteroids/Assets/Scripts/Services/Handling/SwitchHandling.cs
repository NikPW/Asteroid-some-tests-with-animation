using System;
using AppLayer;
using Menus;
using TMPro;
using UnityEngine;

namespace Services.Handling {
    public class SwitchHandling : AEnterAnimation {

        private Player _player;
        private TextMeshPro _text;
        
        private void OnMouseDown() {
            _player.gameObject.SetActive(false);
            if (_player.Handling.HandlingType == HandlingTypes.KEYBOARD) {
                _player.Handling = new MouseHandling();
                _player.Handling.HandlingType = HandlingTypes.MOUSE_KEYBOARD;
                _text.text = "Mouse";
            }
            else if (_player.Handling.HandlingType == HandlingTypes.MOUSE_KEYBOARD) {
                _player.Handling = new KeyboardHandling();
                _player.Handling.HandlingType = HandlingTypes.KEYBOARD;
                _text.text = "Keyboard";
            }
            _player.gameObject.SetActive(true);
        }
        private void Awake() {
            _player = Player.Instance;
            _text = GetComponent<TextMeshPro>();
        }
    }
}
