using System.Collections.Generic;
using UnityEngine;

public class MessagePopup : MonoBehaviour
{
    public Renderer renderer;
    
    public List<Material> catholicismMessages;
    public List<Material> atheismMessages;
    public float targetTime;
    
    private void Update()
    {
        transform.forward = transform.position - Camera.main.transform.position;

        if (targetTime <= Time.time)
        {
            Activate(false);
        }
    }

    public void ShowMessage(bool IsAtheist, float time)
    {
        Activate(true);
        targetTime = Time.time + time;
        List<Material> messages = IsAtheist? atheismMessages : catholicismMessages;
        Material material = messages[Random.Range(0, messages.Count)];
        renderer.material = material;
    }

    private void Activate(bool active)
    {
        gameObject.SetActive(active);
    }
}
