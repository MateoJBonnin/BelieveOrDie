﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TalkSystem
{
    public class Talk : MonoBehaviour
    {
        public Villager villager;
        public float talkTime = 5;
        public float talkProbability = .2f;
        public bool openToTalk;

        private float targetTimeToStartTalkingAgain;

        public MessagePopup messagePopup;

        public Transform other;
        
        private void Awake()
        {
            openToTalk = false;
            targetTimeToStartTalkingAgain = Random.Range(.1f, 5f);
        }

        public void StartTalking(Talk other, float time)
        {
            this.other = other.transform;
            openToTalk = false;
            targetTimeToStartTalkingAgain = Time.time + time + UnityEngine.Random.Range(5,10);
            ActionTasks actionsTasks = new ActionTasks();
            actionsTasks.AddAction(new TalkAction(this,other, time));
            villager.OverrideTasks(actionsTasks);
            messagePopup.ShowMessage(villager.rol == Roles.Atheist, time);
        }

        public void StopTalking()
        {
            other = null;
        }

        private void Update()
        {
            if (targetTimeToStartTalkingAgain <= Time.time)
            {
                openToTalk = true;
                StopTalking();
            }
        }


        private void OnDrawGizmosSelected()
        {
            if (other != null)
                Gizmos.DrawLine(transform.position, other.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(openToTalk && other.attachedRigidbody.TryGetComponent(out Talk otherTalk))
            {
                if (otherTalk.openToTalk && Random.value <= talkProbability)
                {
                    TalkingManager.StartConversation(this, otherTalk, talkTime);                    
                }
            }
        }
    }
    
    
    public static class TalkingManager
    {
        public static void StartConversation(Talk a, Talk b, float talkTime)
        {
            a.StartTalking(b, talkTime);
            b.StartTalking(a,talkTime);
        }
    }
    
    
    
}
