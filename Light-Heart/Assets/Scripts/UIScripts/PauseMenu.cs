using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private FPSController player;

    [Header("Optional: Fade")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed = 8f;

    public static bool IsPaused { get; private set; }

    private bool isFading;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) Resume();
            else Pause();
        }

        // Fade must use unscaledDeltaTime since Time.timeScale is 0 while paused
        if (canvasGroup != null && isFading)
        {
            float target = IsPaused ? 1f : 0f;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, fadeSpeed * Time.unscaledDeltaTime);

            if (Mathf.Approximately(canvasGroup.alpha, target))
            {
                isFading = false;
                if (!IsPaused) pauseMenuPanel.SetActive(false);
            }
        }
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (player != null) player.enabled = false;

        pauseMenuPanel.SetActive(true);
        isFading = canvasGroup != null;
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (player != null) player.enabled = true;

        if (canvasGroup != null)
        {
            isFading = true;
        }
        else
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    // Hook this up to a "Quit" button's OnClick
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Hook this up to a "Main Menu" button's OnClick
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);

    }
}
