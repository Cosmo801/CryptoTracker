using System;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTracker.Data.Helpers
{
    public class ContinuousTaskFactory
    {
        /// <summary>
        /// A task is passed in that needs to continuously operate.
        /// </summary>
        /// 
        //Rework this
        public ContinuousTaskFactory()
        {
            Initialize();
        }

        protected void Initialize()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Dispose();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            IsRunning = false;
        }

        public async Task CreateTask(Action task, int delay, Action callback)
        {
            if (IsRunning) return;

            try
            {
                while (true)
                {
                    if (_cancellationToken.IsCancellationRequested) _cancellationToken.ThrowIfCancellationRequested();


                    var primaryTask = Task.Run(task);
                    var callbackTask = Task.Run(callback);

                    await Task.Delay(delay).ConfigureAwait(false);
                    await primaryTask.ConfigureAwait(false);
                    await callbackTask.ConfigureAwait(false);

                    OnTaskComplete();

                    

                }
            }
            catch (OperationCanceledException)
            {

                Initialize();
                return;
            }

            catch (Exception)
            {
                Initialize();
                return;
            }





            
        }

        private void OnTaskComplete()
        {
            TaskCompleted(this, new EventArgs());
        }

        public event Action<object, EventArgs> TaskCompleted;

        public bool IsRunning { get; private set; }

        protected CancellationTokenSource _cancellationTokenSource;
        protected CancellationToken _cancellationToken;

        public void EndTask()
        {
            if (!IsRunning || _cancellationTokenSource == null) return;

            _cancellationTokenSource.Cancel();


        }

        

    }

   



   



}
