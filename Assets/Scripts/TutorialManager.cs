using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using TalkSystem;

namespace DefaultNamespace
{
    public class TutorialManager : MonoBehaviour
    {
        public CameraController cameraController;

        private Camera mainCamera;

        public Collider firstStepTargetCameraPosition;
        
        private Collider objectHit;

        public Villager christian;
        public Villager atheist;
        
        public void Awake()
        {
            mainCamera = Camera.main;
            StartCoroutine(StartTutorial());
            
            ActionTasks atheistTasks = new ActionTasks();
            atheistTasks.AddAction(new WaitAction(christian,100000));
            atheist.Setup(atheistTasks);

            ActionTasks christianTasks = new ActionTasks();
            christianTasks.AddAction(new WaitAction(christian,100000));
            christian.Setup(christianTasks);
        }

        private IEnumerator StartTutorial()
        {            
            yield return new WaitForSeconds(.25f);

            DeactivateInputs();
            TutorialMessage.instance.ShowMessage("You Can Move with W A S D or pressing and dragging the scroll wheel, Try it.");
            yield return new WaitForSeconds(.25f);
            cameraController.SetMovementActivate(true);
            cameraController.SetDragActivate(true);
            yield return new WaitUntil(() => objectHit == firstStepTargetCameraPosition);
            TutorialMessage.instance.Hide();
            cameraController.SetMovementActivate(false);
            cameraController.SetDragActivate(false);
            
            firstStepTargetCameraPosition.gameObject.SetActive(false);
            
            //TODO CENTRAR LA CAMARA PARA Q EL ZOOM DE A DONDE ESTAN LOS FLAQUITOS!!!
            yield return new WaitForSeconds(1f);


            TutorialMessage.instance.ShowMessage("You can ZOOM with the scroll wheel and hear what the villager are talking about.");
            yield return new WaitForSeconds(.25f);
            cameraController.SetZoomActivate(true);
            yield return new WaitUntil(() => cameraController.transform.position.y < 22);
            TutorialMessage.instance.Hide();
            
            yield return new WaitForSeconds(.5f);
            
            atheist.faithController.ConvertToAtheist();
            TalkingManager.StartConversation(atheist.talk, christian.talk, atheist.talk.talkTime);
            yield return new WaitForSeconds(3f);
            christian.faithController.ConvertToAtheist();
            christian.talk.StopTalking();
            atheist.talk.StopTalking();
            
            TutorialMessage.instance.ShowMessage("Look, That Atheist convert the Christian.");
            yield return new WaitForSeconds(4f);
            TutorialMessage.instance.Hide();
            yield return new WaitForSeconds(.2f);
            TalkingManager.StartConversation(atheist.talk, christian.talk, atheist.talk.talkTime);
            yield return new WaitForSeconds(2f);
            
            //cameraController.SetMovementActivate(true);
            //cameraController.SetDragActivate(true);

            TutorialMessage.instance.ShowMessage("Kill the Atheists.");
            yield return new WaitUntil( () => atheist.isDead && christian.isDead );
            Debug.LogError("TUTORIAL ENDED");
        }

        private void DeactivateInputs()
        {
            cameraController.SetMovementActivate(false);
            cameraController.SetDragActivate(false);
            cameraController.SetZoomActivate(false);
        }

        public void Update()
        {
            CheckColliders();
        }
        
        public void CheckColliders()
        {
            if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 500.0f))
            {
                objectHit = hit.collider;
            }
        }
    }
}