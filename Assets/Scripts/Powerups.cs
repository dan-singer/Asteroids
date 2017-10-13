using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The "compartment" in the ship for powerups
/// </summary>
/// <author>Dan Singer</author>
public class Powerups : MonoBehaviour {

    public enum PowerupType
    {
        RapidFire
    }


    public float defaultPowerupDuration = 5;
    public Color defaultFlashColor;

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
    /// Initialize each of the possible powerupds, and add them to the dictionary
    /// </summary>
    private void InitializePowerups()
    {

        PowerupInfo rapidFire = new PowerupInfo(defaultPowerupDuration);
        rapidFire.SetActions(() =>
        {
            Gun gun = GetComponent<Gun>();
            rapidFire.StoredData = gun.minDurationBetweenShots;
            gun.minDurationBetweenShots /= 2;
            gun.canHoldToFire = true;
            Flash flash = GetComponent<Flash>();
            flash.flashColor = defaultFlashColor;
            flash.enabled = true;
        }, null, 
        () =>
        {
            Gun gun = GetComponent<Gun>();
            gun.minDurationBetweenShots = (float)rapidFire.StoredData;
            gun.canHoldToFire = false;
            GetComponent<Flash>().enabled = false;
        });

        //IMPORTANT: Make sure that powerupInfo objects are populated in the SAME order as the PowerupType enumeration!
        powerupData = new PowerupInfo[]{ rapidFire };
    }


    public void ActivatePowerup(PowerupType powerupType)
    {
        DeactivateCurrentPowerup();
        currentPowerup = powerupData[(int)powerupType];
        if (currentPowerup.Attached != null)
            currentPowerup.Attached();
        timePowerupBegan = Time.time;
        usingPowerup = true;
    }

    public void DeactivateCurrentPowerup()
    {
        usingPowerup = false;
        if (currentPowerup != null)
        {
            if (currentPowerup.Detached != null)
            {
                currentPowerup.Detached();
            }
        }
    }

    // Update is called once per frame
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
