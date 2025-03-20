namespace Pendu;

public partial class MainPage : ContentPage
{

	public MainPage()
	{	NavigationPage.SetHasBackButton(this, false);
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new JeuPage());
	}
}

