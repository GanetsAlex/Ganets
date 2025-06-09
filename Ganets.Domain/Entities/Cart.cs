using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganets.Domain.Entities;

namespace Ganets.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        /// <summary> 
        /// Список объектов в корзине 
        /// key - идентификатор объекта 
        /// </summary> 
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        /// <summary> 
        /// Добавить объект в корзину 
        /// </summary> 
        /// <param name="gadget">Добавляемый объект</param> 
        public virtual void AddToCart(Gadget gadget)
        {
            if (CartItems.ContainsKey(gadget.Id))

        {
                CartItems[gadget.Id].Qty++;
            }
        else
            {
                CartItems.Add(gadget.Id, new CartItem
                {
                    Item = gadget,
                    Qty = 1
                });
            };
        }
        /// <summary> 
        /// Удалить объект из корзины 
        /// </summary> 
        /// <param name="gadget">удаляемый объект</param> 
        public virtual void RemoveItem(int id)
        {
            CartItems.Remove(id);
        }
        /// <summary> 
        /// Очистить корзину 
        /// </summary> 
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        /// <summary> 
        /// Количество объектов в корзине 
        /// </summary> 
        public int Count { get => CartItems.Sum(item => item.Value.Qty); }
        /// <summary> 
        /// Общее количество калорий 
        /// </summary> 
        public decimal TotalPrice => CartItems.Sum(item => item.Value.Item.Price * item.Value.Qty);        
    }
}
