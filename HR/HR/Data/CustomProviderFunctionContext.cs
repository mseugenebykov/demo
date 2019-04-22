using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HR.Data
{
    public class CustomProviderFunctionContext
    {
        private const string FunctionURI = "/api/EmployeeFunctions";

        private static HttpClient httpClient = new HttpClient();
    }
}
