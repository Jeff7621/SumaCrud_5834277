﻿namespace SumaCrud
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbsService;
        private int _editResultadoId;

        public MainPage(LocalDbService dbsService)
        {
            InitializeComponent();
            _dbsService = dbsService;
            Task.Run(async () => Listview.ItemsSource = await _dbsService.GetResultado());
        }



        private async void sumarBtn_Clicked(object sender, EventArgs e)
        {

            if (_editResultadoId == 0)
            {
                //agregar cliente
                await _dbsService.Create(new Resultado
                {
                    Numero1 = Entryprimernumero.Text,
                    Numero2 = Entrysegunodnumero.Text,
                    Suma = labelresultado.Text

                });
            }
            else
            {
                //Editar cliente
                await _dbsService.Update(new Resultado
                {
                    Id = _editResultadoId,
                    Numero1 = Entryprimernumero.Text,
                    Numero2 = Entrysegunodnumero.Text,
                    Suma = labelresultado.Text
                });
                _editResultadoId = 0;
            }

            Entryprimernumero.Text = string.Empty;
            Entrysegunodnumero.Text = string.Empty;
            labelresultado.Text = string.Empty;


            Listview.ItemsSource = await _dbsService.GetResultado();


        }

        private async void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var resultado = (Resultado)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    _editResultadoId = resultado.Id;
                    Entryprimernumero.Text = resultado.Numero1;
                    Entrysegunodnumero.Text = resultado.Numero2;
                    labelresultado.Text = resultado.Suma;
                    break;

                case "Delete":
                    await _dbsService.Delete(resultado);
                    Listview.ItemsSource = await _dbsService.GetResultado();
                    break;

            }
        }

    }
}
