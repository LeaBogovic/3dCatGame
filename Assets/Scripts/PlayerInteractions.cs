using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private string doorTag = "Door";
    [SerializeField] private string itemTag = "Item";
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform pickupParent;

    [Header("Keybinds")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("UI")]
    [SerializeField] private Image uiPanel;
    [SerializeField] private Text panelText;
    [SerializeField] private string pickupText = "Pick up";
    [SerializeField] private string dropText = "Drop";
    [SerializeField] private string openText = "Open";
    [SerializeField] private string closeText = "Close";

    private PhysicsObject _currentlyPickedUpObject;
    private Door _currentDoor;
    private Rigidbody _pickupRigidBody;

    private void Update()
    {
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.CompareTag(itemTag))
            {
                ShowUI(pickupText);
                if (Input.GetKeyDown(interactionKey))
                    TogglePickup(hit.collider.GetComponent<PhysicsObject>());
            }
            else if (hit.collider.CompareTag(doorTag))
            {
                Door door = hit.collider.GetComponent<Door>();
                ShowUI(door.doorOpen ? closeText : openText);
                if (Input.GetKeyDown(interactionKey))
                    door.PlayDoorAnimation();
            }
        }
        else
        {
            HideUI();
        }
    }

    private void TogglePickup(PhysicsObject item)
    {
        if (_currentlyPickedUpObject == null)
        {
            _currentlyPickedUpObject = item;
            _currentlyPickedUpObject.PickUp(pickupParent);
        }
        else
        {
            _currentlyPickedUpObject.Drop();
            _currentlyPickedUpObject = null;
        }
    }

    public void BreakConnection()
    {
        if (_currentlyPickedUpObject != null)
        {
            _currentlyPickedUpObject.pickedUp = false;
            _currentlyPickedUpObject.transform.SetParent(null);
            _pickupRigidBody.constraints = RigidbodyConstraints.None;
            _currentlyPickedUpObject = null;
        }
    }

    private void ShowUI(string message)
    {
        if (uiPanel)
        {
            uiPanel.gameObject.SetActive(true);
            panelText.text = message;
        }
    }

    private void HideUI()
    {
        if (uiPanel) uiPanel.gameObject.SetActive(false);
    }
}
