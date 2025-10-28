namespace SEMANA2.Views;

public partial class LoginPage : ContentPage
{
    
    private readonly string[] user = { "Carlos", "Ana", "Jose" };
    private readonly string[] pass = { "carlos123", "ana123", "jose123" };

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void BtnIngresar_Clicked(object sender, EventArgs e)
    {
        var u = (UsuarioEntry.Text ?? "").Trim();
        var p = (PasswordEntry.Text ?? "").Trim();

        if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
        {
            await DisplayAlert("Datos faltantes", "Ingrese usuario y contraseña.", "OK");
            return;
        }

        // Buscar usuario en el vector
        int idx = Array.IndexOf(user, u);
        if (idx >= 0 && pass[idx] == p)
        {
            // Ir a MainPage pasando el nombre de usuario
            await Navigation.PushAsync(new MainPage(u));
        }
        else
        {
            await DisplayAlert("Acceso denegado", "Usuario o contraseña incorrectos.", "OK");
        }
    }
}
