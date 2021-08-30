using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField] private GameObject _animator;
    private bool _isDestroyed;

    public void Destroy() {
        _animator.GetComponent<Animator>().enabled = false;
        _isDestroyed = true;
    }
    private IEnumerator Respawn() {
        while (true) {
            yield return new WaitForSeconds(3);
            if (_isDestroyed) {
                yield return new WaitForSeconds(2);
                transform.position = new Vector3(Random.Range(-45, 45), 0.15f, Random.Range(-45, 45));
                _animator.GetComponent<Animator>().enabled = true;
                _isDestroyed = false;
            }
        }
    }
    
    void Start() {
        _isDestroyed = false;
        StartCoroutine(Respawn());
    }
}
