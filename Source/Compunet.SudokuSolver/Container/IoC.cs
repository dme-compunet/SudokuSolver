using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Design;

namespace Compunet.SudokuSolver.Container
{
    public class IoC : ServiceContainer
    {
        private static IoC? mDefault;

        public static IoC Simple => mDefault ?? throw new InvalidOperationException("the 'Simple' container is not builded");

        public IoC(IServiceProvider? parentProvider) : base(parentProvider) { }

        internal static void CreateSimple(IServiceCollection services)
        {
            if (mDefault != null)
                throw new InvalidOperationException();

            mDefault = new IoC(services.BuildServiceProvider());
        }
    }
}
