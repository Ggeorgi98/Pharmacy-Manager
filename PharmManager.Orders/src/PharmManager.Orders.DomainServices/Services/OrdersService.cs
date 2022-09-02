using MassTransit;
using PharmManager.Orders.Contracts;
using PharmManager.Orders.Domain.Dtos;
using PharmManager.Orders.Domain.Repositories;
using PharmManager.Orders.Domain.Services;

namespace PharmManager.Orders.DomainServices.Services
{
    public class OrdersService : BaseCrudService<OrderDto>, IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersService(IOrdersRepository ordersRepository, IPublishEndpoint publishEndpoint)
            : base(ordersRepository)
        {
            _ordersRepository = ordersRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task SubmitOrderAsync(Guid orderId)
        {
            await _ordersRepository.ChangeOrderStateAsync(orderId, OrderState.Submitted)
                .ConfigureAwait(false);            
        }

        public async Task ReturnOrderAsync(Guid orderId)
        {
            await _ordersRepository.ChangeOrderStateAsync(orderId, OrderState.Created)
                .ConfigureAwait(false);
        }

        public async Task CloseOrderAsync(Guid orderId)
        {
            await _ordersRepository.ChangeOrderStateAsync(orderId, OrderState.Closed)
                .ConfigureAwait(false);
        }

        public async Task RejectOrderAsync(Guid orderId)
        {
            await _ordersRepository.ChangeOrderStateAsync(orderId, OrderState.Rejected)
                .ConfigureAwait(false);
        }
    }
}
