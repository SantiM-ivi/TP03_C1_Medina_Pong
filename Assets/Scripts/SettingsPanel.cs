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
    [SerializeField] private float minHeight = 0.5f;
    [SerializeField] private float maxHeight = 2f;

    [Header("Color Palette")]
    [SerializeField]
    private Color[] paddleColors =
    {
        Color.white, Color.red, Color.cyan, Color.yellow, Color.green, Color.magenta
    };

    private void OnEnable()
    {
        SetupSlider(player1SpeedSlider, player1SpeedValueText, minSpeed, maxSpeed, player1Movement.MoveSpeed);
        SetupSlider(player2SpeedSlider, player2SpeedValueText, minSpeed, maxSpeed, player2Movement.MoveSpeed);

        SetupSlider(player1HeightSlider, player1HeightValueText, minHeight, maxHeight, player1Appearance.Height);
        SetupSlider(player2HeightSlider, player2HeightValueText, minHeight, maxHeight, player2Appearance.Height);

        SetupColorButtons(player1ColorButtons, player1Appearance);
        SetupColorButtons(player2ColorButtons, player2Appearance);
    }

    private void SetupSlider(Slider slider, TMP_Text valueText, float min, float max, float currentValue)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.SetValueWithoutNotify(currentValue);
        valueText.text = currentValue.ToString("F1");
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
        player1Movement.MoveSpeed = value;
        player1SpeedValueText.text = value.ToString("F1");
    }

    public void OnPlayer2SpeedChanged(float value)
    {
        player2Movement.MoveSpeed = value;
        player2SpeedValueText.text = value.ToString("F1");
    }

    public void OnPlayer1HeightChanged(float value)
    {
        player1Appearance.Height = value;
        player1HeightValueText.text = value.ToString("F1");
    }

    public void OnPlayer2HeightChanged(float value)
    {
        player2Appearance.Height = value;
        player2HeightValueText.text = value.ToString("F1");
    }
}
