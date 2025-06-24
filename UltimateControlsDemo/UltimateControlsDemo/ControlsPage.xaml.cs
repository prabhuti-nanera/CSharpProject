namespace UltimateControlsDemo
{
    public partial class ControlsPage : ContentPage
    {
        public ControlsPage()
        {
            InitializeComponent();
            this.FadeTo(1, 500);
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            CustomProgress.Progress = e.NewValue;
        }
    }
}