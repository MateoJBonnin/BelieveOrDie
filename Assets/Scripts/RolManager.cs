using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TalkSystem;
using UnityEngine;

public class RolManager : MonoBehaviour
{
    public Villager[] allVillagers;
    public Market[] allMarkets;
    public Farm[] allFarms;


    private void Start()
    {
        allVillagers = FindObjectsOfType<Villager>();
        allMarkets = FindObjectsOfType<Market>();
        allFarms = FindObjectsOfType<Farm>();

        SetupRols();
    }

    private void SetupRols()
    {
        foreach (var vill in allVillagers)
        {
            ActionTasks baseTasks = new ActionTasks();

            if (vill.rol == Roles.Villager)
            {
                var toGo = allMarkets.OrderBy(x => (x.transform.position - vill.transform.position).magnitude).FirstOrDefault().buyerPosition;
                baseTasks.AddAction(new GoToAction(vill, toGo.position));
                baseTasks.AddAction(new WaitAction(vill, 5));
                baseTasks.AddAction(new GoToAction(vill, vill.startPos));
            }
            else if (vill.rol == Roles.Priest)
            {
            }
            else if (vill.rol == Roles.Trader)
            {
                var toGo = allMarkets.OrderBy(x => (x.transform.position - vill.transform.position).magnitude).FirstOrDefault().traderPosition;
                baseTasks.AddAction(new GoToAction(vill, toGo.position));
                baseTasks.AddAction(new WaitAction(vill, 5));
                baseTasks.AddAction(new GoToAction(vill, vill.startPos));
            }


            vill.Setup(baseTasks);
        }
    }
}
