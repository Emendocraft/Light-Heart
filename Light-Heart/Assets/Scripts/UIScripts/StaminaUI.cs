using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private FPSController player;
    [SerializeField] private Image staminaSliderBar;

    [Header("Optional: ausblenden wenn voll")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool hideWhenFull = true;
    [SerializeField] private float fadeSpeed = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        staminaSliderBar.fillAmount = player.staminaPercent;

        if (hideWhenFull && canvasGroup != null)
        {
            float target = player.staminaPercent >= 0.999f ? 0f : 1f;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, target, fadeSpeed * Time.deltaTime);
        }
    }
}
