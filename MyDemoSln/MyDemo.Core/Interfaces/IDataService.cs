using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.WindowsAzure.MobileServices;

namespace MyDemo.Core
{
    public interface IDataService
    {
        MobileServiceClient AMSClient { get; }

        Task<List<T>> QueryDataAsync<T>(Expression<Func<T,bool>> predicate,string queryId, bool syncCloud);

        Task<T> UpdateDataAsync<T>(T updateData, string queryId, bool syncCloud);

        Task<T> InsertDataAsync<T>(T insertData,bool syncCloud);

        Task DeleteDataAsync<T>(T deleteData,string queryId,bool syncCloud);

        Task PushToCloud(CancellationToken callectionToken);

        Task PushToCloud();
    }
}

