using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [Header("References")]
    public Player player;

    [Header("Animations")]
    public Animator anim;

    [Header("GUI")]
    public Text speedText;
    public Image boostFill;

    [Header("PowerUps")]
    public float maxDriftPoints = 1;
    public float driftPoints = 0;
    public bool boosting;

    [Header("Transforms")]
    public Transform carModel;
    public Transform carNormal;
    public Rigidbody carSphere;
    public Vector3 offsetFromSphere;

    [Header("Parameters")]
    [SerializeField]private bool disabled;
    [SerializeField] private bool immune;
    private bool currentlyDisabled;
    public float gravity = 10f;
    //Speed
    float speed, currentSpeed, tempSpeed;
    public float acceleration = 30f;
    public float topSpeed = 100;
    public float boostSpeed = 200;
    public float boostDecreaser = 25f;
    public float immunityTime = 3;
    public float offBound = -5;

    //rotation
    float rotate, currentRotate;
    public float steering = 40f;
    public float minSpeedToRotate = .8f;
    //brakes
    public float deacceleration = 20;
    public float brakePower = 4;
    float currentBreakPower;
    //drift
    public bool drifting;
    int driftDirection;
    float driftPower;

    [Header("MeshRenderers")]
    public MeshRenderer brakeLights;
    public Transform frontWheelL;
    public Transform frontWheelR;
    public Transform backWheelL;
    public Transform backWheelR;

    [Header("ParticleSystems")]
    public ParticleSystem disabledParticle;
    public ParticleSystem[] sparks;
    bool sparksPlaying;
    public ParticleSystem[] boosts;
    bool boostPlaying;
    bool boostReset;
    bool carResetTimePassed = true;
    IEnumerator carResetTimer()
    {
        carResetTimePassed = false;
        yield return new WaitForSeconds(3);
        carResetTimePassed = true;
    }
    void Update()
    {
        //Follow Collider
        transform.position = carSphere.transform.position - offsetFromSphere;


        //Check Reset
        if (player.backButton == ButtonEnum.ButtonState.ButtonPressed && carResetTimePassed || transform.position.y < offBound)
        {
            player.ResetCarPosition();
            carResetTimer();
        }

        DrivingCheck();
        SteeringInputCheck();
        DriftCheck();
        BoostCheck();

        frontWheelR.localEulerAngles = new Vector3(frontWheelR.localEulerAngles.x + carSphere.velocity.magnitude * 10, (player.horizontalAxis * 15), 0);
        frontWheelL.localEulerAngles = new Vector3(frontWheelL.localEulerAngles.x + carSphere.velocity.magnitude * 10, (player.horizontalAxis * 15), 0);

        backWheelL.localEulerAngles += new Vector3( carSphere.velocity.magnitude * 10, 0, 0);
        backWheelR.localEulerAngles += new Vector3(carSphere.velocity.magnitude * 10, 0, 0);

        if (!disabled)
        {
            if (boosting)
            {
                currentSpeed = Mathf.SmoothStep(currentSpeed, boostSpeed, Time.deltaTime * 12f);
            }
            else
            {
                currentSpeed = Mathf.SmoothStep(currentSpeed, tempSpeed, Time.deltaTime * 12f); //speed = 0f;
            }
        }
        else
        {
            currentSpeed = 0;
            tempSpeed = 0;
        }

        if (carSphere.velocity.magnitude > minSpeedToRotate)
        {
            currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        }
        else
        {
            currentRotate = 0;
        }

        rotate = 0f;

        UpdateGUI();
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    private void FixedUpdate()
    {
        //Gravity
        carSphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Forward Acceleration
        carSphere.AddForce(-carModel.transform.right * currentSpeed, ForceMode.Acceleration);

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);
        
    }

    public void DrivingCheck()
    {
        //Driving
        if (player.leftTrigger > 0)
        {
            brakeLights.enabled = true;
            Brake();
        }//Accelerate
        else if (player.rightTrigger > 0)
        {
            brakeLights.enabled = false;
            currentBreakPower = brakePower;
            if (tempSpeed < topSpeed)
            {
                tempSpeed += acceleration * Time.deltaTime;
            }
        }
        else
        {
            if (player.leftTrigger == 0)
            {
                brakeLights.enabled = false;
            }

            currentBreakPower = brakePower;
            if (tempSpeed > 0)
            {
                tempSpeed -= (Time.deltaTime * deacceleration);
            }
        }

    }

    public void Brake()
    {
        if (tempSpeed > 0 && currentBreakPower > 0)
        {
            // if(currentBreakPower < 0) currentBreakPower = 0; 
            tempSpeed -= Time.deltaTime * (currentBreakPower += brakePower * Time.deltaTime);
        }
        else
        {
            tempSpeed = 0;
            currentBreakPower = 0;
        }
    }

    public void SteeringInputCheck()
    {
        //Steer
        if (player.horizontalAxis != 0)
        {
            int dir;
            if (player.horizontalAxis > 0)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            //int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs(player.horizontalAxis);
            Steer(dir, amount);
        }
    }

    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    public void DriftCheck()
    {
        //Drift
        if (player.yButton == ButtonEnum.ButtonState.ButtonIdle && drifting)
        {
            drifting = false;
            driftPower = 0;
        }

        if (player.yButton == ButtonEnum.ButtonState.ButtonPressed && !drifting && player.horizontalAxis != 0)
        {
            drifting = true;
            driftDirection = player.horizontalAxis > 0 ? 1 : -1;
        }

        if (drifting)
        {
            AddDriftPoints();
            brakeLights.enabled = true;
            if (!sparksPlaying)
            {
                foreach (ParticleSystem i in sparks)
                {
                    if (!i.isPlaying)
                    {
                        i.Play();
                    }
                }
                sparksPlaying = true;
            }


            float tempControl;
            if (driftDirection == 1)
            {
                tempControl = ExtensionMethods.Remap(player.horizontalAxis, -1, 1, -1, 2);
                anim.SetBool("DriftR", true);
            }
            else
            {
                anim.SetBool("DriftL", true);
                tempControl = ExtensionMethods.Remap(player.horizontalAxis, -1, 1, 2, -1);
            }

            float tempPowerControl;
            if (driftDirection == 1)
            {
                tempPowerControl = ExtensionMethods.Remap(player.horizontalAxis, -1, 1, .2f, 1);
            }
            else
            {
                tempPowerControl = ExtensionMethods.Remap(player.horizontalAxis, -1, 1, 1, .2f);
            }
            Brake();
            Steer(driftDirection, tempControl);
            driftPower += tempPowerControl;
        }
        else
        {
            if (player.leftTrigger == 0) brakeLights.enabled = false;
            anim.SetBool("DriftR", false);
            anim.SetBool("DriftL", false);

            if (sparksPlaying)
            {
                foreach (ParticleSystem i in sparks)
                {
                    if (i.isPlaying)
                    {
                        i.Stop();
                    }
                }
                sparksPlaying = false;
            }
        }
    }

    public void AddDriftPoints()
    {
        if(driftPoints < maxDriftPoints)
        {
            driftPoints += 1f;
        }
        else
        {
            driftPoints = maxDriftPoints;
        }
    }

    public void UpdateGUI()
    {
        if (speedText == null || boostFill == null)
            return;

        speedText.text = "Speed: " + Mathf.Round(currentSpeed) + " KM/H";
        boostFill.fillAmount = driftPoints / 100;
    }

    public void BoostCheck()
    {
        if (boosting)
        {
            if (player.aButton == ButtonEnum.ButtonState.ButtonUp || player.aButton == ButtonEnum.ButtonState.ButtonIdle || driftPoints <= 0 || boostReset)
            {
                boosting = false;
                if (boostPlaying)
                {
                    foreach (ParticleSystem i in boosts)
                    {
                        if (i.isPlaying)
                        {
                            i.Stop();
                        }
                    }
                    boostPlaying = false;
                }
            }
            driftPoints -= boostDecreaser * Time.deltaTime;
        }
        else
        {
            if (player.aButton == ButtonEnum.ButtonState.ButtonPressed)
            {
                if (driftPoints > 0)
                {
                    boosting = true;
                    if (!boostPlaying)
                    {
                        foreach (ParticleSystem i in boosts)
                        {
                            if (!i.isPlaying)
                            {
                                i.Play();
                            }
                        }
                        boostPlaying = true;
                    }
                }
                else
                {
                    driftPoints = 0;
                }
            }
        }
        boostReset = false;
    }

    public void ResetCar()
    {
        currentSpeed = 0;
        tempSpeed = 0;
        carSphere.velocity = Vector3.zero;
        boostReset = true;
    }

    public bool GetDisabledStatus()
    {
        return disabled;
    }

    public void SetDisabledStatus(bool status = false, float timeDisabled = 0)
    {
        if(status && !currentlyDisabled && !immune)
        {
            StartCoroutine(DisabledCounter(timeDisabled));
        }
    }

    IEnumerator DisabledCounter(float timeDisabled)
    {
        disabled = true;
        currentlyDisabled = true;
        disabledParticle.Play();
        yield return new WaitForSeconds(timeDisabled);
        disabledParticle.Stop();
        disabled = false;
        currentlyDisabled = false;
        StartCoroutine(EnableImmunity());
    }

    IEnumerator EnableImmunity()
    {
        immune = true;
        anim.SetBool("Imune", true);
        yield return new WaitForSeconds(immunityTime);
        anim.SetBool("Imune", false);
        immune = false;
    }

}
