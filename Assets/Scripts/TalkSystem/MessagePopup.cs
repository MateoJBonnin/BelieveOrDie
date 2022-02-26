using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class MessagePopup : MonoBehaviour
{
    public Renderer renderer;
    
    public List<Material> catholicismMessages;
    public List<Material> atheismMessages;
    private float targetTime;
    
    private Coroutine randomMessagesCoroutine;

    private bool show = false;

    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

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

    public void Deactivate()
    {
        if (randomMessagesCoroutine != null)
        {
            StopCoroutine(randomMessagesCoroutine);  
        }

        Activate(false);
    }

    private Sequence mySequence;
    
    private IEnumerator SpawnRandomMessages(List<Material> messages)
    {
        Vector3 scale = renderer.transform.localScale;
        
        while (true)
        {
            renderer.enabled = CameraController.CanShowMessages;
            Material material = messages[Random.Range(0, messages.Count)];
            renderer.material = material;
            mySequence = DOTween.Sequence();
            mySequence.Append(renderer.transform.DOScale(Vector3.zero, .25f));
            mySequence.Append(renderer.transform.DOScale(scale, .4f));
            mySequence.Play();
            yield return new WaitForSeconds(Random.Range(1, 2));
        }
    }

    private void Activate(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Stop()
    {
        if (randomMessagesCoroutine != null)
        {
            StopCoroutine(randomMessagesCoroutine);  
        }

        mySequence?.Pause();
    }
}
