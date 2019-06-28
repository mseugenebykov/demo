using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HR.Data;
using HR.Models;
using Microsoft.AspNetCore.Authorization;
using HR.Identity;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using Microsoft.AspNetCore.Antiforgery;

namespace HR.Controllers
{
    [Authorize(AuthenticationSchemes = ApiAuthDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ListKeysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtectionProvider _protectionProvider;
        private readonly IAntiforgery _antiforgery;
        public ListKeysController(ApplicationDbContext context, IDataProtectionProvider protectionProvider, IAntiforgery antiforgery)
        {
            _context = context;
            _protectionProvider = protectionProvider;
            _antiforgery = antiforgery;
        }

        // GET: api/Employees
        [HttpPost]
        public IEnumerable<Key> PostListKeys()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Guid.NewGuid().ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, "ARM");

            var protectionProvider = DataProtectionProvider.Create("MyApplication");
            var dataProtector = protectionProvider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", "ARM", "v2");

            var ticketDataFormat = new TicketDataFormat(dataProtector);
            var cookieValue = ticketDataFormat.Protect(new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), "ARM"), GetTlsTokenBinding());

            return new List<Key>() {
                new Key { Token = $"AMAHosted.Token={cookieValue}" },
                new Key { Token = ".AspNet.Consent=yes" }
            };
        }

        private string GetTlsTokenBinding()
        {
            var binding = HttpContext.Features.Get<ITlsTokenBindingFeature>()?.GetProvidedTokenBindingId();
            return binding == null ? null : Convert.ToBase64String(binding);
        }
    }
}