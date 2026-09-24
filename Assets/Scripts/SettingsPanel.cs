using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    [Header("Player 1")]
    [SerializeField] private PaddleMovement player1Movement;
    [SerializeField] private PaddleAppearance player1Appearance;
    [SerializeField] private Slider player1SpeedSlider;
    [SerializeField] private TMP_Text player1SpeedValueText;
    [SerializeField] private Slider player1HeightSlider;
    [SerializeField] private TMP_Text player1HeightValueText;

    [Header("Player 2")]
    [SerializeField] private PaddleMovement player2Movement;
    [SerializeField] private PaddleAppearance player2Appearance;
    [SerializeField] private Slider player2SpeedSlider;
    [SerializeField] private TMP_Text player2SpeedValueText;
    [SerializeField] private Slider player2HeightSlider;
    [SerializeField] private TMP_Text player2HeightValueText;

    [Header("Color (aplica a ambos jugadores)")]
    [Tooltip("Un solo slider que recorre la paleta y cambia el color de los dos paddles a la vez.")]
    [SerializeField] private Slider colorSlider;
    [SerializeField] private TMP_Text colorValueText;
    [SerializeField] private Image colorPreviewP1;
    [SerializeField] private Image colorPreviewP2;

    [Header("Ranges")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float minHeight = 1f;
    [SerializeField] private float maxHeight = 3f;

    [Header("Color Palette")]
    [SerializeField]
    private Color[] paddleColors =
    {
        Color.white, Color.red, Color.cyan, Color.yellow, Color.green, Color.magenta
    };


    private bool isInitializing;

    private void OnEnable()
    {
        if (!ValidateReferences())
        {

            return;
        }

        isInitializing = true;


        SetupSlider(player1HeightSlider, player1HeightValueText, minHeight, maxHeight, player1Appearance.Height, "F0");
        SetupSlider(player2HeightSlider, player2HeightValueText, minHeight, maxHeight, player2Appearance.Height, "F0");


        player1HeightSlider.wholeNumbers = true;
        player2HeightSlider.wholeNumbers = true;

        SetupSlider(player1SpeedSlider, player1SpeedValueText, minSpeed, maxSpeed, player1Movement.MoveSpeed);
        SetupSlider(player2SpeedSlider, player2SpeedValueText, minSpeed, maxSpeed, player2Movement.MoveSpeed);

        SetupColorSlider();

        isInitializing = false;
    }


    private bool ValidateReferences()
    {
        bool isValid = true;

        if (player1Movement == null) { Debug.LogError("SettingsPanel: falta asignar Player 1 Movement.", this); isValid = false; }
        if (player1Appearance == null) { Debug.LogError("SettingsPanel: falta asignar Player 1 Appearance.", this); isValid = false; }
        if (player1SpeedSlider == null) { Debug.LogError("SettingsPanel: falta asignar Player 1 Speed Slider.", this); isValid = false; }
        if (player1HeightSlider == null) { Debug.LogError("SettingsPanel: falta asignar Player 1 Height Slider.", this); isValid = false; }
        if (player2Movement == null) { Debug.LogError("SettingsPanel: falta asignar Player 2 Movement.", this); isValid = false; }
        if (player2Appearance == null) { Debug.LogError("SettingsPanel: falta asignar Player 2 Appearance.", this); isValid = false; }
        if (player2SpeedSlider == null) { Debug.LogError("SettingsPanel: falta asignar Player 2 Speed Slider.", this); isValid = false; }
        if (player2HeightSlider == null) { Debug.LogError("SettingsPanel: falta asignar Player 2 Height Slider.", this); isValid = false; }
        if (colorSlider == null) { Debug.LogError("SettingsPanel: falta asignar Color Slider.", this); isValid = false; }
        if (paddleColors == null || paddleColors.Length == 0) { Debug.LogError("SettingsPanel: Paddle Colors esta vacio.", this); isValid = false; }

        return isValid;
    }

    private void SetupSlider(Slider slider, TMP_Text valueText, float min, float max, float currentValue, string format = "F1")
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.SetValueWithoutNotify(currentValue);
        valueText.text = currentValue.ToString(format);
    }

    private void SetupColorSlider()
    {
        colorSlider.wholeNumbers = true;
        colorSlider.minValue = 0;
        colorSlider.maxValue = paddleColors.Length - 1;

        // Índice actual basado en el color actual de Player 1 (si no matchea ninguno, arranca en 0).
        int currentIndex = 0;
        Color currentColor = player1Appearance.PaddleColor;
        for (int i = 0; i < paddleColors.Length; i++)
        {
            if (paddleColors[i] == currentColor)
            {
                currentIndex = i;
                break;
            }
        }

        colorSlider.SetValueWithoutNotify(currentIndex);
        ApplyColorIndex(currentIndex);
    }

    public void OnColorSliderChanged(float value)
    {
        if (isInitializing) return;

        ApplyColorIndex(Mathf.RoundToInt(value));
    }

    private void ApplyColorIndex(int index)
    {
        index = Mathf.Clamp(index, 0, paddleColors.Length - 1);
        Color color = paddleColors[index];

        Debug.Log($"[SettingsPanel] Color slider -> índice {index} ({color}). Aplicando a Player 1 y Player 2.");

        player1Appearance.PaddleColor = color;
        player2Appearance.PaddleColor = color;

        if (colorPreviewP1 != null) colorPreviewP1.color = color;
        if (colorPreviewP2 != null) colorPreviewP2.color = color;

        if (colorValueText != null) colorValueText.text = $"{index + 1}/{paddleColors.Length}";
    }

    public void OnPlayer1SpeedChanged(float value)
    {
        if (isInitializing) return;

        player1Movement.MoveSpeed = value;
        player1SpeedValueText.text = value.ToString("F1");
    }

    public void OnPlayer2SpeedChanged(float value)
    {
        if (isInitializing) return;

        player2Movement.MoveSpeed = value;
        player2SpeedValueText.text = value.ToString("F1");
    }

    public void OnPlayer1HeightChanged(float value)
    {
        if (isInitializing) return;

        player1Appearance.Height = value;
        player1HeightValueText.text = value.ToString("F0");
    }

    public void OnPlayer2HeightChanged(float value)
    {
        if (isInitializing) return;

        player2Appearance.Height = value;
        player2HeightValueText.text = value.ToString("F0");
    }
}