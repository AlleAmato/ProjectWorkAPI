using AutoMapper;
using ProjectWorkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWorkAPI.Dto
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderDto>();

            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<OrderStateDto, OrderState>();
            CreateMap<OrderState, OrderStateDto>();
        }
    }
}