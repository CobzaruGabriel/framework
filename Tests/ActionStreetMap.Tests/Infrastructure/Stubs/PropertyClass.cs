﻿using ActionStreetMap.Infrastructure.Dependencies;

namespace ActionStreetMap.Tests.Infrastructure.Stubs
{
    public interface ITestInterface
    {

    }

    public class TestInterface : ITestInterface
    {
        
    }

    public interface IPropertyClass
    {
    }

    public class PropertyClass : IPropertyClass
    {
        [Dependency]
        public ITestInterface Test { get; set; }
    }
}
