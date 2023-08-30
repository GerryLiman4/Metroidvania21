using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> healthIcons = new List<GameObject>();

    private int currentHealth = 0;
    private void Awake()
    {
        foreach (GameObject healthIcon in healthIcons)
        {
            healthIcon.SetActive(false);
        }
    }

    public void SetHealth(int health)
    {
        currentHealth = health;

        int healthCount = 0;
        foreach (GameObject healthIcon in healthIcons)
        {
            if (healthCount < currentHealth)
            {
                healthIcon.SetActive(true);
                healthCount++;
                continue;
            }
            healthIcon.SetActive(false);
        }
    }

    public void ReduceHealth(int health)
    {
        for (int i = 0; i < health; i++)
        {
            foreach (GameObject healthIcon in healthIcons)
            {
                if (healthIcon.activeInHierarchy)
                {
                    healthIcon.SetActive(false);
                    break;
                }
            }
        }
        currentHealth = Mathf.Clamp(currentHealth - health, 0, healthIcons.Count);
    }
    public void AddHealth(int health)
    {
        for (int i = 0; i < health; i++)
        {
            foreach (GameObject healthIcon in healthIcons)
            {
                if (!healthIcon.activeInHierarchy)
                {
                    healthIcon.SetActive(true);
                    break;
                }
            }
        }
        currentHealth = Mathf.Clamp(currentHealth - health, 0, healthIcons.Count);
    }
}
