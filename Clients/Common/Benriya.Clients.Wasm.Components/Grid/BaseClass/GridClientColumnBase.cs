using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benriya.Clients.Wasm.Components.Grid
{
    public class GridClientColumnBase : ComponentBase
    {
        [Parameter]
        public string ColumnTitle { get; set; }

        [Parameter]
        public string Param { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnSearchTextChanged { get; set; }
        [Parameter]
        public bool SearchText { get; set; } = true;
        [Parameter]
        public string InputType { get; set; } = "text";
    }
}
