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

        private Transform other;

        public SpreadFaith spreadFaith;

        public GameObject talkCollider;

        private void Awake()
        {
            openToTalk = false;
            targetTimeToStartTalkingAgain = Random.Range(.1f, 5f);
        }

        public void StartTalking(Talk other, float time, bool spreadFaith)
        {
            this.other = other.transform;
            openToTalk = false;
            targetTimeToStartTalkingAgain = Time.time + time + UnityEngine.Random.Range(5, 10);
            ActionTasks actionsTasks = new ActionTasks();
            var talkAction = new TalkAction(this, other, time);
            talkAction.OnComplete += StopTalking;
            actionsTasks.AddAction(talkAction);
            villager.OverrideTasks(actionsTasks);
            messagePopup.ShowMessage(villager.rol == Roles.Atheist, time);
            if (spreadFaith)
            {
                this.spreadFaith.Active(true);
            }
        }

        public void StopTalking()
        {
            other = null;
            this.spreadFaith.Active(false);
            messagePopup.Stop();
        }

        private void Update()
        {
            if (targetTimeToStartTalkingAgain <= Time.time)
            {
                openToTalk = true;
            }
        }

        public void Deactivate()
        {
            talkCollider.SetActive(false);
            enabled = false;
            messagePopup.Deactivate();
            StopTalking();
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
            bool aAtheist = a.villager.IsAtheist;
            bool bAtheist = b.villager.IsAtheist;
            if (aAtheist && !bAtheist)
            {
                a.StartTalking(b, talkTime, true);
                b.StartTalking(a,talkTime, false);
            }else if (!aAtheist && bAtheist)
            {
                a.StartTalking(b, talkTime, false);
                b.StartTalking(a,talkTime, true);
            }
            else
            {
                a.StartTalking(b, talkTime, true);
                b.StartTalking(a,talkTime, true);
            }
        }
    }
    
    
    
}
