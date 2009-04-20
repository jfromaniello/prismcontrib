using System;
using DSASample.Infrastructure.Events;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Events;
using Microsoft.Practices.SmartClient.DisconnectedAgent;

namespace DSASample
{
    public class ShellPresenter
    {
        private IConnectionMonitor _connectionMonitor;

        public ShellPresenter(IShell shell, IEventAggregator eventAggregator, IConnectionMonitor connectionMonitor)
        {
            Shell = shell;

            StatusUpdateEvent statusUpdateEvent = eventAggregator.GetEvent<StatusUpdateEvent>();
            statusUpdateEvent.Subscribe(StatusUpdate, ThreadOption.UIThread, true);

            ProcessingEvent processingEvent = eventAggregator.GetEvent<ProcessingEvent>();
            processingEvent.Subscribe(ShowProcessing, ThreadOption.UIThread, true);

            // Monitor network status changes.
            _connectionMonitor = connectionMonitor;
            _connectionMonitor.ConnectionStatusChanged += new EventHandler(OnConnectionStatusChanged);

            // Show current connectivity state.
            UpdateConnectivityStatusLabel(_connectionMonitor.IsConnected);
        }

        public IShell Shell { get; private set; }

        private void StatusUpdate(string message)
        {
            Shell.UpdateStatusMessage(message);
        }

        private void ShowProcessing(int seconds)
        {
            Shell.ShowProcessing(seconds);
        }

        private void OnConnectionStatusChanged(object sender, EventArgs e)
        {
            UpdateConnectivityStatusLabel(_connectionMonitor.IsConnected);
        }

        private void UpdateConnectivityStatusLabel(bool connected)
        {
            string message = connected ? "Online" : "Offline";
            Shell.UpdateStatusMessage(message);
        }
    }
}