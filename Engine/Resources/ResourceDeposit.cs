using System.Collections.Generic;
using System.Collections.Immutable;

namespace AntEngine.Resources
{
    /// <summary>
    /// Represents the association Resource => amount.
    /// </summary>
    public class ResourceDeposit
    {
        private Dictionary<Resource, int> _ressources;
        
        public IDictionary<Resource, int> All => _ressources.ToImmutableDictionary();

        /// <summary>
        /// Adds a specific amount of resources to the deposit. 
        /// </summary>
        /// If the resource is already stored in the deposit, the amount will be added to the already existing entry.
        /// <param name="resource">Resource to add</param>
        /// <param name="amount">How much resource we want to add</param>
        public void AddResource(Resource resource, int amount)
        {
            if (_ressources.ContainsKey(resource))
            {
                _ressources[resource] += amount;
            }
            else
            {
                _ressources.Add(resource, amount);
            }
        }

        /// <summary>
        /// Removes a specific amount of resources from the deposit.
        /// </summary>
        /// If the resource is depleted, the entry will be removed.
        /// <param name="resource">Resource to remove</param>
        /// <param name="amount">How much resource we want to remove</param>
        public void RemoveResource(Resource resource, int amount)
        {
            if (!_ressources.ContainsKey(resource)) return;
            
            _ressources[resource] -= amount;
            if (_ressources[resource] <= 0)
            {
                _ressources.Remove(resource);
            }
        }
    }
}