using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIDisplay : MonoBehaviour {
    [SerializeField]
    public Image healthBar;
    [SerializeField]
    public Image experienceBar;
    public Text healthText;
    Color flashColor = new Color(1, 0, 0, 1);
    public Image flickerEffect;

    void Update()
    {
        flashColor.a = Mathf.Lerp(flashColor.a, 0, 0.1f);
        flickerEffect.color = flashColor;

    }
    private void Start()
    {
        flickerEffect.color = flashColor;
        flashColor.a = 0;
    }


    public void updateHealth(float curr, float max)
    {
        healthBar.fillAmount = (float)curr / max;
        healthText.text = curr + " / " + max;
    }
    public void flickerScreen() {
        flashColor.a = 1;
        flickerEffect.color = flashColor;
    }
    public void updateExperience(float curr, float max)
    {
        experienceBar.fillAmount = (float)curr / max;      
    }

}
