using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TMP_Text orangeScoreText;
    public TMP_Text blueScoreText;

    void Start()
    {
        UpdateScores(0, 0);
    }


    public void UpdateScores(int orangeScore, int blueScore)
    {
        orangeScoreText.text = $"Orange: {orangeScore}";
        blueScoreText.text = $"Blue: {blueScore}";
    }
}
