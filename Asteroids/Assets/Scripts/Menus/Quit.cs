using TMPro;
using UnityEngine;

namespace Menus {
    public class Quit : AEnterAnimation {
        private void OnMouseDown() {
            Application.Quit();
        }
    }
}
