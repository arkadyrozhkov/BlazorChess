using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BlazorChess.Client.Components {
    public sealed class ModelObserver : ComponentBase, IDisposable {
        [Parameter]
        public INotifyPropertyChanged Model { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        public override Task SetParametersAsync(ParameterView parameters) {
            Invalidate();
            return base.SetParametersAsync(parameters);
        }
        protected override void OnParametersSet() {
            base.OnParametersSet();
            if(Model != null) {
                Model.PropertyChanged += Model_PropertyChanged;
            }
        }
        private void Model_PropertyChanged(object sender, EventArgs e) {
            StateHasChanged();
        }
        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if(Model != null) {
                builder.AddContent(0, ChildContent);
            }
        }
        private void Invalidate() {
            if(Model != null) {
                Model.PropertyChanged -= Model_PropertyChanged;
                Model = null;
            }
        }
        void IDisposable.Dispose() {
            Invalidate();
        }
    }
}
