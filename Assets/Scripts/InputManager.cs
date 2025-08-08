using Grid;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject currentObj;
    [SerializeField] private Item selectedItem = null;

    private void Update()
    {
        HandleMouseInteraction();
    }

    private void HandleMouseInteraction()
    {
        GameObject hitObject = GetObjectUnderMouse();
        IInteractable interactable = hitObject?.GetComponent<IInteractable>();

        HandleHoverChange(hitObject, interactable);
        HandleHoverAndClick(interactable, hitObject);
    }

    #region HelperFunctions
    private GameObject GetObjectUnderMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
        return hit.collider?.gameObject;
    }

    private void HandleHoverChange(GameObject hitObject, IInteractable interactable)
    {
        if (currentObj == hitObject)
            return;

        if (currentObj != null)
        {
            IInteractable previousInteractable = currentObj.GetComponent<IInteractable>();
            if (previousInteractable != null)
                OnHoverExit(previousInteractable);
        }

        if (interactable != null)
            OnHoverEnter(interactable);

        currentObj = hitObject;
    }

    private void HandleHoverAndClick(IInteractable interactable, GameObject gameObject)
    {
        if (interactable == null)
            return;

        OnHover(interactable);

        if (Input.GetMouseButtonDown(0))
            OnClickEnter(interactable);
        else if (Input.GetMouseButton(0))
            OnClick(interactable);
        else if (Input.GetMouseButtonUp(0))
            OnClickExit(interactable, gameObject);
    }
    #endregion

    #region Events
    public void OnClickEnter(IInteractable interactable) => interactable.OnClickEnter();
    public void OnClick(IInteractable interactable) => interactable.OnClick();
    public void OnClickExit(IInteractable interactable, GameObject gameObject)
    {
        interactable.OnClickExit();
        Utilities.InteractableID id = interactable.GetID();

        if (id == Utilities.InteractableID.ITEM)
        {
            var currentItem = gameObject.GetComponent<Item>();
            if (selectedItem == null)
                selectedItem = currentItem;
            else
                TryTransferItem(selectedItem, currentItem.GetCurrentCell());
        }
        if (id == Utilities.InteractableID.CELL && selectedItem != null)
            TryTransferItem(selectedItem, gameObject.GetComponent<Cell>());

    }

    //TODO make a seperate class for the transferring items or move it to the goods manager 
    public void TryTransferItem(Item item, Cell cell)
    {
        if (item == null || cell == null)
        {
            Debug.LogError("Null value error");
            return;
        }

        if (item.GetCurrentCell() == cell)
        {
            Debug.LogWarning("Clicked on current cell");
            return;
        }

        item.GetCurrentCell().RemoveItemFromGoods(item);
        if (cell.AddItem(item))
            selectedItem = null;

    }

    public void OnHoverEnter(IInteractable interactable) => interactable.OnHoverEnter();
    public void OnHover(IInteractable interactable) => interactable.OnHover();
    public void OnHoverExit(IInteractable interactable) => interactable.OnHoverExit();
    #endregion
}