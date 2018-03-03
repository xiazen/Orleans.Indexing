﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Concurrency;

namespace Orleans.Indexing
{
    public static class IndexExtensions
    {
        /// <summary>
        /// An extension method to intercept the calls to DirectApplyIndexUpdateBatch
        /// on an Index
        /// </summary>
        public static Task<bool> ApplyIndexUpdateBatch(this IndexInterface index, Immutable<IDictionary<IIndexableGrain, IList<IMemberUpdate>>> iUpdates, bool isUniqueIndex, IndexMetaData idxMetaData, SiloAddress siloAddress = null)
        {
            if (index is ActiveHashIndexPartitionedPerSilo)
            {
                ActiveHashIndexPartitionedPerSiloBucket bucketInCurrentSilo = InsideRuntimeClient.Current.InternalGrainFactory.GetSystemTarget<ActiveHashIndexPartitionedPerSiloBucket>(
                    GetAHashIndexPartitionedPerSiloGrainID(IndexUtils.GetIndexNameFromIndexGrain((IAddressable)index), index.GetType().GetGenericArguments()[1]),
                    siloAddress
                );
                return bucketInCurrentSilo.DirectApplyIndexUpdateBatch(iUpdates, isUniqueIndex, idxMetaData/*, siloAddress*/);
            }
            return index.DirectApplyIndexUpdateBatch(iUpdates, isUniqueIndex, idxMetaData, siloAddress);
        }

        /// <summary>
        /// An extension method to intercept the calls to DirectApplyIndexUpdate
        /// on an Index
        /// </summary>
        internal static Task<bool> ApplyIndexUpdate(this IndexInterface index, IIndexableGrain updatedGrain, Immutable<IMemberUpdate> update, bool isUniqueIndex, IndexMetaData idxMetaData, SiloAddress siloAddress = null)
        {
            if (index is ActiveHashIndexPartitionedPerSilo)
            {
                ActiveHashIndexPartitionedPerSiloBucket bucketInCurrentSilo = InsideRuntimeClient.Current.InternalGrainFactory.GetSystemTarget<ActiveHashIndexPartitionedPerSiloBucket>(
                    GetAHashIndexPartitionedPerSiloGrainID(IndexUtils.GetIndexNameFromIndexGrain((IAddressable )index), index.GetType().GetGenericArguments()[1]),
                    siloAddress
                );
                return bucketInCurrentSilo.DirectApplyIndexUpdate(updatedGrain, update, isUniqueIndex, idxMetaData/*, siloAddress*/);
            }
            return index.DirectApplyIndexUpdate(updatedGrain, update, isUniqueIndex, idxMetaData, siloAddress);
        }

        
        private static GrainId GetAHashIndexPartitionedPerSiloGrainID(string indexName, Type iGrainType)
        {
            return GrainId.GetSystemTargetGrainId(Constants.HASH_INDEX_PARTITIONED_PER_SILO_BUCKET_SYSTEM_TARGET_TYPE_CODE,
                                               IndexUtils.GetIndexGrainID(iGrainType, indexName));
        }
    }
}
