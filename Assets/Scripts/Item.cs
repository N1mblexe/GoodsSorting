using System;
using Grid;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private Cell cell;
    public Cell GetCurrentCell() { return cell; }
    public void SetCurrentCell(Cell cell) { this.cell = cell; }

    public Utilities.InteractableID GetID() { return Utilities.InteractableID.ITEM; }
}
