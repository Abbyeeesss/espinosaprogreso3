using espinosaprogreso3.ViewModels;

namespace espinosaprogreso3.Views;

public partial class ListaPrendaPage : ContentPage
{
    private readonly ListaPrendasViewModel _viewModel;

    public ListaPrendaPage(ListaPrendasViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadPrendasAsync();
    }
}