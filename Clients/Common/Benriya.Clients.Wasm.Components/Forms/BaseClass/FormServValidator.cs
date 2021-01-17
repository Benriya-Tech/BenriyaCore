using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;

namespace Benriya.Clients.Wasm.Components.Forms
{
    public class FormServValidator : ComponentBase
    {
        private ValidationMessageStore messageStore;
        [CascadingParameter]
        public EditContext CurrentEditContext { get; set; }

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(FormServValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. " +
                    $"For example, you can use {nameof(FormServValidator)} " +
                    $"inside an {nameof(EditForm)}.");
            }
            messageStore = new ValidationMessageStore(CurrentEditContext);
            CurrentEditContext.OnValidationRequested += (s, e) => messageStore.Clear();
            CurrentEditContext.OnFieldChanged += (s, e) => messageStore.Clear(e.FieldIdentifier);
        }

        public void DisplayErrors(Dictionary<string, List<string>> errors)
        {
            foreach (var err in errors)            
                messageStore.Add(CurrentEditContext.Field(err.Key), err.Value);            
            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void ClearErrors()
        {
            messageStore.Clear();
            CurrentEditContext.NotifyValidationStateChanged();
        }
    }
}
