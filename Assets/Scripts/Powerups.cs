using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Info and methods about powerups
/// </summary>
/// <author>Dan Singer</author>
public class Powerups : MonoBehaviour {

    public enum PowerupType
    {
        RapidFire,
        Invincibility
    }

    private PowerupType currentPowerupType;

    public float defaultPowerupDuration = 5;
    public Color defaultFlashColor;

    //Events
    public event Action<PowerupType> PowerupBegan;
    public event Action<PowerupType> PowerupEnded;


    /// <summary>
    /// Represents a generalized powerup, where actions occur when attached, updated, and detached.
    /// </summary>
    private class PowerupInfo
    {
        public Action Attached { get; private set; }
        public Action Update { get; private set; }
        public Action Detached { get; private set; }

        public object StoredData { get; set; }

        public float Duration { get; private set; }

        public PowerupInfo(float duration)
        {
            Duration = duration;
        }

        public void SetActions(Action attached, Action update, Action detached)
        {
            Attached = attached == null ? () => { } : attached;
            Update = update == null ? () => { } : update;
            Detached = detached == null ? () => { } : detached;
        }
    }

    private PowerupInfo[] powerupData;

    private PowerupInfo currentPowerup;
    private bool usingPowerup = false;
    private float timePowerupBegan = 0;

	// Use this for initialization
	void Start () {
        InitializePowerups();
	}

    /// <summary>
    /// Initialize each of the possible powerups, and add them to the dictionary.
    /// </summary>
    private void InitializePowerups()
    {
        PowerupInfo rapidFire = new PowerupInfo(defaultPowerupDuration);
        //Note that SetActions takes three delegate parameters, hence the odd structure below.
        rapidFire.SetActions(() =>
        {
            GameObject player = GameManager.Instance.PlayerInstance;
            Gun gun = player.GetComponent<Gun>();
            rapidFire.StoredData = gun.minDurationBetweenShots;
            gun.minDurationBetweenShots /= 2;
            gun.canHoldToFire = true;
            Flash flash = player.GetComponent<Flash>();
            flash.flashColor = defaultFlashColor;
            flash.enabled = true;
        }, null, 
        () =>
        {
            GameObject player = GameManager.Instance.PlayerInstance;
            if (!player) return;
            Gun gun = player.GetComponent<Gun>();
            gun.minDurationBetweenShots = (float)rapidFire.StoredData;
            gun.canHoldToFire = false;
            player.GetComponent<Flash>().enabled = false;
        });

        PowerupInfo invincibility = new PowerupInfo(defaultPowerupDuration);
        invincibility.SetActions(() =>
        {
            GameObject player = GameManager.Instance.PlayerInstance;

            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            invincibility.StoredData = ph.canTakeDamage;
            ph.canTakeDamage = false;

            Flash flash = player.GetComponent<Flash>();
            flash.flashColor = defaultFlashColor * .7f;
            flash.enabled = true;

        }, null,
        () =>
        {
            GameObject player = GameManager.Instance.PlayerInstance;
            if (!player) return;
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            ph.canTakeDamage = (bool)invincibility.StoredData;
            player.GetComponent<Flash>().enabled = false;
        });

        //IMPORTANT: Make sure that powerupInfo objects are populated in the SAME order as the PowerupType enumeration!
        powerupData = new PowerupInfo[]{ rapidFire, invincibility };
    }


    /// <summary>
    /// Deactivate the current powerup, and then activate the provided powerup.
    /// </summary>
    /// <param name="powerupType">Enum representing all available powerups</param>
    public void ActivatePowerup(PowerupType powerupType)
    {
        DeactivateCurrentPowerup();
        currentPowerup = powerupData[(int)powerupType];
        if (currentPowerup.Attached != null)
            currentPowerup.Attached();
        timePowerupBegan = Time.time;
        usingPowerup = true;
        currentPowerupType = powerupType;
        if (PowerupBegan != null)
        {
            PowerupBegan(powerupType);
        }
    }

    /// <summary>
    /// Deactivate the current powerup. Does nothing if no powerups activated.
    /// </summary>
    public void DeactivateCurrentPowerup()
    {
        usingPowerup = false;
        if (currentPowerup != null)
        {
            if (currentPowerup.Detached != null)
            {
                currentPowerup.Detached();
            }
            if (PowerupEnded != null)
            {
                PowerupEnded(currentPowerupType);
            }
        }
    }

    /// <summary>
    /// Manages powerup duration
    /// </summary>
    void Update () {

		if (usingPowerup)
        {
            if (currentPowerup.Update != null)
                currentPowerup.Update();

            if (Time.time > timePowerupBegan + currentPowerup.Duration)
            {
                usingPowerup = false;
                DeactivateCurrentPowerup();
            }
        }
	}
}
