using System;
using System.Collections.Generic;
using AppLayer;
using TMPro;
using UnityEngine;

namespace Menus {
    public class UI : MonoBehaviour {

        private Player _player;
        [SerializeField] private TextMeshPro _score;
        [SerializeField] private List<GameObject> _ships;
        [SerializeField] private GameObject _menuBackground;
        
        private void Start() {
            _player = Player.Instance;
        }
        private void Update() {
            _score.text = _player.Score.ToString();
            switch (_player.Ships) {
                case 5:
                    break;
                case 4:
                    _ships[0].SetActive(false);
                    break;
                case 3:
                    _ships[1].SetActive(false);
                    break;
                case 2:
                    _ships[2].SetActive(false);
                    break;
                case 1:
                    _ships[3].SetActive(false);
                    break;
                case 0:
                    foreach (var i in _ships) {
                        i.SetActive(true);
                    }
                    break;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape)) {
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
}
