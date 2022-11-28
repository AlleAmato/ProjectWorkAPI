using ProjectWorkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWorkAPI.Dto
{
    public class OrderItemDto
    {
        public string OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double UnitaryPrice { get; set; }
    }
}