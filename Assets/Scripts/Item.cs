using DG.Tweening;
using Grid;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private uint itemID;
    public uint getItemID() { return itemID; }
    [SerializeField] private Cell cell;
    public Cell GetCurrentCell() { return cell; }
    public void SetCurrentCell(Cell cell) { this.cell = cell; }

    public Utilities.InteractableID GetID() { return Utilities.InteractableID.ITEM; }




    #region Animation

    private bool isSelected = false;
    private Vector2 defaultSize = Vector2.one;

    [SerializeField] private float hoverSizeMultipler = 1.2f;
    [SerializeField] private float clickSizeMultipler = .7f;
    [SerializeField] private float selectedSizeMultipler = 1.4f;
    [SerializeField] private float animationDuration = 0.2f;

    void Start() => defaultSize = transform.localScale;

    public void Select()
    {
        isSelected = true;
        transform.DOScale(defaultSize * selectedSizeMultipler, animationDuration)
                 .SetEase(Ease.OutBack);
    }

    public void UnSelect()
    {
        isSelected = false;
        transform.DOScale(defaultSize, animationDuration)
                 .SetEase(Ease.OutBack);
    }

    public void OnClickEnter()
    {
        if (isSelected) return;
        transform.DOScale(defaultSize * clickSizeMultipler, animationDuration)
                 .SetEase(Ease.OutBack);
    }

    public void OnClickExit()
    {
        if (isSelected) return;
        transform.DOScale(defaultSize, animationDuration)
                 .SetEase(Ease.OutBack);
    }

    public void OnHoverEnter()
    {
        if (isSelected) return;
        transform.DOScale(defaultSize * hoverSizeMultipler, animationDuration)
                 .SetEase(Ease.OutBack);
    }

    public void OnHoverExit()
    {
        if (isSelected) return;
        transform.DOScale(defaultSize, animationDuration)
                 .SetEase(Ease.OutBack);
    }

    #endregion


}
