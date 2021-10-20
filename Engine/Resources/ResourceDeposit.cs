using System.Collections.Generic;
using System.Collections.Immutable;

namespace AntEngine.Resources
{
    /// <summary>
    /// Represents the association Resource => amount.
    /// </summary>
    public class ResourceDeposit
    {
        private Dictionary<Resource, int> _resources;

        public ResourceDeposit()
        {
            _resources = new Dictionary<Resource, int>();
        }
        
        public IDictionary<Resource, int> All => _resources.ToImmutableDictionary();

        /// <summary>
        /// Adds a specific amount of resources to the deposit. 
        /// </summary>
        /// If the resource is already stored in the deposit, the amount will be added to the already existing entry.
        /// <param name="resource">Resource to be added</param>
        /// <param name="amount">Quantity of resource to be added</param>
        public void AddResource(Resource resource, int amount)
        {
            if (_resources.ContainsKey(resource))
            {
                _resources[resource] += amount;
            }
            else
            {
                _resources.Add(resource, amount);
            }
        }

        /// <summary>
        /// Removes a specific amount of resources from the deposit.
        /// </summary>
        /// If the resource is depleted, the entry will be removed.
        /// <param name="resource">Resource to be removed</param>
        /// <param name="amount">Quantity of resource to be removed</param>
        public void RemoveResource(Resource resource, int amount)
        {
            if (!_resources.ContainsKey(resource)) return;
            
            _resources[resource] -= amount;
            if (_resources[resource] <= 0)
            {
                _resources.Remove(resource);
            }
        }
    }
}