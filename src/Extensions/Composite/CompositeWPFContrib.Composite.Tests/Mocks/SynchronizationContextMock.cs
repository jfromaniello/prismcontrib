using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Threading;
using System.Diagnostics;

namespace CompositeWPFContrib.Composite.Tests.Mocks
{
    /// <summary>
    /// This synchronization context implementation is based on the ThreadPoolSynchronizer sample
    /// from IDesign Inc, www.idesign.net
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
    public class SynchronizationContextMock : SynchronizationContext, IDisposable
    {
        class WorkerThread
        {
            SynchronizationContextMock m_Context;
            public Thread m_ThreadObj;
            bool m_EndLoop;

            internal WorkerThread(string name, SynchronizationContextMock context)
            {
                m_Context = context;

                m_EndLoop = false;

                m_ThreadObj = new Thread(Run);
                m_ThreadObj.IsBackground = true;
                m_ThreadObj.Name = name;
                m_ThreadObj.Start();
            }
            bool EndLoop
            {
                set
                {
                    lock (this)
                    {
                        m_EndLoop = value;
                    }
                }
                get
                {
                    lock (this)
                    {
                        return m_EndLoop;
                    }
                }
            }

            void Run()
            {
                SynchronizationContext.SetSynchronizationContext(m_Context);

                while (EndLoop == false)
                {
                    WorkItem workItem = m_Context.GetNext();
                    if (workItem != null)
                    {
                        workItem.CallBack();
                    }
                }
            }
            public void Kill()
            {
                //Kill is called on client thread - must use cached thread object
                Debug.Assert(m_ThreadObj != null);
                if (m_ThreadObj.IsAlive == false)
                {
                    return;
                }
                EndLoop = true;

                //Wait for thread to die
                m_ThreadObj.Join();
            }
        }

        WorkerThread m_WorkerThread;
        Queue<WorkItem> m_WorkItemQueue;
        protected Semaphore CallQueued
        { get; private set; }

        public SynchronizationContextMock()
        {
            CallQueued = new Semaphore(0, Int32.MaxValue);
            m_WorkItemQueue = new Queue<WorkItem>();

            m_WorkerThread = new WorkerThread("SynchronizationContextMockThread", this);
        }
        virtual internal void QueueWorkItem(WorkItem workItem)
        {
            lock (m_WorkItemQueue)
            {
                m_WorkItemQueue.Enqueue(workItem);
                CallQueued.Release();
            }
        }
        protected virtual bool QueueEmpty
        {
            get
            {
                lock (m_WorkItemQueue)
                {
                    if (m_WorkItemQueue.Count > 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
        internal virtual WorkItem GetNext()
        {
            CallQueued.WaitOne();
            lock (m_WorkItemQueue)
            {
                if (m_WorkItemQueue.Count == 0)
                {
                    return null;
                }
                return m_WorkItemQueue.Dequeue();
            }
        }
        public void Dispose()
        {
            Close();
        }
        public override SynchronizationContext CreateCopy()
        {
            return this;
        }
        public override void Post(SendOrPostCallback method, object state)
        {
            WorkItem workItem = new WorkItem(method, state);
            QueueWorkItem(workItem);
        }
        public override void Send(SendOrPostCallback method, object state)
        {
            //If already on the correct context, must invoke now to avoid deadlock
            if (SynchronizationContext.Current == this)
            {
                method(state);
                return;
            }
            WorkItem workItem = new WorkItem(method, state);
            QueueWorkItem(workItem);
            workItem.AsyncWaitHandle.WaitOne();
        }
        public void Close()
        {
            if (CallQueued.SafeWaitHandle.IsClosed)
            {
                return;
            }
            CallQueued.Release(Int32.MaxValue);

            m_WorkerThread.Kill();
            CallQueued.Close();
        }
        public void Abort()
        {
            CallQueued.Release(Int32.MaxValue);

            m_WorkerThread.m_ThreadObj.Abort();
            CallQueued.Close();
        }
    }

    [Serializable]
    internal class WorkItem
    {
        object m_State;
        SendOrPostCallback m_Method;
        ManualResetEvent m_AsyncWaitHandle;

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return m_AsyncWaitHandle;
            }
        }

        internal WorkItem(SendOrPostCallback method, object state)
        {
            m_Method = method;
            m_State = state;
            m_AsyncWaitHandle = new ManualResetEvent(false);
        }

        //This method is called on the worker thread to execute the method
        internal void CallBack()
        {
            m_Method(m_State);
            m_AsyncWaitHandle.Set();
        }
    }

}
