using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface ITPLService
    {
        Task<IEnumerable<TOutput>> ConcurrentAsync<TInput, TOutput>(Func<TInput, TOutput> action, IEnumerable<TInput> inputs, int concurrency,
                                                                    CancellationToken token = default(CancellationToken));
        Task<IEnumerable<TOutput>> ConcurrentAsync<TInput, TOutput>(Func<TInput, Task<TOutput>> action, IEnumerable<TInput> inputs, int concurrency,
                                                                    CancellationToken token = default(CancellationToken));
    }
}
