using Services;
using TMPro;
using UnityEngine;

public class GameOverGetScore : MonoBehaviour {
    private void Start() {
        gameObject.GetComponent<TextMeshPro>().text += LastGameScore.Score;
    }
}
