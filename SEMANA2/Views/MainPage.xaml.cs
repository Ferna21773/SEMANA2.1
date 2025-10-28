using System.Globalization;

namespace SEMANA2.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly string _usuario;
        public MainPage(string usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            FechaSeleccionada.Date = DateTime.Today;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Mensaje de bienvenida al abrir la ventana de calificaciones
            await DisplayAlert("Bienvenido", $"Hola, {_usuario}.", "OK");
        }
        private async void BtnCalcular_Clicked(object sender, EventArgs e)
        {
            var errores = new List<string>();

            string alumno = ListaEstudiantes.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(alumno))
                errores.Add("Seleccione un estudiante.");

            if (!ValidarNota(SeguimientoUno.Text, out double seg1)) errores.Add("Seguimiento 1 debe ser numérico (0–10).");
            if (!ValidarNota(ExamenUno.Text, out double ex1)) errores.Add("Examen 1 debe ser numérico (0–10).");
            if (!ValidarNota(SeguimientoDos.Text, out double seg2)) errores.Add("Seguimiento 2 debe ser numérico (0–10).");
            if (!ValidarNota(ExamenDos.Text, out double ex2)) errores.Add("Examen 2 debe ser numérico (0–10).");

            if (errores.Count > 0)
            {
                await DisplayAlert("Datos inválidos", string.Join("\n", errores), "OK");
                return;
            }

            const double pesoSeg = 0.3;
            const double pesoEx = 0.2;

            double notaP1 = Math.Round(seg1 * pesoSeg + ex1 * pesoEx, 2);
            double notaP2 = Math.Round(seg2 * pesoSeg + ex2 * pesoEx, 2);
            double notaTotal = Math.Round(notaP1 + notaP2, 2);

            ResultadoParcialUno.Text = notaP1.ToString("0.00", CultureInfo.InvariantCulture);
            ResultadoParcialDos.Text = notaP2.ToString("0.00", CultureInfo.InvariantCulture);
            ResultadoFinal.Text = notaTotal.ToString("0.00", CultureInfo.InvariantCulture);

            string estadoFinal = CalcularEstado(notaTotal);

            string resumen =
                $"Nombre: {alumno}\n" +
                $"Fecha: {FechaSeleccionada.Date:yyyy-MM-dd}\n\n" +
                $"Nota Parcial 1: {notaP1:0.00}\n" +
                $"Nota Parcial 2: {notaP2:0.00}\n" +
                $"Nota Final: {notaTotal:0.00}\n" +
                $"Estado: {estadoFinal}";

            await DisplayAlert("Resultado", resumen, "OK");
        }

        private bool ValidarNota(string? texto, out double valor)
        {
            if (double.TryParse(texto?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out valor)
                && valor >= 0 && valor <= 10)
                return true;

            valor = 0;
            return false;
        }

        private string CalcularEstado(double nota)
        {
            if (nota >= 7) return "Aprobado";
            if (nota >= 5 && nota <= 6.9) return "Complementario";
            return "Reprobado";
        }
    }

}
