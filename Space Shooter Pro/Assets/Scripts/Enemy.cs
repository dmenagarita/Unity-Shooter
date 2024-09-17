using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private float _bottonPosition = -6.0f;

    [SerializeField]
    private float _topPosition = 8.0f;

    private float randomX = 0f;

    private Player _player;

    private Animator _animator;
    private BoxCollider2D _boxCollider;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    // Start is called before the first frame update
    void Start()
    {
        randomX = Random.Range(-9.0f, 9.0f);
        transform.position = new Vector3(randomX, _topPosition, 0);

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player was not found");
        }

        _animator = gameObject.GetComponent<Animator>();

        if(_animator == null)
        {
            Debug.LogError("enemy Animatior component was not found");
        }

        _boxCollider = gameObject.GetComponent<BoxCollider2D>();

        if(_boxCollider ==  null)
        {
            Debug.LogError("enemy Box Collider component was not found");
        }

        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source was not found in enemy");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            var enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            //Debug.Break();
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            foreach (var l in lasers)
            {
                l.AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        randomX = Random.Range(-9.0f, 9.0f);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= _bottonPosition)
        {
            transform.position = new Vector3(randomX, _topPosition, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Add damage functionality
            _player.Damage();

            _animator.SetTrigger("OnEnemyDeath");
            _boxCollider.enabled = false;
            _audioSource.Play();
            Destroy(gameObject, 2.8f);
        }
        else if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            _animator.SetTrigger("OnEnemyDeath");

            _player.AddScore(10);
            _boxCollider.enabled = false;
            _audioSource.Play();
            Destroy(gameObject, 2.8f);
        }
    }
}
