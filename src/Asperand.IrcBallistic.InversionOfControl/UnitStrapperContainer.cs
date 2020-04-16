using System;
using System.Collections.Generic;
using System.Linq;
using Asperand.IrcBallistic.InversionOfControl.Extenstions;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.InversionOfControl
{
    internal class UnitStrapperContainer : List<ServiceDescriptor>, IServiceCollection
    {
        private readonly string _baseNamespace;

        public UnitStrapperContainer(string baseNamespace)
        {
            if (string.IsNullOrWhiteSpace(baseNamespace))
                throw new InvalidOperationException("UnitStrappers _need_ to have a namespace!");
            _baseNamespace = baseNamespace;
        }

        public RegistrationAnalysis AnalyzeRegistrations(IServiceProvider provider)
        {
            var constructorParameters = GetUniqueConstructorParameterTypes().ToList();
            var registeredButNotInScopeConstructor = GetUnusedInScopeRegistrations(constructorParameters).ToList();
            var usedTypesRegisteredAndUnregistered = constructorParameters.SplitBy(t => this.Any(s => s.ServiceType == t));
            var registeredTypesInAndOutOfScope = usedTypesRegisteredAndUnregistered.True.SplitBy(IsInScope);
            var unregisteredTypesInAndOutOfScope = usedTypesRegisteredAndUnregistered.False.SplitBy(IsInScope);
            var unusedInScopeRegisteredTypesInAndOutOfScope = registeredButNotInScopeConstructor.SplitBy(IsInScope);

            return new RegistrationAnalysis
            {
                BaseNamespace = _baseNamespace,
                InScopeRegisteredTypes = registeredTypesInAndOutOfScope.True.ToList(),
                OutOfScopeRegisteredTypes = registeredTypesInAndOutOfScope.False.ToList(),
                InScopeUnregisteredTypes = unregisteredTypesInAndOutOfScope.True.ToList(),
                OutOfScopeUnregisteredTypes = unregisteredTypesInAndOutOfScope.False.Where(t=> !provider.IsRegistered(t)).ToList(),
                InScopeUnusedRegisteredTypes = unusedInScopeRegisteredTypesInAndOutOfScope.True.ToList(),
                OutOfScopeUnusedRegisteredTypes = unusedInScopeRegisteredTypesInAndOutOfScope.False.ToList()
            };
        }

        private IEnumerable<Type> GetConstructorParameterTypes(Type type) => type
            .GetConstructors()
            .SelectMany(c =>
                c.GetParameters()
                    .Select(p => p.ParameterType));

        private IEnumerable<Type> GetUnusedInScopeRegistrations(List<Type> usedParams) =>
            this.Select(s => s.ServiceType)
                .Where(t => usedParams.All(p => p != t));

        private IEnumerable<Type> GetUniqueConstructorParameterTypes()
        {
            var types = new HashSet<Type>();
            foreach (var service in this)
            {
                var constructorParameters = GetConstructorParameterTypes(service.ImplementationType??service.ServiceType);
                foreach (var parameter in constructorParameters)
                {
                    types.Add(parameter);
                }
            }
            return types;
        }

        private bool IsInScope(Type type)
        {
            var targetNamespace = type.Namespace ?? "";
            return targetNamespace.StartsWith(_baseNamespace);
        }
    }
}