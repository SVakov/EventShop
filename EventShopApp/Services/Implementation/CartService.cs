using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using EventShopApp.Services.Interfaces;
using EventShopApp.ViewModels;

namespace EventShopApp.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private static List<CartItemViewModel> CartItems = new List<CartItemViewModel>();

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CartItemViewModel> GetCartItems()
        {
            return CartItems;
        }

        public void AddToCart(CartItemViewModel item)
        {
            var existingItem = CartItems.FirstOrDefault(i => i.Id == item.Id && i.ItemType == item.ItemType);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                CartItems.Add(item);
            }
        }

        public void RemoveFromCart(int id)
        {
            var item = CartItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                CartItems.Remove(item);
            }
        }

        public void ClearCart()
        {
            CartItems.Clear();
        }

        public async Task<bool> SubmitOrder(OrderViewModel model)
        {
            //Console.WriteLine("SubmitOrder in CartService triggered.");
            //Console.WriteLine($"CartItems Count: {CartItems.Count}");
            //Console.WriteLine($"Order Details - Email: {model.Email}, Name: {model.Name}");

            if (!CartItems.Any()) return false;

            var client = _context.Clients.FirstOrDefault(c => c.Email == model.Email);

            if (client == null)
            {
                client = new Client
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Address = model.Address
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            var order = new Order
            {
                ClientId = client.Id,
                DateOfOrder = DateTime.Now,
                DeadLineDate = model.DeadLineDate,
                Status = OrderStatus.InProgress
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var cartItem in CartItems)
            {
                if (cartItem.ItemType == OrderType.Flower)
                {
                    var flower = _context.Flowers.FirstOrDefault(f => f.Id == cartItem.Id);
                    if (flower != null && flower.FlowerQuantity >= cartItem.Quantity)
                    {
                        flower.FlowerQuantity -= cartItem.Quantity;
                        _context.Flowers.Update(flower);

                        _context.OrderDetails.Add(new OrderDetail
                        {
                            OrderId = order.Id,
                            FlowerId = flower.Id,
                            OrderedFlowerQuantity = cartItem.Quantity,
                            Type = OrderType.Flower,
                            ArrangementItemsId = null,
                            OrderedArrangementQuantity = null,
                            IsPrepayed = false
                        });
                    }
                }
                else if (cartItem.ItemType == OrderType.Arrangement)
                {
                    var arrangement = _context.ArrangementItems.FirstOrDefault(a => a.Id == cartItem.Id);
                    if (arrangement != null && arrangement.ArrangementItemsQuantity >= cartItem.Quantity)
                    {
                        arrangement.ArrangementItemsQuantity -= cartItem.Quantity;
                        _context.ArrangementItems.Update(arrangement);

                        _context.OrderDetails.Add(new OrderDetail
                        {
                            OrderId = order.Id,
                            FlowerId = null,
                            OrderedFlowerQuantity = null,
                            Type = OrderType.Arrangement,
                            ArrangementItemsId = arrangement.Id,
                            OrderedArrangementQuantity = cartItem.Quantity,
                            IsPrepayed = false
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            ClearCart();
            return true;
        }
    }
}
