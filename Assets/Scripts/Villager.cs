using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum Roles
{
    Villager,
    Priest,
    Trader
}

public class Villager : MonoBehaviour
{
    public Roles rol;
    [SerializeField]
    private NavMeshAgent agent;

    private void Start()
    {
        Market[] allMarkets = FindObjectsOfType<Market>();
        if (this.rol == Roles.Villager)
        {
            Transform toGo = allMarkets.OrderBy(x => (x.transform.position - this.transform.position).magnitude).FirstOrDefault().buyerPosition;
            this.agent.SetDestination(toGo.position);
        }
        else if (this.rol == Roles.Priest)
        {
        }
        else if (this.rol == Roles.Trader)
        {
            Transform toGo = allMarkets.OrderBy(x => (x.transform.position - this.transform.position).magnitude).FirstOrDefault().traderPosition;
            this.agent.SetDestination(toGo.position);
        }
    }

    public void OnDieHandler()
    {
        Destroy(this.agent);
    }
}
