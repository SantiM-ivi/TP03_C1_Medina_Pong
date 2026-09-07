using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private PaddleMovement player1Movement;
    [SerializeField] private PaddleMovement player2Movement;

    [SerializeField] private Slider player1Slider;
    [SerializeField] private TMP_Text player1ValueText;

    [SerializeField] private Slider player2Slider;
    [SerializeField] private TMP_Text player2ValueText;

    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 15f;

    private void OnEnable()
    {
        player1Slider.minValue = minSpeed;
        player1Slider.maxValue = maxSpeed;
        player2Slider.minValue = minSpeed;
        player2Slider.maxValue = maxSpeed;

        player1Slider.value = player1Movement.MoveSpeed;
        player2Slider.value = player2Movement.MoveSpeed;

        UpdatePlayer1Text(player1Slider.value);
        UpdatePlayer2Text(player2Slider.value);
    }

    public void OnPlayer1SliderChanged(float value)
    {
        player1Movement.MoveSpeed = value;
        UpdatePlayer1Text(value);
    }

    public void OnPlayer2SliderChanged(float value)
    {
        player2Movement.MoveSpeed = value;
        UpdatePlayer2Text(value);
    }

    private void UpdatePlayer1Text(float value)
    {
        player1ValueText.text = value.ToString("F1");
    }

    private void UpdatePlayer2Text(float value)
    {
        player2ValueText.text = value.ToString("F1");
    }
}
