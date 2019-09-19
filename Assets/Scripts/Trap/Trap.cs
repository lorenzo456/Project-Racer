using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Info")]
    public bool repeat;
    public bool hasParticles;
    public float activationDistance;
    public float cooldownTimer;
    public float disableTime;

    [SerializeField]private bool currentlyUsed;
    [SerializeField] protected float currentTime;
    [SerializeField]protected bool activated;
    [SerializeField] protected List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    GameObject ButtonSprite;
    public bool selected = false;


    protected virtual void Start()
    {
        ButtonSprite = transform.GetChild(0).gameObject;
        if (hasParticles) GetParticles();
        currentTime = cooldownTimer + 1;
        UnSelected();
    }

    protected void GetParticles()
    {
        //TODO: hardcoded
        Transform particleHolder = transform.GetChild(1).gameObject.transform;
        foreach (Transform p in particleHolder)
        {
            ParticleSystem temp = p.GetComponent<ParticleSystem>();
            if (temp != null)
            {
                particleSystems.Add(temp);
            }
        }
    }

    protected virtual void Update()
    {
        CheckDistanceToPlayers();


    }

    bool CheckIfPlayerIsParallel(Player p)
    {
        Vector3 forward = p.carController.transform.TransformDirection(Vector3.forward);
        Vector3 toOther = transform.position - p.carController.transform.position;

        //Debug.Log(Vector3.Dot(forward, toOther));

        if (Vector3.Dot(forward, toOther) > 0)
        {
            //Debug.Log("The player is looking at me!");
            return true;
        }

        return false;
    }
    void CheckDistanceToPlayers()
    {
        //Check one player
        foreach(Player p in GameManager.instance.players)
        {
            //Check if player is close to 
            if (Vector3.Distance(new Vector3(p.GetCarPosition().x,0, p.GetCarPosition().z), new Vector3(transform.position.x, 0,transform.position.z)) < activationDistance && !currentlyUsed && CheckIfPlayerIsParallel(p))
            {
                p.selectedTrap = this;
                Selected();
                return;
            }
            else if(p.selectedTrap == this)
            {
                p.selectedTrap = null;
                UnSelected();
            }
        }
    }

    public virtual void Selected()
    {
        ButtonSprite.SetActive(true);
    }

    public virtual void UnSelected()
    {
        ButtonSprite.SetActive(false);
    }

    public virtual void Activate()
    {
        if (currentlyUsed || currentTime < cooldownTimer)
            return;

        if (activated && repeat)
        {
            DeActivate();
            return;
        }
        activated = true;
        currentlyUsed = true;
        EnableParticles(true);
        StartCoroutine(StartTimer());
    }

    public virtual void DeActivate()
    {
        activated = false;
        currentTime = 0;
        currentlyUsed = false;
        currentTime = cooldownTimer + 1;
        EnableParticles(false);
    }

    IEnumerator StartTimer()
    {
        currentTime = 0;
        while (currentTime < cooldownTimer)
        {
            yield return new WaitForSeconds(1);
            currentTime++;
        }
        currentTime = cooldownTimer + 1;
        if (!repeat)
        {
            DeActivate();
        }
    }

    protected void EnableParticles(bool enable)
    {
        if (!hasParticles)
            return;

        foreach (ParticleSystem p in particleSystems)
        {
            if (enable)
            {
                p.Play();
            }
            else
            {
                p.Stop();
            }
        }
    }
}
