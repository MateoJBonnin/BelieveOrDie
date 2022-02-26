using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TalkSystem;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum Roles
{
    Villager,
    Priest,
    Trader,
    Atheist
}

public class Villager : MonoBehaviour
{
    public Roles rol;
    public NavMeshAgent agent;
    public Vector3 startPos;
    public FaithController faithController;
    public Rigidbody rigidBody;
    public Talk talk;
    
    public Action<Villager> OnDie;
    private ActionTasks baseTasks;

    Stack<ActionTasks> toDoTasks = new Stack<ActionTasks>();

    public List<Collider> colliders;
    public bool IsAtheist => rol == Roles.Atheist;

    public bool isDead;

    public void Setup(ActionTasks bt)
    {
        startPos = transform.position;
        baseTasks = bt;

        toDoTasks.Push(baseTasks);
        baseTasks.StartActivities();
        baseTasks.OnEndAllTasks += RestartBaseTasks;
    }

    public void Die()
    {
        while (toDoTasks.Any())
        {
            ActionTasks tasks = toDoTasks.Pop();
            tasks.Stop();
        }

        if (faithController)
        {
            faithController.SpreadActive(false);
            rigidBody.constraints = RigidbodyConstraints.None;
            talk.Deactivate();
        }

        colliders.ForEach(c=> c.enabled = false);
        OnDie?.Invoke(this);
        Destroy(agent);
        isDead = true;
        
        Sequence mySequence = DOTween.Sequence();
        mySequence.PrependInterval(2);
        mySequence.Append(transform.DOScale(Vector3.zero, 1));
        mySequence.OnComplete(() => Destroy(gameObject));
    }

    public void RestartBaseTasks()
    {
        if (isDead) return;

        baseTasks.StartActivities();
    }

    public void OverrideTasks(ActionTasks newTasks)
    {
        if (isDead) return;

        toDoTasks.Peek().Stop();
        toDoTasks.Push(newTasks);
        toDoTasks.Peek().StartActivities();
        newTasks.OnEndAllTasks += ResumeBase;
    }

    public void ResumeBase()
    {
        if (isDead) return;

        toDoTasks.Pop().OnEndAllTasks -= ResumeBase;
        toDoTasks.Peek().Resume();
    }

    private void Update()
    {
        if (this.toDoTasks != null &&
            this.toDoTasks.Count > 0)
        {
            toDoTasks.Peek().Update();
        }
    }
}

public class ActionTasks
{
    public System.Action OnEndAllTasks;

    private List<Actions> actions = new List<Actions>();

    private int currentAction;

    public void AddAction(Actions action)
    {
        actions.Add(action);
    }

    public void Stop()
    {
        actions[currentAction].Stop();
    }

    public void Resume()
    {
        actions[currentAction].Execute();
    }

    public void StartActivities(Actions action)
    {
        action.OnComplete += OnActionComplete;
        action.Execute();
    }

    public void StartActivities()
    {
        currentAction = 0;
        actions[currentAction].OnComplete += OnActionComplete;
        actions[currentAction].Execute();
    }

    public void Update()
    {
        actions[currentAction].Update();
    }

    private void OnActionComplete()
    {
        actions[currentAction].OnComplete -= OnActionComplete;

        currentAction++;
        if (currentAction >= actions.Count)
        {
            currentAction = 0;
            OnEndAllTasks?.Invoke();
        }
        else
        {
            StartActivities(actions[currentAction]);
        }
    }
}

public abstract class Actions
{
    public Villager vill;
    public System.Action OnComplete;
    public abstract void Update();
    public abstract void Stop();
    public abstract void Execute();
}

public class GoToAction : Actions
{
    Vector3 toPosition;

    public GoToAction(Villager vill, Vector3 toPosition)
    {
        this.vill = vill;
        this.toPosition = toPosition;
    }

    public override void Execute()
    {
        vill.agent.isStopped = false;
        vill.agent.SetDestination(toPosition);
    }

    public override void Stop()
    {
        vill.agent.isStopped = true;
    }

    public override void Update()
    {
        if(Vector3.SqrMagnitude(vill.transform.position - vill.agent.destination) <= .5f)
        {
            OnComplete?.Invoke();
        }
    }
}

public class WaitAction : Actions
{
    private float targetTime;

    public WaitAction(Villager vill, float time)
    {
        this.vill = vill;
        targetTime = time;
    }

    public override void Execute()
    {

    }

    public override void Stop()
    {
    }

    public override void Update()
    {
        targetTime -= Time.deltaTime;
        if (targetTime <= 0)
        {
            OnComplete?.Invoke();
        }
    }
}

public class WanderAction : Actions
{
    private float wanderRadius;
    private float wanderTime;
    private float waitTime;

    public WanderAction(Villager vill, float wanderRadius, float wanderTime, float waitTime)
    {
        this.vill = vill;
        this.wanderRadius = wanderRadius;
        this.wanderTime = wanderTime;
        this.waitTime = waitTime;
    }

    public override void Execute()
    {
        vill.agent.isStopped = false;
        vill.agent.SetDestination(vill.transform.position + Vector3.ProjectOnPlane((Random.insideUnitSphere * wanderRadius), Vector3.up));
    }
    public override void Stop()
    {
        vill.agent.isStopped = true;
    }

    public override void Update()
    {
         //OnComplete?.Invoke();
    }
}


public class TalkAction : Actions
{
    private Talk other;
    private Talk villagerTalk;
    private float targetTime;
    
    public TalkAction(Talk me, Talk other, float talkTime)
    {
        this.villagerTalk = me;
        this.vill = me.villager;
        this.other = other;
        targetTime = Time.time + talkTime;
    }

    public override void Execute()
    {
        vill.rigidBody.velocity = Vector3.zero;
        vill.rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        Vector3 deltaPos = other.transform.position - vill.transform.position;
        vill.agent.SetDestination(vill.transform.position + deltaPos * .5f);
        vill.transform.forward = Vector3.ProjectOnPlane(other.transform.position - vill.transform.position,Vector3.up);
    }
    
    public override void Stop()
    {
        vill.rigidBody.constraints = RigidbodyConstraints.None;
    }

    public override void Update()
    {
        if (targetTime <= Time.time)
        {
            OnComplete?.Invoke();
            vill.rigidBody.constraints = RigidbodyConstraints.None;
        }
    }
}
