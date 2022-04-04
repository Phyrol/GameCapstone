using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public static HealthDisplay Instance;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI healthTextShadow;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        healthText = transform.Find("PercentageText").GetComponent<TextMeshProUGUI>();
        healthTextShadow = transform.Find("TextShadow").GetComponent<TextMeshProUGUI>();
    }

    public void SetHealthDisplay( float percentage )
    {
        healthText.text = percentage.ToString() + "%";
        healthTextShadow.text = percentage.ToString() + "%";
    }
}
