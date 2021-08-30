using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Fields

    private Animator _animator;
    [SerializeField] private GameObject _spine;
    [SerializeField] private Camera _camera;
    [SerializeField] private TextMeshPro _finishHimText;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _playerModel;
    private int _state;

    #endregion
    #region Methods

    private IEnumerator Finishing() {
        while (true) {
            yield return new WaitForSeconds(2);
            _sword.SetActive(false);
            _weapon.SetActive(true);
        }
    }

    #endregion
    #region Event methods

    private void Start() {
        _animator = _playerModel.GetComponent<Animator>();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_finishHimText.gameObject.activeSelf) {
                _enemy.GetComponent<EnemyController>().Destroy();
                _weapon.SetActive(false);
                _sword.SetActive(true);
                _animator.SetInteger("State", 2);
                StartCoroutine(Finishing());
            }
        }
    }
    private void LateUpdate() {
        
        Plane playerPlane = new Plane(Vector3.up, _spine.transform.position);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        float hitDistance = 0.0f;
        if (playerPlane.Raycast(ray, out hitDistance)) {
            Vector3 targetPoint = ray.GetPoint(hitDistance);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - _spine.transform.position);
            // Giving targetRotation.y to x axis cause otherwise model brakes down.
            // Negative cause it's reversing bone's rotation
            _spine.transform.localRotation = Quaternion.Slerp(_spine.transform.localRotation, 
                new Quaternion(-targetRotation.y, 0, 0, targetRotation.w), 1000 * Time.deltaTime);
        }

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        
        transform.Translate((transform.right * -xAxis + transform.forward * yAxis) / 10);
        
        if (xAxis != 0 || yAxis != 0) {
            _animator.SetInteger("State", 1);
        }
        else {
            _animator.SetInteger("State", 0);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            if (!_finishHimText.gameObject.activeSelf) {
                _finishHimText.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            if (_finishHimText.gameObject.activeSelf) {
                _finishHimText.gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
