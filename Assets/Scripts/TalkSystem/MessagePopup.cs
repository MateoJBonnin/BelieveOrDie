using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
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
        ShowMessagesManager.instance.OnShowChanged += OnShowChanged;
    }

    private void OnShowChanged(bool show)
    {
        this.show = show;
        if (show)
        {
            renderer.enabled = true;
            renderer.transform.DOScale(baseScale, .5f);
        }
        else
        {
            renderer.transform.DOScale(Vector3.zero, .5f).OnComplete(() => renderer.enabled = false);
        }
    }

    private void OnDisable()
    {
        renderer.enabled = false;
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

        ShowMessagesManager.instance.OnShowChanged -= OnShowChanged;

        Activate(false);
    }
    
    private IEnumerator SpawnRandomMessages(List<Material> messages)
    {
        Vector3 scale = renderer.transform.localScale;
        
        while (true)
        {
            Material material = messages[Random.Range(0, messages.Count)];
            renderer.material = material;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(renderer.transform.DOScale(Vector3.zero, .25f).OnComplete(() => { renderer.enabled = show; }));
            mySequence.Append(renderer.transform.DOScale(scale, .4f).OnComplete(() => { renderer.enabled = show; }));
            mySequence.Play();
            yield return new WaitForSeconds(Random.Range(1, 2));
        }
    }

    private void Activate(bool active)
    {
        gameObject.SetActive(active);
        renderer.enabled = show;
    }
}
