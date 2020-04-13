using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class AbilityCoolDown : MonoBehaviour
{
    public Image darkMask;
    public TMP_Text coolDownTextDisplay;

    [SerializeField] private Ability ability;
    [HideInInspector] public GameObject actor;
    private Image buttonImage;
    private float coolDownDuration, nextReadyTime, coolDownTimeLeft;
    private bool initialized;
    private void Start()
    {
        ServicesLocator.EventManager.Register<GameStarted>((AGPEvent e) =>
        {
            Initialize(ability);
        });
    }

    private void Initialize(Ability selectedAbility)
    {
        actor = GameObject.FindWithTag("Player");
        Debug.Log(actor.name);
        ability = selectedAbility;
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = ability.abilityIcon;
        buttonImage.color = ability.abilityColor;
        darkMask.sprite = ability.abilityIcon;
        coolDownDuration = ability.baseCoolDown;
        ability.Initialize(actor);
        AbilityReady();
        initialized = true;
    }

    private void Update()
    {
        if (initialized)
        {
            var coolDownComplete = Time.time > nextReadyTime;

            if (coolDownComplete)
            {
                AbilityReady();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ButtonTriggered();
                }
            }
            else
            {
                CoolDown();
            }
        }
    }
    
    private void AbilityReady()
    {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }

    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        var roundedCoolDownTime = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCoolDownTime.ToString();
        darkMask.fillAmount = coolDownTimeLeft / coolDownDuration;
    }

    private void ButtonTriggered()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;
        
        ability.TriggerAbility();
    }
}
