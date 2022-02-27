using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TalkSystem;
using UnityEngine;

public class RoleManager : MonoBehaviour
{
    public Villager[] allVillager;
    public Market[] allMarkets;
    public Farm[] allFarms;
    public Path[] allPaths;


    public float atheismPercentage = .15f;
    private void Start()
    {
        allVillager = FindObjectsOfType<Villager>();
        allMarkets = FindObjectsOfType<Market>();
        allFarms = FindObjectsOfType<Farm>();
        allPaths = FindObjectsOfType<Path>();

        SetupRoles();
        CreateAtheists();
    }

    private void CreateAtheists()
    {
        float atheistCount = allVillager.Length * atheismPercentage;
        for (int i = 0; i < atheistCount; i++)
        {
            var a = allVillager.GetRandom();
            a.faithController.ConvertToAtheist();
        }
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    Time.timeScale *= 6;
        //}
        //if(Input.GetKeyUp(KeyCode.Space))
        //{
        //    Time.timeScale = 1;
        //}
    }

    private void SetupRoles()
    {
        int totalVillCount = allVillager.Length;

        float normalVill = totalVillCount * 0.7f;
        float farmersPercent = totalVillCount * 0.2f;
        float lumberjackPercent = totalVillCount * 0.1f;

        while(totalVillCount > 0)
        {
            Villager vill = allVillager[totalVillCount - 1];
            ActionTasks baseTasks = new ActionTasks();

            
            var toGo = allMarkets.OrderBy(x => (x.transform.position - vill.transform.position).magnitude).FirstOrDefault().buyerPosition;
            baseTasks.AddAction(new GoToAction(vill, toGo.position));
            baseTasks.AddAction(new WaitAction(vill, 5));
            baseTasks.AddAction(new WanderAction(vill, Random.Range(3, 5), 10, Random.Range(1, 5)));
            baseTasks.AddAction(new PathWalking(vill, allPaths[Random.Range(0, allPaths.Length)], Random.Range(5,20)));
            baseTasks.AddAction(new GoToVillagerAction(vill, allVillager));


            vill.Setup(baseTasks);

            totalVillCount--;
        }

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