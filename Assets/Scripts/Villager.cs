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

    [SerializeField] private AudioSource screamSfx = null;

    public void Setup(ActionTasks bt)
    {
        startPos = transform.position;
        baseTasks = bt;

        toDoTasks.Push(baseTasks);
        baseTasks.StartActivities(true);
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

        screamSfx.pitch = Random.Range(0.8f, 1.2f);
        screamSfx.Play();
    }

    public void RestartBaseTasks()
    {
        if (isDead) return;

        baseTasks.StartActivities(true);
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

    public void ShuffleList()
    {

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

    public void StartActivities(bool randomActivities = false)
    {
        currentAction = randomActivities ? Random.Range(0, actions.Count) : 0;
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
    private float currentWaitingTime;

    public WaitAction(Villager vill, float time)
    {
        this.vill = vill;
        targetTime = time;
        currentWaitingTime = 0;
    }

    public override void Execute()
    {

    }

    public override void Stop()
    {
    }

    public override void Update()
    {
        currentWaitingTime += Time.deltaTime;
        if (currentWaitingTime >= targetTime)
        {
            currentWaitingTime = 0;
            OnComplete?.Invoke();
        }
    }
}

public class WanderAction : Actions
{
    private float wanderRadius;
    private float wanderTime;
    private float waitTime;
    private float totalTime;

    bool reach;
    bool waiting;
    float currentTimeWaiting;

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

        Vector3 toPosition = vill.transform.position + Vector3.ProjectOnPlane((Random.insideUnitSphere * wanderRadius), Vector3.up);
        NavMesh.SamplePosition(toPosition, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas);
        vill.agent.SetDestination(hit.position);
    }

    public override void Stop()
    {
        vill.agent.isStopped = true;
    }

    public override void Update()
    {
        if (!reach)
        {
            reach = Vector3.SqrMagnitude(vill.transform.position - vill.agent.destination) <= .5f;
        }
        else
        {
            if(!waiting)
            {
                OnReach();
            }
            else
            {
                currentTimeWaiting += Time.deltaTime;

                if(currentTimeWaiting >= waitTime)
                {
                    currentTimeWaiting = 0;
                    Execute();
                    reach = false;
                    waiting = false;
                }
            }
        }

        totalTime += Time.deltaTime;
        if (totalTime >= wanderTime)
        {
            reach = false;
            waiting = false;
            currentTimeWaiting = 0;
            totalTime = 0;
            OnComplete?.Invoke();
        }
    }

    private void OnReach()
    {
        waiting = true;
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
            vill.rigidBody.constraints = RigidbodyConstraints.None;
            OnComplete?.Invoke();
        }
    }
}

public class PathWalking : Actions
{
    Path pathToWalk;
    int currentIndex = -1;
    int totalWaypoints;


    int currentTotalIndex;

    int dir;

    public PathWalking(Villager vill, Path path, int totalWaypoints)
    {
        this.vill = vill;
        pathToWalk = path;
        currentTotalIndex = this.totalWaypoints = totalWaypoints;

        currentIndex = -1;
        dir = Random.Range(0, 2) * 2 - 1;
    }

    public override void Execute()
    {
        if(currentIndex < 0)
        {
            currentIndex = pathToWalk.waypoints.OrderBy(x => Vector3.Distance(x.transform.position, vill.transform.position)).FirstOrDefault().GetSiblingIndex();
        }

        vill.agent.isStopped = false;

        Vector3 toPosition = pathToWalk.waypoints[currentIndex].position + Vector3.ProjectOnPlane((Random.insideUnitSphere * 10), Vector3.up);
        NavMesh.SamplePosition(toPosition, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas);

        vill.agent.SetDestination(toPosition);
    }
    public override void Stop()
    {
        vill.agent.isStopped = true;
    }

    public override void Update()
    {
        if (Vector3.SqrMagnitude(vill.transform.position - vill.agent.destination) <= .5f)
        {
            currentIndex += dir;
            if (currentIndex >= pathToWalk.waypoints.Length)
            {
                currentIndex = 0;
            }
            else if(currentIndex < 0)
            {
                currentIndex = pathToWalk.waypoints.Length - 1;
            }

            currentTotalIndex--;
            if (currentTotalIndex <= 0)
            {
                dir = Random.Range(0, 2) * 2 - 1;
                currentTotalIndex = totalWaypoints;
                currentIndex = -1;
                OnComplete?.Invoke();
            }
            else
            {
                Vector3 toPosition = pathToWalk.waypoints[currentIndex].position + Vector3.ProjectOnPlane((Random.insideUnitSphere * 10), Vector3.up);
                NavMesh.SamplePosition(toPosition, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas);

                vill.agent.SetDestination(toPosition);
            }
        }
    }
}
