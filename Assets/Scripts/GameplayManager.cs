using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameplayManager : MonoBehaviour
    {
        public List<GameObject> startsGameplayBehaviours;
        public CameraController cameraController;

        public void StartGame()
        {
            foreach (var go in startsGameplayBehaviours)
            {
                go.SetActive(true);
            }
        }
    }

}