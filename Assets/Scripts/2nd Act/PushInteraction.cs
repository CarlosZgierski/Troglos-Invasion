using UnityEngine;

public class PushInteraction : MonoBehaviour {

    [SerializeField] private GameObject collisionWalls;

    public void WallsInteraction(bool _interactionBool)
    {
       collisionWalls.SetActive(_interactionBool);
    }
}
