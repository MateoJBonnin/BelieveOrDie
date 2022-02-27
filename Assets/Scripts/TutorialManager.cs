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

        private GodHand godHand;

        bool passTutorial;
        
        public void Awake()
        {
            godHand = FindObjectOfType<GodHand>();
            mainCamera = Camera.main;
            ActionTasks atheistTasks = new ActionTasks();
            atheistTasks.AddAction(new WaitAction(christian,1));
            atheist.Setup(atheistTasks);

            ActionTasks christianTasks = new ActionTasks();
            christianTasks.AddAction(new WaitAction(christian,1));
            christian.Setup(christianTasks);


            StartCoroutine(StartTutorial());
        }


        private IEnumerator StartTutorial()
        {
            DeactivateInputs();

            TutorialMessage.instance.ShowMessage("Welcome to this world my lord");

            yield return new WaitForSeconds(3);
            yield return new WaitUntil(() => passTutorial);
            
            TutorialMessage.instance.Hide();
            TutorialMessage.instance.ShowMessage("You are here because of the people who believe in you");

            yield return new WaitForSeconds(3);
            yield return new WaitUntil(() => passTutorial);

            TutorialMessage.instance.Hide();
            TutorialMessage.instance.ShowMessage("But you have to be aware of those who don't");

            yield return new WaitForSeconds(3);
            yield return new WaitUntil(() => passTutorial);

            TutorialMessage.instance.ShowMessage("You can move with W A S D or pressing and dragging the scroll wheel");
            yield return new WaitForSeconds(5f);
            TutorialMessage.instance.Hide();

            TutorialMessage.instance.ShowMessage("Go to the nearest town in the north");
            cameraController.SetMovementActivate(true);
            cameraController.SetDragActivate(true);
            firstStepTargetCameraPosition.gameObject.SetActive(true);


            yield return new WaitUntil(() => objectHit == firstStepTargetCameraPosition);
            TutorialMessage.instance.Hide();
            cameraController.SetMovementActivate(false);
            cameraController.SetDragActivate(false);
            firstStepTargetCameraPosition.gameObject.SetActive(false);
            
            ////TODO CENTRAR LA CAMARA PARA Q EL ZOOM DE A DONDE ESTAN LOS FLAQUITOS!!!
            //yield return new WaitForSeconds(1f);


            TutorialMessage.instance.ShowMessage("You can ZOOM with the scroll wheel and hear what the villagers are talking about");
            yield return new WaitForSeconds(.25f);
            cameraController.SetZoomActivate(true);
            yield return new WaitUntil(() => cameraController.transform.position.y < 22);
            cameraController.SetZoomActivate(false);
            cameraController.transform.position = new Vector3(cameraController.transform.position.x, 22, cameraController.transform.position.z);
            TutorialMessage.instance.Hide();
            
            yield return new WaitForSeconds(.5f);
            
            atheist.faithController.ConvertToAtheist();
            TalkingManager.StartConversation(atheist.talk, christian.talk, atheist.talk.talkTime);

            yield return new WaitForSeconds(3f);
            christian.faithController.ConvertToAtheist();
            christian.talk.StopTalking();
            atheist.talk.StopTalking();
            
            TutorialMessage.instance.ShowMessage("Look my lord!, That person is trying to convince the other one to stop believing in you!");
            yield return new WaitForSeconds(5f);
            yield return new WaitUntil(() => passTutorial);
            TutorialMessage.instance.Hide();

            TalkingManager.StartConversation(atheist.talk, christian.talk, atheist.talk.talkTime);
            yield return new WaitForSeconds(2f);



            TutorialMessage.instance.ShowMessage("Oh no! is too late now, we have to erase both now");
            yield return new WaitForSeconds(5f);
            yield return new WaitUntil(() => passTutorial);
            TutorialMessage.instance.Hide();

            TutorialMessage.instance.ShowMessage("Use your thunder power to stop these people from spreading the word");
            godHand.InputBlock = false;
            yield return new WaitUntil(() => atheist.isDead && christian.isDead);
            TutorialMessage.instance.Hide();
            godHand.InputBlock = true;

            TutorialMessage.instance.ShowMessage("Good job my lord you have cleaned the land from non-believers");
            yield return new WaitForSeconds(3f);
            yield return new WaitUntil(() => passTutorial);
            TutorialMessage.instance.Hide();

            TutorialMessage.instance.ShowMessage("Now we have to go to the other city, I have been comunicated that there are people there that does not believe in you");
            yield return new WaitForSeconds(4f);
            yield return new WaitUntil(() => passTutorial);
            TutorialMessage.instance.Hide();

            TutorialMessage.instance.ShowMessage("But take care, if the town reach a high percentage of non-believers you will stop being REAL");
            yield return new WaitForSeconds(4f);
            yield return new WaitUntil(() => passTutorial);
            TutorialMessage.instance.Hide();


            PlayerPrefs.SetInt("tuto", 1);

            LevelManager.instance.ChangeScene(2);
        }

        private void DeactivateInputs()
        {
            cameraController.SetMovementActivate(false);
            cameraController.SetDragActivate(false);
            cameraController.SetZoomActivate(false);
            godHand.InputBlock = true;
        }

        public void Update()
        {
            CheckColliders();

            if (Input.GetMouseButtonDown(0))
            {
                passTutorial = true;
            }
            else
            {
                passTutorial = false;
            }
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