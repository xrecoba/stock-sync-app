using System;
using System.Collections.Generic;
using System.Text;
using Stock.Sync.Domain.Execution;
using Xunit;

namespace Stock.Sync.Tests.Unit.Execution
{
    public class InputActionsFromFileFactoryTests
    {
        [Fact]
        public void Test()
        {
            var lines = new InputActionsFromFileFactory().LoadJson();
        }
    }
}
