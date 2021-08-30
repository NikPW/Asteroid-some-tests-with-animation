using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AppLayer {
    public class UFO : AObject {

        #region Fields

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _speed = 2f;
        private ObjectPool _bulletsPool;
        private Animation _animation;
        private Rigidbody2D _rigidbody2D;
        private Player _player;
        private bool _canShoot;

        // Singleton
        public static UFO Instance;

        #endregion
        #region Properties

        public new Vector3 Coordinates {
            get => transform.position;
            set {
                //_rigidbody2D.velocity = Vector3.zero;
                if (transform.position.x >= 9.5f) {
                    _rigidbody2D.AddForce(-transform.up * _speed, ForceMode2D.Impulse);
                }
                else if (transform.position.x <= -9.5f) {
                    _rigidbody2D.AddForce(transform.up * _speed, ForceMode2D.Impulse);
                }
                else {
                    transform.position = value;
                }
            }
        }

        #endregion
        #region Methods

        public void Create(Vector3 position, int direction) {
            gameObject.SetActive(true);
            Coordinates = position;
            _audio.Play();
            if (direction == 0) {
                _rigidbody2D.AddForce(transform.up * _speed, ForceMode2D.Impulse);
            }
            else {
                _rigidbody2D.AddForce(-transform.up * _speed, ForceMode2D.Impulse);
            }
        }
        public override void Destroy() {
            gameObject.SetActive(false);
        }
        public void DestroyBullet(GameObject obj, GameObject destroyedObject) {
            _bulletsPool.ReturnObjectInPool(obj);
        }
        private IEnumerator CanShootCooldown() {
            while (true) {
                yield return new WaitForSeconds(Random.Range(2, 5));
                _canShoot = true;
            }
        }
        private IEnumerator Lifetime() {
            while (true) {
                yield return new WaitForSeconds(10);
                Destroy();
            }
        }
        private IEnumerator PlayAnimationAndDestroy() {
            while (true) {
                _animation.Play();
                yield return new WaitForSeconds(1);
                Destroy();
            }
        }

        #endregion
        #region Event methods

        public override void OnTriggerEnter2D(Collider2D enteredCollider) {
            StartCoroutine(PlayAnimationAndDestroy());
        }
        private void OnEnable() {
            StartCoroutine(Lifetime());
            StartCoroutine(CanShootCooldown());
            _player = Player.Instance;
        }
        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animation = GetComponent<Animation>();
            _audio = GetComponent<AudioSource>();
            
            List<GameObject> bullets = new List<GameObject>();
            for (int i = 0; i < 10; ++i) {
                bullets.Add(Instantiate(_bulletPrefab));
                bullets[i].SetActive(false);
                bullets[i].gameObject.layer = 10;
                bullets[i].gameObject.tag = "EnemyBullet";
                bullets[i].gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                bullets[i].gameObject.GetComponent<Bullet>().BulletDestroyEvent += DestroyBullet;
            }
            _bulletsPool = new ObjectPool(bullets);
            
            Instance = this;
            gameObject.SetActive(false);
        }
        private void Update() {
            if (_canShoot) {
                GameObject bullet = _bulletsPool.GetObjectFromPool();
                Bullet b = bullet.GetComponent<Bullet>();
                
                Debug.Log(_player);
                Vector3 dir = gameObject.transform.position - _player.Coordinates;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
                
                b.Create(new Vector3(0, 0, angle), transform.position);
                b.ActivateOnScene();
                _canShoot = false;
            }
        }
        private void FixedUpdate() {
            Coordinates = transform.position;
        }

        #endregion
    }
}
