using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIDisplay : MonoBehaviour {
    [SerializeField]
    public Image healthBar;

    public void updateHealth(float curr, float max)
    {
        healthBar.fillAmount = (float)curr / max;
    }

}
