//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ThePeejayAPI.Models;

//namespace ThePeejayAPI.Repositories
//{
//    public class OrderRepository : IOrderRepository
//    {
//        private readonly AppDbContext _context;

//        public OrderRepository(AppDbContext context)
//        {
//            _context = context;
//        }
//        public IQueryable<Order> Orders => _context.Orders
//                                .Include(o => o.Items)
//                                .ThenInclude(i => i.Product);
//        public void SaveOrder(Order order)
//        {
//            _context.AttachRange(order.Items.Select(i => i.Product));
//            if (order.OrderId == 0)
//            {
//                _context.Orders.Add(order);
//            }
//            _context.SaveChanges();
//        }
//    }
//}
