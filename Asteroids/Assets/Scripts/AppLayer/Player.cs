using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using Services.Handling;
using UnityEngine;

namespace AppLayer {
    public class Player : AObject {

        #region Fields
        
        private int _ships;
        private Rigidbody2D _rigidBody2D;
        private Animation _animation;
        private float _speed;
        private bool _isGod;
        private ObjectPool _bulletsPool;
        [SerializeField] private GameObject _bulletPrefab;
        private int _spawnedBullets;
        public Handling Handling;

        // Singleton
        public static Player Instance;

        #endregion
        #region Properties

        public int Ships {
            get => _ships;
        }
        public new Vector3 Direction {
            get => transform.eulerAngles;
            private set => transform.eulerAngles = value;
        }
        public bool IsGod {
            get => _isGod;
        }
        public int Score { get; private set; }

        #endregion
        #region Methods
        
        public override void Destroy() {
            --_ships;
        }
        private void Create() {
            _rigidBody2D.velocity = Vector2.zero;
            Coordinates = new Vector2(0, 0);
            _animation.Stop();
            _animation.Play();
            SetGodMod();
        }
        private void SetGodMod() {
            if (_isGod == false) {
                _isGod = true;
            }
            else if (_isGod && (int)Time.time % 4 >= 3) {
                _isGod = false;
            }
        }
        public void DestroyBullet(GameObject obj, GameObject destroyedObject) {
            try {
                _bulletsPool.ReturnObjectInPool(obj);
            }
            catch { }
            finally {
                if (destroyedObject != null) {
                    if (destroyedObject.gameObject.CompareTag("Asteroid")) {
                        switch (destroyedObject.GetComponent<Asteroid>().Size) {
                            case AsteroidsSizes.BIG:
                                Score += 20;
                                break;
                            case AsteroidsSizes.MIDDLE:
                                Score += 50;
                                break;
                            case AsteroidsSizes.SMALL:
                                Score += 100;
                                break;
                        }
                    }
                    else if (destroyedObject.gameObject.CompareTag("UFO")) {
                        Score += 200;
                    }
                }
            }
        }
        private void SpawnBullet() {
            if (_spawnedBullets <= 3) {
                GameObject bullet = _bulletsPool.GetObjectFromPool();
                Bullet b = bullet.GetComponent<Bullet>();
                b.Create(Direction, Coordinates);
                b.ActivateOnScene();
            }
        }
        
        #endregion
        #region Event methods
        
        public override void OnTriggerEnter2D(Collider2D enteredCollider) {
            if (enteredCollider.gameObject.CompareTag("Asteroid") || enteredCollider.gameObject.CompareTag("UFO") ||
                enteredCollider.gameObject.CompareTag("EnemyBullet")) {
                if (!_isGod) {
                    Destroy();
                    Create();
                }
            }
        }
        private void Start() {
            _rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
            _animation = gameObject.GetComponent<Animation>();
            _ships = 5;
            _speed = 1f;
            _spawnedBullets = 0;
            Score = 0;
            Create();
        }
        private void Update() {
            if (_ships == 0) ShipsOutEvent?.Invoke();
            if (Handling.Attack() && _spawnedBullets <= 3) {
                ++_spawnedBullets;
                SpawnBullet();
            }
        }
        private void FixedUpdate() {
            Handling.AddForce(_rigidBody2D, _speed);
            Handling.AddTorque(_rigidBody2D, 0.2f);
            Coordinates = transform.position;
            if (_isGod && (int)Time.time % 4 >= 3) _isGod = false;
            if ((int)Time.time % 2 == 1) _spawnedBullets = 0;
        }
        private void Awake() {
            // Creating bullets pool
            Handling = new KeyboardHandling();
            Handling.HandlingType = HandlingTypes.KEYBOARD;
            List<GameObject> bullets = new List<GameObject>();
            for (int i = 0; i < 30; ++i) {
                bullets.Add(Instantiate(_bulletPrefab));
                bullets[i].SetActive(false);
                bullets[i].gameObject.layer = 7;
                bullets[i].gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                bullets[i].gameObject.GetComponent<Bullet>().BulletDestroyEvent += DestroyBullet;
            }
            _bulletsPool = new ObjectPool(bullets);
            _audio = GetComponent<AudioSource>();
            Instance = this;
        }

        #endregion

        public delegate void ShipsOutDelegate();
        public event ShipsOutDelegate ShipsOutEvent;
    }
}
