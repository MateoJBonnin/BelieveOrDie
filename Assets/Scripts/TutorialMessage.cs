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
        
        private void Awake()
        {
            instance = this;
        }

        public void ShowMessage(string message)
        {
            animator.SetTrigger("IN");
            instance.text.text  = message;
        }

        public void Hide()
        {
            animator.SetTrigger("OUT");
        }

    }
}