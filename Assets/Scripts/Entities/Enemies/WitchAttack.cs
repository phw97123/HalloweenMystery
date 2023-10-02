using UnityEngine;
using Utils;

public class WitchAttack : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;

    private int _circleShotsFired = 0;
    private const int _maxCircleShots = 10;

    [SerializeField] private float circleShotInterval = 2.0f;
    [SerializeField] private float spinShotInterval = 20.0f;
    [SerializeField] private float restDuration = 5.0f;

    private float _timeSinceLastCircleShot = 0f;
    private float _timeSinceLastSpinShot = 0f;
    private float _restTimer = 0f;
    private float _spawnTimer;

    [SerializeField] private float initialTurnSpeed = 1.0f;
    [SerializeField] private float maxTurnSpeed = 50.0f;
    private float _currentTurnSpeed;

    [SerializeField] private float SpawnInterval = 0.2f;

    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip restSound;

    private enum ShotState
    {
        CircleShot,
        SpinShot,
        Rest
    }

    private ShotState currentState = ShotState.CircleShot;


    private void Start()
    {
        _currentTurnSpeed = initialTurnSpeed;
    }

    private void Update()
    {
        switch (currentState)
        {
            case ShotState.CircleShot:
                _timeSinceLastCircleShot += Time.deltaTime;

                if (_timeSinceLastCircleShot >= circleShotInterval && _circleShotsFired < _maxCircleShots)
                {
                    ShotCircle();
                    _timeSinceLastCircleShot = 0f;
                }

                if (_circleShotsFired >= _maxCircleShots)
                {
                    currentState = ShotState.Rest;

                    if (restSound != null)
                    {
                        SoundManager.PlayClip(restSound);
                    }
                }
                break;

            case ShotState.SpinShot:
                _timeSinceLastSpinShot += Time.deltaTime;

                if (_timeSinceLastSpinShot >= spinShotInterval)
                {
                    currentState = ShotState.CircleShot;
                    _timeSinceLastSpinShot = 0f;
                    _circleShotsFired = 0;

                    _currentTurnSpeed = Mathf.Min(_currentTurnSpeed + 2.0f, maxTurnSpeed);    
                }
                else
                {
                    transform.Rotate(Vector3.forward * (_currentTurnSpeed * 100 * Time.deltaTime));

                    _spawnTimer += Time.deltaTime;
                    if (_spawnTimer >= SpawnInterval)
                    {
                        GameObject temp = Instantiate(Bullet);
                        if (attackSound != null)
                        {
                            SoundManager.PlayClip(attackSound);
                        }

                        Destroy(temp, 2f);

                        temp.transform.position = transform.position;
                        temp.transform.rotation = transform.rotation;

                        _spawnTimer = 0f;
                    }
                }
                break;

            case ShotState.Rest:
                _restTimer += Time.deltaTime;
                
                if (_restTimer >= restDuration)
                {
                    currentState = ShotState.SpinShot;
                    _restTimer = 0f;
                }
                break;
        }
    }

    private void ShotCircle()
    {
        for (int i = 0; i < 360; i += 13)
        {
            GameObject temp = Instantiate(Bullet);
            Destroy(temp, 2f);
            temp.transform.position = Vector2.zero;
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }

        if (attackSound != null)
        {
            SoundManager.PlayClip(attackSound);
        }

        _circleShotsFired++;
    }
}
