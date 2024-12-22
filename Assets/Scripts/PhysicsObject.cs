using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    [SerializeField] private float waitOnPickup = 0.2f;
    [HideInInspector] public bool pickedUp = false;
    [HideInInspector] public bool wasPickedUp = false;
    [HideInInspector] public PlayerInteractions playerInteraction;

    public void PickUp(Transform parent)
    {
        transform.SetParent(parent);
        playerInteraction = parent.GetComponentInParent<PlayerInteractions>();
        pickedUp = true;
        StartCoroutine(PickUpCoroutine());
    }

    public void Drop()
    {
        transform.SetParent(null);
        pickedUp = false;
        playerInteraction = null;
    }

    private IEnumerator PickUpCoroutine()
    {
        yield return new WaitForSeconds(waitOnPickup);
        wasPickedUp = true;
    }
}
