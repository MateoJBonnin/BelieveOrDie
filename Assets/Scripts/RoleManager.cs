using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TalkSystem;
using UnityEngine;

public class RoleManager : MonoBehaviour
{
    
    public Villager[] allVillagersAndTraders;
    public Villager[] onlyVillager;
    public Market[] allMarkets;
    public Farm[] allFarms;

    public float atheismPercentage = .15f;
    private void Start()
    {
        allVillagersAndTraders = FindObjectsOfType<Villager>();
        onlyVillager = allVillagersAndTraders.Where(villager => villager.rol == Roles.Villager).ToArray();
        allMarkets = FindObjectsOfType<Market>();
        allFarms = FindObjectsOfType<Farm>();

        SetupRoles();
        CreateAtheists();
    }

    private void CreateAtheists()
    {
        float atheistCount = allVillagersAndTraders.Length * atheismPercentage;
        for (int i = 0; i < atheistCount; i++)
        {
            var a = onlyVillager.GetRandom();
            a.faithController.ConvertToAtheist();
        }
    }

    private void SetupRoles()
    {
        float current = 0;

        float tradersCount = (float)allMarkets.Length;
        float WandererCount = (float)allVillagersAndTraders.Length * 0.1f;
        float BuyerCount = (float)allVillagersAndTraders.Length * 0.3f;

        for (int i = 0; i < allVillagersAndTraders.Length; i++)
        {
            Villager vill = allVillagersAndTraders[i];
            ActionTasks baseTasks = new ActionTasks();

            if (i < tradersCount)
            {
                var toGo = allMarkets[i].traderPosition;
                baseTasks.AddAction(new GoToAction(vill, toGo.position));
                baseTasks.AddAction(new WaitAction(vill, 5));
                baseTasks.AddAction(new GoToAction(vill, vill.startPos));
            }
            else
            {
                var toGo = allMarkets.OrderBy(x => (x.transform.position - vill.transform.position).magnitude).FirstOrDefault().buyerPosition;
                baseTasks.AddAction(new GoToAction(vill, toGo.position));
                baseTasks.AddAction(new WaitAction(vill, 5));
                baseTasks.AddAction(new GoToAction(vill, vill.startPos));
            }

            vill.Setup(baseTasks);
            current++;
        }
    }

    private ActionTasks WanderTasks(Villager vill)
    {
        ActionTasks baseTasks = new ActionTasks();

        baseTasks.AddAction(new WanderAction(vill, Random.Range(3,5) , 10, Random.Range(1, 5)));

        return baseTasks;
    }
}


public static class Extensions
{
    public static T GetRandom<T>(this T[] objs)
    {
        var index = Random.Range(0, objs.Length);
        T r = objs[index]; 
        return r;
    }
}