using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private float _bottonPosition = -6.0f;

    private enum type
    {
        Triple_Shot,
        Speed,
        Shields
    }

    [SerializeField]
    private type powerUpType;

    [SerializeField]
    private AudioClip _clip;

    private UIManager _uiManager;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= _bottonPosition)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var playerComponent = other.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (playerComponent != null)
            {
                switch (powerUpType)
                {
                    case type.Triple_Shot:
                        playerComponent.ActivateTripleShot();
                        break;
                    case type.Speed:
                        playerComponent.ActivateSpeedPoweUp();
                        break;
                    case type.Shields:
                        playerComponent.ActivateShield();
                        _uiManager.ShowShieldText(true);
                        break;
                }
                
                Destroy(gameObject);
            }
        }
    }
}
