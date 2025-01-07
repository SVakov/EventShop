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
            if (item.ItemType == OrderType.Flower)
            {
                var flower = _context.Flowers.FirstOrDefault(f => f.Id == item.Id);
                if (flower == null || flower.FlowerQuantity < item.Quantity)
                {
                    throw new InvalidOperationException("Not enough stock for the selected flower.");
                }

                flower.FlowerQuantity -= item.Quantity;
                Console.WriteLine($"Stock after deduction: {flower.FlowerQuantity}");
                _context.Flowers.Update(flower);
            }
            else if (item.ItemType == OrderType.Arrangement)
            {
                var arrangement = _context.ArrangementItems.FirstOrDefault(a => a.Id == item.Id);
                if (arrangement == null || arrangement.ArrangementItemsQuantity < item.Quantity)
                {
                    throw new InvalidOperationException("Not enough stock for the selected arrangement.");
                }

                arrangement.ArrangementItemsQuantity -= item.Quantity;
                _context.ArrangementItems.Update(arrangement);
            }

            _context.SaveChanges();

            var existingItem = CartItems.FirstOrDefault(i => i.Id == item.Id && i.ItemType == item.ItemType);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                existingItem.AddedAt = DateTime.UtcNow;
            }
            else
            {
                item.AddedAt = DateTime.UtcNow;
                CartItems.Add(item);
            }
        }


        public void RemoveFromCart(int id)
        {
            var item = CartItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                if (item.ItemType == OrderType.Flower)
                {
                    var flower = _context.Flowers.FirstOrDefault(f => f.Id == item.Id);
                    if (flower != null)
                    {
                        flower.FlowerQuantity += item.Quantity;
                        _context.Flowers.Update(flower);
                    }
                }
                else if (item.ItemType == OrderType.Arrangement)
                {
                    var arrangement = _context.ArrangementItems.FirstOrDefault(a => a.Id == item.Id);
                    if (arrangement != null)
                    {
                        arrangement.ArrangementItemsQuantity += item.Quantity;
                        _context.ArrangementItems.Update(arrangement);
                    }
                }

                _context.SaveChanges();
                CartItems.Remove(item);
            }
        }


        public void ClearCart()
        {
            foreach (var item in CartItems)
            {
                if (item.ItemType == OrderType.Flower)
                {
                    var flower = _context.Flowers.FirstOrDefault(f => f.Id == item.Id);
                    if (flower != null)
                    {
                        flower.FlowerQuantity += item.Quantity;
                        _context.Flowers.Update(flower);
                    }
                }
                else if (item.ItemType == OrderType.Arrangement)
                {
                    var arrangement = _context.ArrangementItems.FirstOrDefault(a => a.Id == item.Id);
                    if (arrangement != null)
                    {
                        arrangement.ArrangementItemsQuantity += item.Quantity;
                        _context.ArrangementItems.Update(arrangement);
                    }
                }
            }

            // Save changes to update stock
            _context.SaveChanges();

            // Clear the cart
            CartItems.Clear();
        }


        public void CleanupStaleCartItems(TimeSpan timeout)
        {
            var now = DateTime.UtcNow;
            var staleItems = CartItems.Where(i => now - i.AddedAt > timeout).ToList();

            foreach (var item in staleItems)
            {
                if (item.ItemType == OrderType.Flower)
                {
                    var flower = _context.Flowers.FirstOrDefault(f => f.Id == item.Id);
                    if (flower != null)
                    {
                        flower.FlowerQuantity += item.Quantity;
                        _context.Flowers.Update(flower);
                    }
                }
                else if (item.ItemType == OrderType.Arrangement)
                {
                    var arrangement = _context.ArrangementItems.FirstOrDefault(a => a.Id == item.Id);
                    if (arrangement != null)
                    {
                        arrangement.ArrangementItemsQuantity += item.Quantity;
                        _context.ArrangementItems.Update(arrangement);
                    }
                }

                CartItems.Remove(item);
            }

            _context.SaveChanges();
        }


        public async Task<bool> SubmitOrder(OrderViewModel model)
        {
            if (!CartItems.Any()) return false;

            // Find or create the client
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

            // Create the order
            var order = new Order
            {
                ClientId = client.Id,
                DateOfOrder = DateTime.Now,
                DeadLineDate = model.DeadLineDate,
                Status = OrderStatus.Pending
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Order created: {order.Id}");

            // Validate and process cart items
            foreach (var cartItem in CartItems.ToList())
            {
                if (cartItem.ItemType == OrderType.Flower)
                {
                    var flower = _context.Flowers.FirstOrDefault(f => f.Id == cartItem.Id);
                    if (flower != null && flower.FlowerQuantity >= cartItem.Quantity)
                    {
                        flower.FlowerQuantity -= cartItem.Quantity;
                        _context.Flowers.Update(flower);



                        // Add order detail for flower
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

                        // Add order detail for arrangement
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
            Console.WriteLine("OrderDetails saved.");

            ClearCart();

            return true;
        }

    }
}
