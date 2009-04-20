namespace CompositeWPFContrib.Composite.Events
{
    /// <summary>
    /// Specifies on which thread a <see cref="CompositeSynchronizationContextEvent{TPayload}"/> subscriber will be called.
    /// </summary>
    public enum ThreadOption
    {
        /// <summary>
        /// The call is done on the same thread on which the <see cref="CompositeSynchronizationContextEvent{TPayload}"/> was published.
        /// </summary>
        PublisherThread,

        /// <summary>
        /// The call is done on the thread the subscriber subscribes from, if a SynchronizationContext is present indicating thread affinity. Otherwise it fires on  the publisher's thread.
        /// </summary>
        SubscriberAffinityThread,

        /// <summary>
        /// The call is done asynchronously on a background thread.
        /// </summary>
        BackgroundThread
    }

}
