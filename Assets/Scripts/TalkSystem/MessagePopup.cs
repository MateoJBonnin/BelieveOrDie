using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MessagePopup : MonoBehaviour
{
    public Renderer renderer;
    
    public List<Material> catholicismMessages;
    public List<Material> atheismMessages;
    private float targetTime;
    
    private Coroutine randomMessagesCoroutine;
    

    private void Update()
    {
        transform.forward = transform.position - Camera.main.transform.position;

        if (targetTime <= Time.time)
        {
            Activate(false);
            StopCoroutine(randomMessagesCoroutine);
        }
    }

    public void ShowMessage(bool IsAtheist, float time)
    {
        Activate(true);
        targetTime = Time.time + time;
        List<Material> messages = IsAtheist? atheismMessages : catholicismMessages;
        RandomizeMessages(messages);
    }

    
    private void RandomizeMessages(List<Material> messages)
    {
        randomMessagesCoroutine = StartCoroutine(SpawnRandomMessages(messages));
    }

    private IEnumerator SpawnRandomMessages(List<Material> messages)
    {
        Vector3 scale = renderer.transform.localScale;
        
        
        while (true)
        {
            Material material = messages[Random.Range(0, messages.Count)];
            renderer.material = material;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(renderer.transform.DOScale(Vector3.zero, .1f));
            mySequence.Append(renderer.transform.DOScale(scale, .2f));
            mySequence.Play();
            yield return new WaitForSeconds(Random.Range(1, 2));
        }
    }

    private void Activate(bool active)
    {
        gameObject.SetActive(active);
    }
}
