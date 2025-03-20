using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Controls;


namespace Pendu
{
    public partial class JeuPage : ContentPage
    {
        private string mots_a_deviner;
        private char[] essaye;
        private int max = 10;
        private int erreur;
        private List<char> choix_incorrects;

        public JeuPage()
        {   NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            Initialisation();
        }

        private async void Initialisation()
        {
            mots_a_deviner = await Mots_aleatoire(); 
            essaye = new char[mots_a_deviner.Length];
            erreur = 0;
            choix_incorrects = new List<char>();
            Mise_a_jour();
        }

        private void Mise_a_jour()
        {
            string affichage = "";
            foreach (char e in essaye)
            {
                affichage += (e == '\0') ? "_ " : (e);
            }
            reponseLabel.Text = affichage;

            CompteurLabel.Text = $"Vie : {max - erreur} ❤️";
            
            choixLabel.Text = $"Lettres incorrectes : {string.Join(", ", choix_incorrects)}";

            string pendu = PenduDessin(erreur);
            penduLabel.Text = pendu;
        }

        private async void BoutonDeviner(object sender, EventArgs e)
        {
            string lettre = Entre.Text?.Trim().ToUpper() ?? "";

             if (lettre.Length != 1 || (!char.IsLetter(lettre[0]) && lettre[0] != '\'' && lettre[0] != '?'))
            {
                await DisplayAlert("Erreur", "Veuillez entrer une seule lettre, un apostrophe ou un point d'interrogation.", "OK");
            }
            else
            {
    
                if (mots_a_deviner.Contains(lettre[0]))
                {
                    Mise_a_jour_essaye(lettre[0]);
                    Mise_a_jour();

                    if (Mots_Trouvee())
                    {
                        await Navigation.PushAsync(new GagnerPage(mots_a_deviner));
                    }
                }
                else
                {
                    erreur++;
                    choix_incorrects.Add(lettre[0]);
                    Mise_a_jour();

                    if (erreur == max)
                    {
                        await Navigation.PushAsync(new PerduPage(mots_a_deviner));
                    }
                }
                
            }

            Entre.Text = "";
        }

        private void Mise_a_jour_essaye(char lettre)
        {
            for (int i = 0; i < mots_a_deviner.Length; i++)
            {
                if (mots_a_deviner[i] == lettre)
                {
                    essaye[i] = lettre;
                }
            }
        }

        private bool Mots_Trouvee()
        {
            for (int i = 0; i < mots_a_deviner.Length; i++)
            {
                if (essaye[i] == '\0')
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<string> Mots_aleatoire()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiMTE1Njg1NDM1ODkxOTYzMDg4OCIsImxpbWl0IjoxMDAsImtleSI6Ik5WN3pCNTNkMUpZUFhPR3JqdDE3dFdDdzIzZUxQVkdrODZwOVdCR3pNakJENWdBYzZ1IiwiY3JlYXRlZF9hdCI6IjIwMjUtMDMtMTRUMTk6MjQ6NTMrMDA6MDAiLCJpYXQiOjE3NDE5ODAyOTN9.CVBR8Vae7WKgCQspejgaKkb53jQNeWoai1Ni8b5K6ls");

                    HttpResponseMessage reponse = await client.GetAsync("https://www.blagues-api.fr/api/random");

                    if (reponse.IsSuccessStatusCode)
                    {
                        string reponseContent = await reponse.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(reponseContent);

                        string joke = json["joke"]?.ToString() ?? "ERREUR";
                        string[] words = joke.Split(' ');
                        string randomWord = words[new Random().Next(words.Length)];

                        return randomWord.ToUpper();
                    }
                    else
                    {
                        return "ERREUR";
                    }
                }
                catch
                {
                    return "ERREUR";
                }
            }
        }
        private string PenduDessin(int erreur)
        {
            switch (erreur)
            {
                case 0:
                    return "\n\n\n\n\n\n";
                case 1:
                    return " \n \n \n \n __|________";
                case 2:
                    return "    | \n    |  \n    |     \n    |    \n __|________";
                case 3:
                    return "    | / \n    |  \n    |     \n    |    \n __|________";
                case 4:
                    return "    _______\n    | / \n    | \n    |     \n    |    \n __|________";
                case 5:
                    return "    _______\n    | /    |\n    |  \n    |     \n    |   \n __|________";
                case 6:
                    return "    _______\n    | /    |\n    |      O\n    |     \n    |    \n __|________";
                case 7:
                    return "    _______\n    | /    |\n    |      O\n    |      |\n    |    \n __|________";
                case 8:
                    return "    _______\n    | /    |\n    |      O\n    |      |\n    |     / \\\n __|________";
                case 9:
                    return "    _______\n    | /    |\n    |      O\n    |      |\\\n    |     / \\\n __|________";
                case 10:
                    return "    _______\n    | /    |\n    |      O\n    |    /|\\\n    |     / \\\n __|________";
                default:
                    return "";
            }
        }

    }
}
