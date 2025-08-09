using System;
using System.Collections.Generic;
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
}
