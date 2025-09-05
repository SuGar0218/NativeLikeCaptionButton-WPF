using System;

namespace SuGarToolkit.WPF.SourceGenerators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RelayDependencyPropertyAttribute : Attribute
    {
        public RelayDependencyPropertyAttribute(string TargetPropertyPath)
        {
            _path = TargetPropertyPath;
        }

        public object DefaultValue { get; set; }

        public string DefaultValuePath { get; set; }

        private readonly string _path;
    }
}