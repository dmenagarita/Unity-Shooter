using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _shieldText;
    [SerializeField]
    private Image _LivesImage;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private AudioClip _shieldActivatedSoundClip;

    private AudioSource _audioSource;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        _shieldText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("Game manager was not found");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Error finding component audio source on the player");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void ShowShieldText(bool activeValue)
    {
        _shieldText.gameObject.SetActive(activeValue);

        if(activeValue)
        {
            _audioSource.clip = _shieldActivatedSoundClip;
            _audioSource.Play();
        }
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImage.sprite = _livesSprite[currentLives];
    }

    public void DisplayGameOverMessage()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
