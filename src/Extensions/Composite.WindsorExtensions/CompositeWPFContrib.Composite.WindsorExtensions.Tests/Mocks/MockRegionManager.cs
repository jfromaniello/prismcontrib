using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Regions;

namespace CompositeWPFContrib.Composite.WindsorExtensions.Tests.Mocks
{
    public class MockRegionManager : IRegionManager
    {
		public void AttachNewRegion(object regionTarget, string regionName)
        {
            throw new NotImplementedException();
        }

        public IRegionManager CreateRegionManager()
        {
            throw new NotImplementedException();
        }

    	public IRegionCollection Regions
    	{
    		get { throw new NotImplementedException(); }
    	}
    }
}
