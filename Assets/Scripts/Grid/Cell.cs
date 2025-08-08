using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Grid
{
    public class Cell : MonoBehaviour
    {
        //maybe i'll add a new states in the future to make things more interesting (that's why i didnt prefer bool)
        protected enum CellState { OPEN, CLOSE };
        CellState currentState = CellState.OPEN;

        private static uint goodsAmount = 3;
        protected Item[] goods = new Item[goodsAmount];

        [SerializeField] private float spacing = .05f;
        [SerializeField] private Vector2 itemSize = new Vector2(.5f, .2f);
        public void SetGoods(Item[] goods)
        {
            if (goods == null)
            {
                Debug.LogWarning("Array must be non-null!!");
                for (int i = 0; i < goodsAmount; i++)
                    this.goods[i] = null;

                return;
            }

            //TODO : make a class for math
            Vector2 originOffset = new Vector2
            (
                ((goodsAmount - 1) * itemSize.x) / 2f,
                ((goodsAmount - 1) * itemSize.y) / 2f
            );

            for (int i = 0; i < goodsAmount; i++)
            {
                this.goods[i] = goods[i];

                if (this.goods[i] != null)
                {
                    Vector3 position = new Vector3
                    (
                        transform.position.x + ((i * (itemSize.x + spacing)) - originOffset.x) - spacing,
                        transform.position.y,
                        transform.position.z - 1
                    );

                    this.goods[i].transform.position = position;
                    this.goods[i].transform.parent = this.transform;
                }

            }
        }

        //TODO remove this if not necessary
        public void SetGoods(Item first = null, Item second = null, Item third = null)
        {
            SetGoods(new Item[] { first, second, third });
        }
    }
}