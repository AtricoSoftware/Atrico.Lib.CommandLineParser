using System;
using Atrico.Lib.Common;
using Moq;

// ReSharper disable once CheckNamespace

namespace Atrico.Lib.Testing
{
    public abstract class TestFixtureBase2 : TestFixtureBase
    {
        private readonly Lazy<IRunContextInfo> _runContextInfo = new Lazy<IRunContextInfo>(CreateRunContextInfo);

        protected IRunContextInfo MockRunContext
        {
            get { return _runContextInfo.Value; }
        }

        private static IRunContextInfo CreateRunContextInfo()
        {
            var mock = new Mock<IRunContextInfo>();
            mock.Setup(rc => rc.EntryAssemblyName).Returns(@"assembly.name");
            mock.Setup(rc => rc.EntryAssemblyVersion).Returns(new Version(1,2,3,4));
            mock.Setup(rc => rc.EntryAssemblyCopyright).Returns(@"copyright");
            mock.Setup(rc => rc.EntryAssemblyPath).Returns(@"C:\test.exe");
            return mock.Object;
        }
    }
}