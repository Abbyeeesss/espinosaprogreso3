using espinosaprogreso3.ViewModels;

namespace espinosaprogreso3.Views;

public partial class FormularioPage : ContentPage
{
    public FormularioPage(FormularioViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}