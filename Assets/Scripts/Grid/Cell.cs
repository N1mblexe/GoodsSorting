using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Grid
{
    public class Cell : MonoBehaviour, IInteractable
    {
        //maybe i'll add a new states in the future to make things more interesting (that's why i didnt prefer bool)
        protected enum CellState { OPEN, CLOSE };
        CellState currentState = CellState.OPEN;

        private static uint maxGoods = GoodsManager.maxGoodsPerCell;
        protected List<Item> goods = new List<Item>();

        [SerializeField] private float spacing = .05f;
        [SerializeField] private Vector2 itemSize = new Vector2(.5f, .2f);
        private Vector2 originOffset;

        void Start()
        {
            originOffset = new Vector2
            (
                ((maxGoods - 1) * itemSize.x) / 2f,
                ((maxGoods - 1) * itemSize.y) / 2f
            );
        }
        public void SetGoods(Item[] goods)
        {
            if (goods == null)
            {
                Debug.LogWarning("Array must be non-null!!");
                return;
            }

            for (int i = 0; i < goods.Length; i++)
                AddItem(goods[i]);
        }

        public bool AddItem(Item item)
        {
            if (currentState == CellState.CLOSE || item == null || this.goods.Count >= maxGoods)
                return false;

            this.goods.Add(item);

            item.transform.parent = this.transform;
            item.SetCurrentCell(this);

            RecalculatePositions();
            return true;
        }

        /// <summary>
        /// Only removes from array , so handle it carefully
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItemFromGoods(Item item)
        {
            if (currentState == CellState.CLOSE || item == null)
                return;

            this.goods.Remove(item);

            RecalculatePositions();
        }

        //TODO : Make a ease anim for this
        public void RecalculatePositions()
        {
            for (int i = 0; i < this.goods.Count; i++)
                this.goods[i].transform.position = CalculatePosition(i);
        }

        public Vector3 CalculatePosition(int positionId)
        {
            return new Vector3
            (
                transform.position.x + ((positionId * (itemSize.x + spacing)) - originOffset.x) - spacing,
                transform.position.y,
                transform.position.z - 1
            );
        }

        //TODO remove this if not necessary
        public void SetGoods(Item first = null, Item second = null, Item third = null)
        {
            SetGoods(new Item[] { first, second, third });
        }

        public Utilities.InteractableID GetID() { return Utilities.InteractableID.CELL; }
    }
}