﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Stock.Sync.Domain.Execution;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Stock.Sync.Tests.Unit.StockEventFactory
{
    public class StockEventFactoryTests
    {
        private static int _parentId = 10;
        private static int _childId = 2;

        private Engine _engine = new Engine();
        private InputLine _createParent = new InputLine(Constants.ProductCreated, _parentId, 20, 140, "None");
        private InputLine _createChild = new InputLine(Constants.ProductCreated, _childId, 15, 140, _parentId.ToString());

        // Skip the tests to check if a field is missing as the library to read the json is already taking care of it.

        [Fact]
        public void WithInvalidOperationType_ErrorIsIgnored()
        {
            var productsRepository = new ProductsRepository();

            Mock<ILogger> moq = new Mock<ILogger>();
            var invalidType = "invalid";

            Mock<ILogger> errors = new Mock<ILogger>();
            var stockEventFactory = new InputLinesToIStockEventsFactory(productsRepository, errors.Object);
            var lines = new List<InputLine>
            {
                new InputLine(invalidType, 1, 1, 1, "whatever")
            };

            var input = stockEventFactory.GetInputEvents(lines);

            Assert.Equal(0 , input.Count());
            errors.Verify(l => l.LogMessage($"Unknown action type {invalidType}"), Times.Never);
        }

        [Fact]
        public void WithRightData_CanCreateParentItem()
        {
            var productsRepository = new ProductsRepository();
            var stockEventFactory = new InputLinesToIStockEventsFactory(productsRepository);
            var lines = new List<InputLine>
            {
                _createParent
            };
            var events = stockEventFactory.GetInputEvents(lines);

            _engine.Run(productsRepository, events);

            Assert.Equal(events.Count(), 1);
            Assert.Equal(20, productsRepository.GetProduct(_parentId).Stock);
        }

        [Fact]
        public void WithRightData_CanCreateChildItem()
        {
            var productsRepository = new ProductsRepository();
            var stockEventFactory = new InputLinesToIStockEventsFactory(productsRepository);
            var lines = new List<InputLine>
            {
                _createParent,
                _createChild
            };
            var events = stockEventFactory.GetInputEvents(lines);

            _engine.Run(productsRepository, events);

            Assert.Equal(15, productsRepository.GetProduct(_childId).Stock);
        }


        [Fact]
        public void WithNoParent_FailsToCreateChildItem()
        {
            var productsRepository = new ProductsRepository();
            var stockEventFactory = new InputLinesToIStockEventsFactory(productsRepository);
            var lines = new List<InputLine>
            {
                _createChild
            };
            var events = stockEventFactory.GetInputEvents(lines);

            Mock<ILogger> errors = new Mock<ILogger>();
            Mock<ILogger> output = new Mock<ILogger>();

            var engine = new Engine(errors.Object, output.Object);
            engine.Run(productsRepository, events);

            errors.Verify(l => l.LogMessage($"Unexisting parentId {_parentId}"), Times.Once);
        }

        [Fact]
        public void AfterAddingItem_StockCanBeUpdated()
        {
            var productsRepository = new ProductsRepository();
            var stockEventFactory = new InputLinesToIStockEventsFactory(productsRepository);
            var lines = new List<InputLine>
            {
                _createParent,
                new InputLine(Constants.ProductUpdated, _parentId, 99, 0, "")
            };
            var events = stockEventFactory.GetInputEvents(lines);

            _engine.Run(productsRepository, events);

            Assert.Equal(99, productsRepository.GetProduct(_parentId).Stock);
        }

        [Fact]
        public void UpdatingUnexistingItem_ThrowsException()
        {
            var productsRepository = new ProductsRepository();
            var stockEventFactory = new InputLinesToIStockEventsFactory(productsRepository);
            var lines = new List<InputLine>
            {
                new InputLine(Constants.ProductUpdated, _parentId, 99, 0, "")
            };
            var events = stockEventFactory.GetInputEvents(lines);

            Mock<ILogger> errors = new Mock<ILogger>();
            Mock<ILogger> output = new Mock<ILogger>();

            var engine = new Engine(errors.Object, output.Object);
            engine.Run(productsRepository, events);

            errors.Verify(l => l.LogMessage($"Unexisting product {_parentId}"), Times.Once);
        }

        [Fact]
        public void EndingUnexistingItem_ThrowsException()
        {
            var productsRepository = new ProductsRepository();
            var stockEventFactory = new InputLinesToIStockEventsFactory(productsRepository);
            var lines = new List<InputLine>
            {
                new InputLine(Constants.ProductEnded, _parentId, 99, 0, "")
            };
            var events = stockEventFactory.GetInputEvents(lines);

            Mock<ILogger> errors = new Mock<ILogger>();
            Mock<ILogger> output = new Mock<ILogger>();

            var engine = new Engine(errors.Object, output.Object);
            engine.Run(productsRepository, events);

            errors.Verify(l => l.LogMessage($"Unexisting product {_parentId}"), Times.Once);
        }

        [Fact]
        public void AfterAddingItem_ItCanBeEnded()
        {
            var productsRepository = new ProductsRepository();
            var stockEventFactory = new InputLinesToIStockEventsFactory(productsRepository);
            var lines = new List<InputLine>
            {
                _createParent,
                new InputLine(Constants.ProductEnded, _parentId, 0, 0, "")

            };
            var events = stockEventFactory.GetInputEvents(lines);
            _engine.Run(productsRepository, events);

            Assert.Equal(true, productsRepository.GetProduct(_parentId).IsEnded);
        }
    }
}
