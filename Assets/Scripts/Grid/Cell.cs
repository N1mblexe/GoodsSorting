using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Grid
{
    public class Cell : MonoBehaviour
    {
        //maybe i'll add a new state in the future to make things more interesting (that's why i didnt prefer bool)
        protected enum State { OPEN, CLOSE };
        State currentState = State.OPEN;

        private static uint goodsSize = 3;
        protected Item[] goods = new Item[goodsSize];

        [SerializeField] private float spacing = .05f;
        public void SetGoods(Item[] goods)
        {
            if (goods == null)
            {
                Debug.LogWarning("Array must be non-null!!");
                for (int i = 0; i < goodsSize; i++)
                    this.goods[i] = null;

                return;
            }

            for (int i = 0; i < goodsSize; i++)
            {
                this.goods[i] = goods[i];

                //TODO SET POSITION
                if (this.goods[i] != null)
                {

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