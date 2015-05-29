using System;
using System.Reflection;

// ReSharper disable once CheckNamespace

namespace Atrico.Lib.Common
{
    /// <summary>
    ///     Information about the current run context
    /// </summary>
    public interface IRunContextInfo
    {
        /// <summary>
        ///     The name of the entry assembly
        /// </summary>
        string EntryAssemblyName { get; }

        /// <summary>
        ///     The version of the entry assembly
        /// </summary>
        Version EntryAssemblyVersion { get; }

        /// <summary>
        ///     The copyright message of the entry assembly
        /// </summary>
        string EntryAssemblyCopyright { get; }

        /// <summary>
        ///     The path of the entry assembly
        /// </summary>
        string EntryAssemblyPath { get; }
    }

    /// <summary>
    ///     Default implementation of Run context info
    /// </summary>
    public class RunContextInfo : IRunContextInfo
    {
        private static readonly Lazy<Assembly> _entryAssembly = new Lazy<Assembly>(Assembly.GetEntryAssembly);
        private static readonly Lazy<string> _entryAssemblyName = new Lazy<string>(() => GetAssemblyName(_entryAssembly.Value));
        private static readonly Lazy<Version> _entryAssemblyVersion = new Lazy<Version>(() => GetAssemblyVersion(_entryAssembly.Value));
        private static readonly Lazy<string> _entryAssemblyCopyright = new Lazy<string>(() => GetAssemblyCopyright(_entryAssembly.Value));
        private static readonly Lazy<string> _entryAssemblyPath = new Lazy<string>(() => GetAssemblyPath(_entryAssembly.Value));

        public string EntryAssemblyName
        {
            get { return _entryAssemblyName.Value; }
        }

        public Version EntryAssemblyVersion
        {
            get { return _entryAssemblyVersion.Value; }
        }

        public string EntryAssemblyCopyright
        {
            get { return _entryAssemblyCopyright.Value; }
        }

        public string EntryAssemblyPath
        {
            get { return _entryAssemblyPath.Value; }
        }

        private static string GetAssemblyName(Assembly assembly)
        {
            return assembly == null ? "Unknown" : assembly.GetName().Name;
        }

        private static Version GetAssemblyVersion(Assembly assembly)
        {
            return assembly == null ? new Version() : assembly.GetName().Version;
        }

        private static string GetAssemblyCopyright(Assembly assembly)
        {
            if (assembly == null) return "Unknown";
            var copyrightAttr = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            return copyrightAttr == null ? "" : copyrightAttr.Copyright;
        }

        private static string GetAssemblyPath(Assembly assembly)
        {
            return assembly == null ? "Unknown" : assembly.Location;
        }
    }
}