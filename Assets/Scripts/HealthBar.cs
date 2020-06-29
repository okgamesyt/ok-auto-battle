using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform bar;
    public Vector3 offset;

    private float maxHealth;
    Transform target;

    public void Setup(Transform target, float maxHealth)
    {
        this.maxHealth = maxHealth;
        UpdateBar(maxHealth);
        this.target = target;
    }

    public void UpdateBar(float newValue)
    {
        float newScale = newValue / maxHealth;
        Vector3 scale = bar.transform.localScale;
        scale.x = newScale;
        bar.transform.localScale = scale;
    }

    private void Update()
    {
        if(target != null)
            this.transform.position = target.position + offset;
    }
}
