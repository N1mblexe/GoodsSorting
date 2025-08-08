using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject currentObj;
    private void Update()
    {
        HandleMouseInteraction();
    }

    private void HandleMouseInteraction()
    {
        GameObject hitObject = GetObjectUnderMouse();
        IInteractable interactable = hitObject?.GetComponent<IInteractable>();

        HandleHoverChange(hitObject, interactable);
        HandleHoverAndClick(interactable);
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

    private void HandleHoverAndClick(IInteractable interactable)
    {
        if (interactable == null)
            return;

        OnHover(interactable);

        if (Input.GetMouseButtonDown(0))
            OnClickEnter(interactable);
        else if (Input.GetMouseButton(0))
            OnClick(interactable);
        else if (Input.GetMouseButtonUp(0))
            OnClickExit(interactable);
    }
    #endregion

    #region Events
    public void OnClickEnter(IInteractable button) => button.OnClickEnter();
    public void OnClick(IInteractable button) => button.OnClick();
    public void OnClickExit(IInteractable button) => button.OnClickExit();
    public void OnHoverEnter(IInteractable button) => button.OnHoverEnter();
    public void OnHover(IInteractable button) => button.OnHover();
    public void OnHoverExit(IInteractable button) => button.OnHoverExit();
    #endregion
}