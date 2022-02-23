using System.Collections.Generic;
using System.Linq;
using TalkSystem;
using UnityEngine;
using UnityEngine.AI;

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

    int currentActivity;
    private ActionTasks baseTasks;

    Stack<ActionTasks> toDoTasks = new Stack<ActionTasks>();

    bool doingDailies;

    private List<Actions> newTasks;

    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        var allMarkets = FindObjectsOfType<Market>();
        baseTasks = new ActionTasks();

        if (rol == Roles.Villager)
        {
            var toGo = allMarkets.OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault().buyerPosition;
            baseTasks.AddAction(new GoToAction(this, toGo.position));
            baseTasks.AddAction(new WaitAction(this, 5));
            baseTasks.AddAction(new GoToAction(this, startPos));
        }
        else if (this.rol == Roles.Priest)
        {
        }
        else if (this.rol == Roles.Trader)
        {
            var toGo = allMarkets.OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault().traderPosition;
            baseTasks.AddAction(new GoToAction(this, toGo.position));
            baseTasks.AddAction(new WaitAction(this, 5));
            baseTasks.AddAction(new GoToAction(this, startPos));
        }

        toDoTasks.Push(baseTasks);
        baseTasks.StartActivities();
    }

    public void OverrideTasks(ActionTasks newTasks)
    {
        toDoTasks.Peek().Stop();
        toDoTasks.Push(newTasks);
        toDoTasks.Peek().StartActivities();
        newTasks.OnEndAllTasks += ResumeBase;
    }

    public void ResumeBase()
    {
        toDoTasks.Pop().OnEndAllTasks -= ResumeBase;
        toDoTasks.Peek().Resume();
    }

    private void Update()
    {
        toDoTasks.Peek().Update();
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
        if(Vector3.SqrMagnitude(vill.transform.position - toPosition) <= .5f)
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
        vill.agent.isStopped = true;
        vill.transform.forward = Vector3.ProjectOnPlane(other.transform.position - vill.transform.position,Vector3.up);
    }
    
    public override void Stop()
    {
    }

    public override void Update()
    {
        if (targetTime <= Time.time)
        {
            OnComplete?.Invoke();
        }
    }
}

