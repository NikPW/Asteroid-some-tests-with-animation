using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppLayer;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Terrain : MonoBehaviour {

    #region Fields

    private ObjectPool _asteroidsPool;
    private List<GameObject> _activeAsteroids;
    [SerializeField] private List<GameObject> _asteroidPrefabs;
    private int _prevSpawnedAsteroidsAmount;
    private Player _player;
    private UFO _ufo;
    private bool _canSpawnUfo;

    // Singleton
    public static Terrain Instance;

    #endregion
    #region Methods

    // Handler of "DestroyAsteroidEvent"
    private void DestroyAndSpawnSmallerAsteroid((AsteroidsSizes size, GameObject obj,
        Vector3 coordinates, Vector3 direction, GameObject bullet) asteroidInfo) {

        if (asteroidInfo.bullet.CompareTag("Bullet")) {
            _player.DestroyBullet(asteroidInfo.bullet, asteroidInfo.obj);
        }

        SetupAsteroid(asteroidInfo.obj.gameObject.GetComponent<Asteroid>());
        try {
            _asteroidsPool.ReturnObjectInPool(asteroidInfo.obj);
        }
        catch {
            return;
        }

        _activeAsteroids.RemoveAt(0);
        if (asteroidInfo.size + 1 > AsteroidsSizes.SMALL) {
            return;
        }

        Vector2 speed = new Vector2(Random.Range(15, 50), Random.Range(15, 50));

        for (int i = 0; i < 2; ++i) {
            GameObject asteroid = _asteroidsPool.GetObjectFromPool(i);
            Asteroid a = asteroid.GetComponent<Asteroid>();

            float directionZ;
            if (i == 0) {
                directionZ = asteroidInfo.direction.z + 45;
            }
            else {
                directionZ = asteroidInfo.direction.z - 45;
            }

            a.Create(new Vector3(0, 0, directionZ), asteroidInfo.coordinates, speed, asteroidInfo.size + 1);
            a.ActivateOnScene();
            _activeAsteroids.Add(asteroid);
        }
    }

    // Set's asteroid's fields to "factory" parameters
    private void SetupAsteroid(Asteroid asteroid) {
        float xAxis = 0;
        float yAxis = 0;
        if (Random.Range(0, 1) == 0) {
            xAxis = Random.Range(-8f, 8f);
            if (Random.Range(0, 1) == 0) {
                yAxis = -5;
            }
            else {
                yAxis = 5;
            }
        }
        else {
            yAxis = Random.Range(-4f, 4f);
            if (Random.Range(0, 1) == 0) {
                xAxis = -9.5f;
            }
            else {
                xAxis = 9.5f;
            }
        }

        asteroid.Create(new Vector3(0, 0, Random.Range(0, 360)),
            new Vector3(xAxis, yAxis, 0),
            new Vector2(Random.Range(15, 50), Random.Range(15, 50)));
    }

    private void SpawnAsteroids(int amount) {
        for (int i = 0; i < amount; ++i) {
            GameObject asteroid = _asteroidsPool.GetObjectFromPool();
            Asteroid a = asteroid.GetComponent<Asteroid>();
            a.ActivateOnScene();
            _activeAsteroids.Add(asteroid);
        }
    }
    private void GameOver() {
        LastGameScore.Score = _player.Score;
        SceneManager.LoadScene("GameOver");
    }
    private IEnumerator SpawnUFOCooldown() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(20, 40));
            _canSpawnUfo = true;
        }
    }

    #endregion
    #region Event methods

    private void Awake() {
        Instance = this;
        _player = Player.Instance;
        _ufo = UFO.Instance;
    }
    // Creating pool of asteroids
    private void Start() {
        List<GameObject> asteroids = new List<GameObject>();
        for (int i = 0; i < 100; ++i) {
            asteroids.Add(Instantiate(_asteroidPrefabs[Random.Range(0, _asteroidPrefabs.Count)]));
            asteroids[i].SetActive(false);
        }
        _asteroidsPool = new ObjectPool(asteroids);
        for (int i = 0; i < 100; ++i) {
            Asteroid a = _asteroidsPool.PeekObjectInPool(i).GetComponent<Asteroid>();
            a.DestroyAsteroidEvent += DestroyAndSpawnSmallerAsteroid;
            SetupAsteroid(a);
        }
        _activeAsteroids = new List<GameObject>();
        
        SpawnAsteroids(2);
        _prevSpawnedAsteroidsAmount = 2;

        StartCoroutine(SpawnUFOCooldown());

        Player.Instance.ShipsOutEvent += GameOver;
    }
    private void FixedUpdate() {
        if (_canSpawnUfo && !_ufo.gameObject.activeSelf) {
            float xAxis;
            float yAxis = Random.Range(-3.5f, 3.5f);
            int direction = Random.Range(0, 2);
            if (direction == 0) {
                xAxis = -8f;
            }
            else {
                xAxis = 8f;
            }

            _ufo.Create(new Vector3(xAxis, yAxis, 0), direction);
            _canSpawnUfo = false;
        }
        if (_activeAsteroids.Count == 0) {
            SpawnAsteroids(++_prevSpawnedAsteroidsAmount);
        }
    }

    #endregion
}
