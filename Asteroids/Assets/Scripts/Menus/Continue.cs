using AppLayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus {
    public class Continue : AEnterAnimation {
    
        [SerializeField] private GameObject _menuBackground;
        private Player _player;
    
        private void Awake() {
            _player = Player.Instance;
        }
        private void OnMouseDown() {
            if (_menuBackground.activeSelf) {
                _menuBackground.SetActive(false);
                _player.GetComponent<Player>().enabled = true;
            }
            else {
                _menuBackground.SetActive(true);
                _player.GetComponent<Player>().enabled = false;
            }
        }
    }
}
