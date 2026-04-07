using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField] private Image ProcessBar;

    public void UpdateProcessBar(float weightTreshold, float currentweight)
    {
        ProcessBar.fillAmount = currentweight / weightTreshold;
    }
}
