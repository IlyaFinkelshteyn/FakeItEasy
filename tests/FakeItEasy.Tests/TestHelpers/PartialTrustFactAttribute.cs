namespace FakeItEasy.Tests.TestHelpers
{
    using System;
    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method)]
    [XunitTestCaseDiscoverer("PartialTrustExample.XunitExtensions.PartialTrustFactDiscoverer", "FakeItEasy.Tests")]
    public class PartialTrustFactAttribute : FactAttribute { }
}
