﻿using CesarBmx.Shared.Messaging.Ordering.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Ordering.Persistence.Contexts;
using CesarBmx.Ordering.Application.Services;

namespace CesarBmx.Ordering.Application.Consumers
{
    public class OrderSubmittedConsumer : IConsumer<OrderSubmitted>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderSubmittedConsumer> _logger;
        private readonly ActivitySource _activitySource;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly OrderService _orderService;

        public OrderSubmittedConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<OrderSubmittedConsumer> logger,
            ActivitySource activitySource,
            IPublishEndpoint publishEndpoint,
            OrderService orderService)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
            _publishEndpoint = publishEndpoint;
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<OrderSubmitted> context)
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(OrderSubmitted));

                var orderSubmitted = context.Message;

                // TODO: Place order

                // Event
                var orderPlaced = _mapper.Map<OrderPlaced>(orderSubmitted);

                // Publish event
                await _publishEndpoint.Publish(orderPlaced);

                // Response
                await context.RespondAsync(orderPlaced);

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(OrderPlaced), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
            }
            catch(Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}