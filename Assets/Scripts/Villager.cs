using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public enum Roles
{
    Villager,
    Priest,
    Trader
}

public class Villager : MonoBehaviour
{
    public Roles rol;
    [SerializeField] NavMeshAgent agent;

    private void Start()
    {
        var allMarkets = FindObjectsOfType<Market>();
        if (rol == Roles.Villager)
        {
            var toGo = allMarkets.OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault().buyerPosition;
            agent.SetDestination(toGo.position);
        }
        else if(rol == Roles.Priest)
        {
            
        }
        else if(rol == Roles.Trader)
        {
            var toGo = allMarkets.OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault().traderPosition;
            agent.SetDestination(toGo.position);
        }
    }

}
