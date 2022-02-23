using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TalkSystem;
using UnityEngine;

public class RoleManager : MonoBehaviour
{
    public Villager[] allVillagers;
    public Market[] allMarkets;
    public Farm[] allFarms;


    private void Start()
    {
        allVillagers = FindObjectsOfType<Villager>();
        allMarkets = FindObjectsOfType<Market>();
        allFarms = FindObjectsOfType<Farm>();

        SetupRoles();
    }

    private void SetupRoles()
    {
        float current = 0;

        float tradersCount = (float)allMarkets.Length;
        float WandererCount = (float)allVillagers.Length * 0.1f;
        float BuyerCount = (float)allVillagers.Length * 0.3f;

        for (int i = 0; i < allVillagers.Length; i++)
        {
            Villager vill = allVillagers[i];
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
