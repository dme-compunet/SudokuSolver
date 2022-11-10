using System;
using System.Windows;

namespace Compunet.SudokuSolver.Application
{
    public class ThemeApplicationResource : IApplicationResource
    {
        private readonly Lazy<ResourceDictionary> mResource;

        public string Name { get; }
        public ApplicationResourceCategory Category { get; }

        public ThemeApplicationResource(string name,
                                        ApplicationResourceCategory category,
                                        Lazy<ResourceDictionary> resource)
        {
            Name = name;
            Category = category;
            mResource = resource;
        }

        public ResourceDictionary CreateResourceDictionary()
        {
            return mResource.Value;
        }
    }
}
