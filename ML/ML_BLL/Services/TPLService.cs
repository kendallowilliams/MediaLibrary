using MediaLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MediaLibraryBLL.Services
{
    [Export(typeof(ITPLService))]
    public class TPLService : ITPLService
    {
        [ImportingConstructor]
        public TPLService()
        {

        }

        public async Task<IEnumerable<TOutput>> ConcurrentAsync<TInput, TOutput>(Func<TInput, Task<TOutput>> action, IEnumerable<TInput> inputs, int concurrency, 
                                                                                 CancellationToken token = default(CancellationToken))
        {
            IEnumerable<TOutput> output = Enumerable.Empty<TOutput>();
            ExecutionDataflowBlockOptions transformOptions = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = concurrency,
                CancellationToken = token
            };
            TransformBlock<TInput, TOutput> transformBlock = new TransformBlock<TInput, TOutput>(action, transformOptions);
            DataflowBlockOptions bufferOptions = new DataflowBlockOptions()
            {
                CancellationToken = token
            };
            BufferBlock<TOutput> bufferBlock = new BufferBlock<TOutput>(bufferOptions);

            using (transformBlock.LinkTo(bufferBlock))
            {
                foreach (TInput input in inputs) { await transformBlock.SendAsync(input, token); }
                transformBlock.Complete();
                await transformBlock.Completion;
                output = bufferBlock.TryReceiveAll(out IList<TOutput> items) ? items : Enumerable.Empty<TOutput>();
            }

            return output;
        }

        public async Task<IEnumerable<TOutput>> ConcurrentAsync<TInput, TOutput>(Func<TInput, TOutput> action, IEnumerable<TInput> inputs, int concurrency,
                                                                                 CancellationToken token = default(CancellationToken))
        {
            IEnumerable<TOutput> output = Enumerable.Empty<TOutput>();
            ExecutionDataflowBlockOptions transformOptions = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = concurrency,
                CancellationToken = token
            };
            TransformBlock<TInput, TOutput> transformBlock = new TransformBlock<TInput, TOutput>(action, transformOptions);
            DataflowBlockOptions bufferOptions = new DataflowBlockOptions()
            {
                CancellationToken = token
            };
            BufferBlock<TOutput> bufferBlock = new BufferBlock<TOutput>(bufferOptions);

            using (transformBlock.LinkTo(bufferBlock))
            {
                foreach (TInput input in inputs) { await transformBlock.SendAsync(input, token); }
                transformBlock.Complete();
                await transformBlock.Completion;
                output = bufferBlock.TryReceiveAll(out IList<TOutput> items) ? items : Enumerable.Empty<TOutput>();
            }

            return output;
        }
    }
}
