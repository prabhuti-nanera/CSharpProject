//using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Controls;

namespace UltimateControlsDemo
{
    public partial class MainFlyoutPage : FlyoutPage
    {
        public MainFlyoutPage()
        {
            InitializeComponent();
            DetailPage.Navigation.PushAsync(new HomePage());
        }

        private void OnHomeClicked(object sender, System.EventArgs e)
        {
            DetailPage.Navigation.PushAsync(new HomePage());
            IsPresented = false;
        }

        private void OnLayoutsClicked(object sender, System.EventArgs e)
        {
            DetailPage.Navigation.PushAsync(new LayoutsPage());
            IsPresented = false;
        }

        private void OnControlsClicked(object sender, System.EventArgs e)
        {
            DetailPage.Navigation.PushAsync(new ControlsPage());
            IsPresented = false;
        }

        private void OnAlertClicked(object sender, System.EventArgs e)
        {
            DetailPage.Navigation.PushAsync(new AlertPage());
            IsPresented = false;
        }
    }
}