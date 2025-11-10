using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StaminaManager : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;

    [Header("Regeneration")]
    [SerializeField] private float regenRate = 10f; // Stamina per second
    [SerializeField] private float regenDelay = 2f; // Delay before regen starts

    [Header("Depletion")]
    [SerializeField] public float depletionRate = 20f; // Stamina per second when sprinting

    private float lastUsedTime;
    private bool isRegenerating;
    public ProgressBar Pb;
    // Event for UI updates
    public event Action<float, float> OnStaminaChanged; //current, max
    public event Action OnStaminaDepleted;
    public event Action OnStaminarestored;

    private void Awake()
    {
        Pb = FindObjectOfType<ProgressBar>();
        currentStamina = maxStamina;
        UpdateUi();
    }

    private void UpdateUi()
    {
        if (Pb != null)
        {
            Pb.BarValue = GetStaminaPercentage() * 100f;
        }
    }
    private void Update()
    {
        HandleRegeneration();
    }

    private void HandleRegeneration()
    {
        if (currentStamina < maxStamina && Time.time - lastUsedTime >= regenDelay)
        {
            if (!isRegenerating)
            {
                isRegenerating = true;
            }

            currentStamina += regenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            UpdateUi();

            if (currentStamina >= maxStamina)
            {
                OnStaminarestored?.Invoke();
            }

        }
    }

    public bool UseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            lastUsedTime = Time.time;
            isRegenerating = false;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            UpdateUi();

            if (currentStamina <=0)
            {
                currentStamina = 0;
                OnStaminaDepleted?.Invoke();
            }

            return true;
        }

        return false;
    }

    public bool CanUseStamina(float amount)
    {
        return currentStamina >= amount;
    }

    public void UseStaminaContinuous(float deltaTime)
    {
        UseStamina(depletionRate * deltaTime);
    }

    public float GetStaminaPercentage()
    {
        return currentStamina / maxStamina;
    }

    public bool IsDepleted()
    {
        return currentStamina <= 0;
    }

    public void ResetStamina()
    {
        currentStamina = maxStamina;
        OnStaminaChanged.Invoke(currentStamina, maxStamina);
        UpdateUi();
    }

    internal bool CanUseStamina(object p)
    {
        throw new NotImplementedException();
    }
}
