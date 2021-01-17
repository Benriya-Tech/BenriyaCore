using System;

namespace Benriya.Clients.Wasm.Components.Loading.Component.Services
{
    public interface ILoadingService
    {
        LoadingComponentState LoadingState { get; }
        DateTime Timer { get; }
        event Action<DateTime> OnElapsed;
        event Action<bool> OnStop;
        void Hide();
        void Show();
        void SetLoading(bool isLoading);
    }
}