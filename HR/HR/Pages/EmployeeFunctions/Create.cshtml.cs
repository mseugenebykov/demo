using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HR.Data;
using HR.Models;
using System.Net.Http;

namespace HR.Pages.EmployeeFunctions
{
    public class CreateModel : PageModel
    {
        private const string FunctionURI = "/api/EmployeeFunctions";

        private static HttpClient httpClient = new HttpClient();
        private readonly EmployeeFunctionClient client;

        public CreateModel(IHttpClientFactory clientFactory)
        {
            client = new EmployeeFunctionClient(clientFactory.CreateClient(EmployeeFunctionClient.DefaultClientName));
        }

        public IActionResult OnGet()
        {
            this.Employee = new Employee
            {
                ID = Guid.NewGuid()
            };

            return Page();
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await client.PostAsync(this.HttpContext, this.Employee);

            return RedirectToPage((response.IsSuccessStatusCode) ? "../Tools": "../Error"); 
        }
    }
}