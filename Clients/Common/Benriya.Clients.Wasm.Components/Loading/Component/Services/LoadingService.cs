using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Benriya.Clients.Wasm.Components.Loading.Component.Services
{
    public class LoadingService : ILoadingService
    {
        public DateTime Timer { get; private set; }
        public LoadingComponentState LoadingState { get; private set; } = LoadingComponentState.Hide;
        private Timer Count { get; set; } = null;
        
        public event Action<DateTime> OnElapsed;
        public event Action<bool> OnStop;
     
        public void SetLoading(bool isLoading)
        {
            if (isLoading)
                this.Show();
            else
                this.Hide();

        }

        public void Show()
        {
            if (Count != null) return;
            var dateNow = DateTime.Now;
            Timer = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 0, 0, 0, 0);
            LoadingState = LoadingComponentState.Show;
            Count = new Timer();
            Count.Interval = 10;
            Count.AutoReset = true;
            Count.Enabled = true;
            Count.Elapsed += NotifyTimerElapsed;
        }
        public void Hide()
        {
            if (Count != null)
            {
                Count.Stop();
                Count.Dispose();
                Count = null;
            }   
            LoadingState = LoadingComponentState.Hide;          
            var dateNow = DateTime.Now;
            Timer = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 0, 0, 0, 0);
            OnStop?.Invoke(true);           
        }

        private void NotifyTimerElapsed(Object source, ElapsedEventArgs e)
        {            
            if(LoadingState == LoadingComponentState.Show)
                Timer = Timer.AddMilliseconds(10);            
            OnElapsed?.Invoke(Timer);
        }

    }
}