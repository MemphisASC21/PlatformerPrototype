using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public PlayerHealth playerHealth;

    // Update is called once per frame
    void Update()
    {
        fillImage.fillAmount = playerHealth.HealthPercent;
    }
}
