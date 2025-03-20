
namespace Pendu
{
    public partial class GagnerPage : ContentPage
    {
        public GagnerPage(string mots)
        {   NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            MotsLabel.Text = "Le mot était : " + mots;
        }

        private async void BoutonRejouer(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new JeuPage()); 
        }

        private async void BoutonQuitter(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage()); 
        }
    }
}



