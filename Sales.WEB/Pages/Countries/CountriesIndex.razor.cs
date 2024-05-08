using Microsoft.AspNetCore.Components;
using Sales.Share.Entities;
using Sales.WEB.Repositories;

namespace Sales.WEB.Pages.Countries
{
    public partial class CountriesIndex
    {
        [Inject] private IRepository Repository { get; set; } = null;
        public List<Country>? Countries { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries");
            Countries = responseHttp.Response;
        }
    }
}
