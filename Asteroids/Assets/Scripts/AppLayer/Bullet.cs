using System;
using UnityEngine;

namespace AppLayer {
    public class Bullet : AObject {

        #region Fields

        private static float _speed = 1.2f;
        private Rigidbody2D _rigidBody2D;
        private float _totalDistance;

        #endregion
        #region Methods

        public override void Destroy() {}
        public void Destroy(GameObject destroyedObject) {
            gameObject.SetActive(false);
            BulletDestroyEvent?.Invoke(gameObject, destroyedObject);
        }
        public void Create(Vector3 direction, Vector3 position) {
            transform.eulerAngles = direction;
            Coordinates = position;
            _totalDistance = 0;
        }
        public void ActivateOnScene() {
            gameObject.SetActive(true);
            _rigidBody2D.AddForce(transform.up * _speed, ForceMode2D.Impulse);
        }

        #endregion
        #region Event methods

        public override void OnTriggerEnter2D(Collider2D enteredCollider) {
            if (gameObject.CompareTag("EnemyBullet") && enteredCollider.gameObject.CompareTag("Asteroid")) {
                return;
            }
            Destroy(enteredCollider.gameObject);
        }
        private void Start() {
            _audio.Play();
        }
        public void Awake() {
            _rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
            _audio = GetComponent<AudioSource>();
        }
        private void FixedUpdate() {
            Coordinates = transform.position;
            _totalDistance += _speed / 10;
            // 19 - terrain width
            if (_totalDistance >= 19) Destroy(null);
        }

        #endregion
        
        public delegate void BulletDestroyDelegate(GameObject obj, GameObject destroyedObject);
        public event BulletDestroyDelegate BulletDestroyEvent;
    }
}
