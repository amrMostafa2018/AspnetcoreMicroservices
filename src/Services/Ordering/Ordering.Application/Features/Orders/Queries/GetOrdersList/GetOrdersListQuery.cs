using MediatR;
using System;
using System.Collections.Generic;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<List<OrdersVm>>
    {
        public string userName { get; set; }

        public GetOrdersListQuery(string userName)
        {
            userName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
