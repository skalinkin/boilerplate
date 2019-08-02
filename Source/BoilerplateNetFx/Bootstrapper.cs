using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace BoilerplateNetFx
{
    public class Bootstrapper
    {
        private CompositionContainer _compositionContainer;
        private IContainer _container;

        [ImportMany(typeof(IModule))] private IEnumerable<IModule> Modules { get; set; }

        public void BuildComposition()
        {
            Debug.Print("Starting build the composition.");
            var catalog = new AggregateCatalog();
            var entryAssembly = Assembly.GetEntryAssembly();
            var assemblyCatalog = new AssemblyCatalog(entryAssembly);
            catalog.Catalogs.Add(assemblyCatalog);
            Debug.Print($"Added assembly composition catalog for assembly: {entryAssembly.Location}");
            var location = Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(location);
            var directoryCatalog = new DirectoryCatalog(path ?? throw new InvalidOperationException());
            catalog.Catalogs.Add(directoryCatalog);
            Debug.Print($"Added directory composition catalog for path: {path}");
            _compositionContainer = new CompositionContainer(catalog);
            _compositionContainer.ComposeParts(this);
            var builder = new ContainerBuilder();
            foreach (var module in Modules)
            {
                Debug.Print($"Registering module ${module.GetType().Name}");
                builder.RegisterModule(module);
            }

            _container = builder.Build();
            Debug.Print("Container has been built");
        }

        public T GetCompositionRoot<T>()
        {
            var start = _container.Resolve<T>();
            return start;
        }
    }
}
