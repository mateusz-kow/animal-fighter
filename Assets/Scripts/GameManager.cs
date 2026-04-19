using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject endPanel;

    [Header("HUD Elements")]
    public TextMeshProUGUI hudInfoText;

    [Header("Pause & Sound Elements")]
    public Slider volumeSlider;
    public Toggle muteToggle;
    public Button resumeButton;

    [Header("End Screen Elements")]
    public TextMeshProUGUI endStatusText;

    [Header("Logic References")]
    public BoardGenerator generator;
    public AnimalSpawner spawner;
    public PlayerController player;
    public CameraMovement cam;

    [Header("Game Settings")]
    public int currentLevel = 1;
    public int maxLevels = 2;
    private int caughtCount = 0;
    private int targetCount;
    private float timer;
    
    private bool isPaused = false;
    private bool gameActive = false;

    void Awake() => Instance = this;

    void Start()
    {
        player.gameObject.SetActive(false);
        Time.timeScale = 0; 
        gameActive = false;
        
        ShowPanel(startPanel);

        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteToggle.onValueChanged.AddListener(ToggleMute);

        AudioListener.volume = volumeSlider.value;
    }

    public void StartGame()
    {
        player.gameObject.SetActive(true);
        gameActive = true;
        Time.timeScale = 1;
        ShowPanel(hudPanel);
        LoadLevel();
    }

    void LoadLevel()
    {
        caughtCount = 0;
        targetCount = 3 + (currentLevel * 2);
        timer = 40f - (currentLevel * 5f);

        // Generowanie planszy i norki
        var burrows = generator.GenerateLevel(currentLevel);
        float size = 10 + (currentLevel * 4);

        // Rozesłanie danych do systemów
        spawner.SetBurrows(burrows.ToArray());
        player.SetBounds(size);
        player.transform.position = new Vector3(size / 2f, size / 2f, 0);
        cam.SetBounds(size);
    }

    void Update()
    {
        if (!gameActive) return;

        // Klawisz pauzy
        if (Keyboard.current != null && (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame))
        {
            TogglePause();
        }

        if (!isPaused)
        {
            timer -= Time.deltaTime;
            UpdateHUD();

            if (timer <= 0) GameOver("Czas minął! Cel nieosiągnięty.");
        }
    }

    void UpdateHUD()
    {
        hudInfoText.text = $"POZIOM: {currentLevel} | CZAS: {timer:F1}s | ZŁAPANE: {caughtCount}/{targetCount}";
    }

    public void CatchAnimal()
    {
        caughtCount++;
        if (caughtCount >= targetCount)
        {
            if (currentLevel < maxLevels)
            {
                currentLevel++;
                LoadLevel();
            }
            else
            {
                GameOver("Gratulacje! Wszystkie poziomy ukończone!");
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        
        if (isPaused)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }

    public void SetVolume(float val)
    {
        if (muteToggle.isOn) AudioListener.volume = val;
    }

    public void ToggleMute(bool isOn)
    {
        AudioListener.pause = !isOn;
    }

    void GameOver(string message)
    {
        player.gameObject.SetActive(false);
        gameActive = false;
        Time.timeScale = 0;
        endStatusText.text = message;
        ShowPanel(endPanel);
    }

    public void RestartGame()
    {
        currentLevel = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ShowPanel(GameObject panelToShow)
    {
        startPanel.SetActive(panelToShow == startPanel);
        hudPanel.SetActive(panelToShow == hudPanel || panelToShow == pausePanel);
        pausePanel.SetActive(panelToShow == pausePanel);
        endPanel.SetActive(panelToShow == endPanel);
    }
}