using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AppLayer {
    public class Asteroid : AObject {

        #region Fields

        private Vector2 _speed;
        private AsteroidsSizes _size;
        private Rigidbody2D _rigidBody2D;
        private static Player _player;
        private Animation _animation;
        [SerializeField] private List<AudioClip> _audioClips;

        #endregion
        #region Properties

        public AsteroidsSizes Size { get => _size; }

        #endregion
        #region Methods
        
        // Just to delete Destroy() method from AObject
        public override void Destroy() {}
        // Bringing asteroid into the factory condition and causes event
        public void Destroy(GameObject bullet) {
            _animation.Play();
            gameObject.SetActive(false);
            DestroyAsteroidEvent?.Invoke((_size, gameObject, Coordinates, Direction, bullet));
        }
        public void Create(Vector3 direction, Vector3 position, Vector2 speed, AsteroidsSizes size = AsteroidsSizes.BIG) {
            _speed = new Vector2(Random.Range(15, 50), Random.Range(15, 50));
            Direction = direction;
            Coordinates = position;
            transform.eulerAngles = Direction;
            _size = size;
            float newAsteroidsSize;
            switch (_size) {
                case AsteroidsSizes.BIG:
                    newAsteroidsSize = Random.Range(1f, 1.4f);
                    transform.localScale = new Vector3(newAsteroidsSize, newAsteroidsSize, 1);
                    break;
                case AsteroidsSizes.MIDDLE:
                    newAsteroidsSize = Random.Range(0.5f, 0.8f);
                    transform.localScale = new Vector3(newAsteroidsSize, newAsteroidsSize, 1);
                    break;
                case AsteroidsSizes.SMALL:
                    newAsteroidsSize = Random.Range(0.1f, 0.3f);
                    transform.localScale = new Vector3(newAsteroidsSize, newAsteroidsSize, 1);
                    break;
            }
        }
        public void ActivateOnScene() {
            gameObject.SetActive(true);
            _rigidBody2D.AddForce(transform.up * _speed * 2f);
        }
        private IEnumerator PlayAnimationAndCallDestroy(GameObject bullet) {
            while (true) {
                switch (Size) {
                    case AsteroidsSizes.BIG:
                        _audio.clip = _audioClips[0];
                        break;
                    case AsteroidsSizes.MIDDLE:
                        _audio.clip = _audioClips[1];
                        break;
                    case AsteroidsSizes.SMALL:
                        _audio.clip = _audioClips[2];
                        break;
                }
                _audio.Play();
                _animation.Play();
                yield return new WaitForSeconds(1);
                Destroy(bullet);
            }
        }

        #endregion
        #region Event methods

        public override void OnTriggerEnter2D(Collider2D enteredCollider) {
            if (enteredCollider.gameObject.CompareTag("Bullet") || (enteredCollider.gameObject.CompareTag("SpaceShip") ||
                (enteredCollider.gameObject.CompareTag("EnemyBullet")) && !_player.IsGod)) {
                StartCoroutine(PlayAnimationAndCallDestroy(enteredCollider.gameObject));
            }
        }
        public void Awake() {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _animation = GetComponent<Animation>();
            _audio = GetComponent<AudioSource>();
            _player = Player.Instance;
        }
        private void FixedUpdate() {
            Coordinates = transform.position;  
        }

        #endregion

        public delegate void DestroyAsteroidDelegate((AsteroidsSizes size, GameObject obj, 
            Vector3 coordinates, Vector3 direction, GameObject bullet) asteroidInfo);
        public event DestroyAsteroidDelegate DestroyAsteroidEvent;
    }
}
