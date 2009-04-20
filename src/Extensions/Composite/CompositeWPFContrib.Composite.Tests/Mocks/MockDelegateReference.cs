using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Events;

namespace CompositeWPFContrib.Composite.Tests.Mocks
{
    class MockDelegateReference : IDelegateReference
    {
        public Delegate Target { get; set; }

        public MockDelegateReference()
        {

        }

        public MockDelegateReference(Delegate target)
        {
            Target = target;
        }
    }

}
