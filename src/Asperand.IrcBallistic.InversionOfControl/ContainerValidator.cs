using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Asperand.IrcBallistic.InversionOfControl.Abstracts;
using Asperand.IrcBallistic.InversionOfControl.Extenstions;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.InversionOfControl
{
    public class ContainerValidator
    {
        private readonly IEnumerable<DependencyAnalyzer> _dependencyManagers;
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;

        public ContainerValidator(
            IEnumerable<DependencyAnalyzer> dependencyManagers,
            IServiceCollection services,
            IServiceProvider provider)
        {
            _dependencyManagers = dependencyManagers.Where(x => x.IsConfigured);
            _services = services;
            _serviceProvider = provider;
        }

        public bool Validate(string reportPath)
        {
            if (!_dependencyManagers.Any())
            {
                return true;
            }

            var errors = GetValidationResults().ToList();

            if (errors.Any())
            {
                File.WriteAllLines(reportPath, errors);
                return false;
            }

            return true;
        }
        
        //todo: deal with duplicates
        private IEnumerable<string> GetValidationResults()
        {
            var errors = new Dictionary<Type, List<string>>();
            var outOfScopeUnregisteredErrors = new Dictionary<Type, List<(Type unit, string error)>>();

            foreach (var unit in _dependencyManagers)
            {
                var unitType = unit.GetType();
                var analysis = unit.GetAnalysis(_serviceProvider);

                foreach (var dependency in analysis.OutOfScopeUnregisteredTypes)
                {
                    var error = (unitType, FormatUnitDependencyError("Error: Missing external dependency", unitType, dependency));
                    outOfScopeUnregisteredErrors.AddCreate(dependency, error);
                }

                foreach (var dependency in analysis.InScopeUnregisteredTypes)
                {
                    if (_services.All(x => x.ServiceType != dependency))
                    {
                        errors.AddCreate(unitType, FormatUnitDependencyError("Error: Missing internal dependency", unitType, dependency));
                    }

                    //We should always prefer in scope errors
                    outOfScopeUnregisteredErrors.TryRemove(dependency);
                }

                foreach (var dependency in analysis.OutOfScopeRegisteredTypes)
                {
                    errors.AddCreate(unitType, FormatUnitDependencyError("Warn: Out of scope dependency registered", unitType, dependency));
                }

                foreach (var dependency in analysis.InScopeUnusedRegisteredTypes)
                {
                    errors.AddCreate(unitType, FormatUnitDependencyError("Warn: In scope registered dependency not used in scope", unitType, dependency));
                }
                
                foreach (var dependency in analysis.OutOfScopeUnusedRegisteredTypes)
                {
                    errors.AddCreate(unitType, FormatUnitDependencyError("Error: Out of scope registered dependency not used in scope", unitType, dependency));
                }
            }

            //get these out of the temp dictionary and flatten
            foreach (var unitError in outOfScopeUnregisteredErrors.FlattenToList())
            {
                errors.AddCreate(unitError.unit, unitError.error);
            }
            
            //flatten again
            return errors.FlattenToList();
        }
        
        private string FormatUnitDependencyError(string error, Type unit, Type dependency) => $"{error}\n\tUnit: {unit}\n\tDependency: {dependency}";
    }
}