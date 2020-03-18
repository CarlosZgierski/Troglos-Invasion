using UnityEngine;

public class VaultTrigger : MonoBehaviour {

    [SerializeField] GameObject targetVault;

    public GameObject VaultTarget()
    {
        return targetVault;
    }
}
