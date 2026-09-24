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
    [SerializeField] private Button[] player1ColorButtons;

    [Header("Player 2")]
    [SerializeField] private PaddleMovement player2Movement;
    [SerializeField] private PaddleAppearance player2Appearance;
    [SerializeField] private Slider player2SpeedSlider;
    [SerializeField] private TMP_Text player2SpeedValueText;
    [SerializeField] private Slider player2HeightSlider;
    [SerializeField] private TMP_Text player2HeightValueText;
    [SerializeField] private Button[] player2ColorButtons;

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

    // Evita que asignar minValue/maxValue en SetupSlider dispare onValueChanged
    // (y por lo tanto los handlers OnXChanged) durante la inicializacion del panel.
    private bool isInitializing;

    private void OnEnable()
    {
        if (!ValidateReferences())
        {
            // Si falta algo, no seguimos: mejor un panel a medio configurar
            // con un error claro en la Console que uno silenciosamente roto
            // (los sliders de Height quedarian con los valores crudos de
            // Unity: min 0, max 1, y podrian dejar la paleta invisible).
            return;
        }

        isInitializing = true;

        // Height primero: es el que mas rompe visualmente si algo falla,
        // asi queda configurado antes que cualquier otra cosa pueda fallar.
        SetupSlider(player1HeightSlider, player1HeightValueText, minHeight, maxHeight, player1Appearance.Height, "F0");
        SetupSlider(player2HeightSlider, player2HeightValueText, minHeight, maxHeight, player2Appearance.Height, "F0");

        // Altura en pasos enteros: 1, 2, 3. wholeNumbers hace que Unity redondee
        // el valor del slider al entero mas cercano dentro del rango [minHeight, maxHeight].
        player1HeightSlider.wholeNumbers = true;
        player2HeightSlider.wholeNumbers = true;

        SetupSlider(player1SpeedSlider, player1SpeedValueText, minSpeed, maxSpeed, player1Movement.MoveSpeed);
        SetupSlider(player2SpeedSlider, player2SpeedValueText, minSpeed, maxSpeed, player2Movement.MoveSpeed);

        SetupColorButtons(player1ColorButtons, player1Appearance);
        SetupColorButtons(player2ColorButtons, player2Appearance);

        isInitializing = false;
    }

    /// <summary>
    /// Chequea que todas las referencias esten asignadas antes de tocar los
    /// sliders. Si falta algo, loguea exactamente que campo es y corta.
    /// </summary>
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

        return isValid;
    }

    private void SetupSlider(Slider slider, TMP_Text valueText, float min, float max, float currentValue, string format = "F1")
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.SetValueWithoutNotify(currentValue);
        valueText.text = currentValue.ToString(format);
    }

    private void SetupColorButtons(Button[] buttons, PaddleAppearance appearance)
    {
        for (int i = 0; i < buttons.Length && i < paddleColors.Length; i++)
        {
            Color color = paddleColors[i];

            Image swatch = buttons[i].GetComponent<Image>();
            if (swatch != null)
            {
                swatch.color = color;
            }

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => appearance.PaddleColor = color);
        }
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