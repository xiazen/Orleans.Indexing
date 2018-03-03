﻿using Orleans.Streams;
using System.Linq;
using System.Threading.Tasks;

namespace Orleans.Indexing
{
    /// <summary>
    /// Extension for the built-in <see cref="IOrderedQueryable{T}"/> allowing for Orleans specific operations
    /// </summary>
    public interface IOrleansQueryable<TGrain, TProperties> : /*IOrleansQueryable, */ IOrderedQueryable<TProperties> where TGrain : IIndexableGrain
    {
        /// <summary>
        /// Observes the result of the query for the current IOrleansQueryable
        /// </summary>
        /// <param name="observer">the observer object</param>
        Task ObserveResults(IAsyncBatchObserver<TGrain> observer);

        /// <summary>
        /// gets the result of the query for the current IOrleansQueryable
        /// </summary>
        /// <returns>the whole query result</returns>
        Task<IOrleansQueryResult<TGrain>> GetResults();
    }
}
