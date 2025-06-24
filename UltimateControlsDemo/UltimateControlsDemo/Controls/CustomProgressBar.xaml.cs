using Microsoft.Maui.Controls;

namespace UltimateControlsDemo.Controls
{
    public partial class CustomProgressBar : ContentView
    {
        public static readonly BindableProperty ProgressProperty =
            BindableProperty.Create(nameof(Progress), typeof(double), typeof(CustomProgressBar), 0.0, propertyChanged: OnProgressChanged);

        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        public CustomProgressBar()
        {
            InitializeComponent();
        }

        private static void OnProgressChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomProgressBar control)
            {
                control.UpdateProgress();
            }
        }

        private void UpdateProgress()
        {
            ProgressFill.WidthRequest = this.Width * Math.Clamp(Progress, 0, 1);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            UpdateProgress();
        }
    }
}