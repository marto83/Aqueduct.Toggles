using System;

namespace Aqueduct.Toggles
{
    [AttributeUsage(AttributeTargets.All)]
    public class FeatureCleanupAttribute : Attribute
    {
        public string Comment;
        public string Name;

        public FeatureCleanupAttribute(string name)
        {
            Name = name;
            Comment = string.Empty;
        }
    }
}