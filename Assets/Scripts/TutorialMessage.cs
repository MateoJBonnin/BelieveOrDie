using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class TutorialMessage : MonoBehaviour
    {
        public static TutorialMessage instance;

        public TMPro.TextMeshProUGUI text;
        public Animator animator;
        public bool complete;

        private Coroutine currentCoroutine;
        public GameObject arrow;

        private void Awake()
        {
            instance = this;
        }

        public void ShowMessage(string message)
        {
            arrow.SetActive(false);

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            instance.text.text = "";

            animator.SetTrigger("IN");

            currentCoroutine = StartCoroutine(TextCoroutine(message));
        }

        private IEnumerator TextCoroutine(string textToShow)
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < textToShow.Length; i++)
            {
                instance.text.text += textToShow[i];
                yield return new WaitForSeconds(0.03f);
            }

            instance.text.text = textToShow;

            arrow.SetActive(true);
        }

        public void Hide()
        {
            arrow.SetActive(false);

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            instance.text.text = "";
            animator.SetTrigger("OUT");
        }

    }
}