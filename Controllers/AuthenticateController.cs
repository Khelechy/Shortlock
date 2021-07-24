using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shortlock.Models;
using Shortlock.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shortlock.Controllers
{
	public class AuthenticateController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IShortlockService _shortlockService;

		public AuthenticateController(ILogger<HomeController> logger, IShortlockService shortlockService)
		{
			_logger = logger;
			_shortlockService = shortlockService;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GetURL()
		{
			var result = _shortlockService.GetShortlink(HttpContext);
			if (result != null)
			{
				if (result.IsLocked)
				{
					HttpContext.Session.SetString("LinkURL", result.Url);
					HttpContext.Session.SetString("LinkPassword", result.Password);
					return RedirectToAction(nameof(Index));
				}
				else
				{
					return Redirect(result.Url);
				}

			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
		public Task AuthenticateURL(ShortlinkViewModel model)
		{
			var storedPassword = HttpContext.Session.GetString("LinkPassword");
			if (storedPassword == null)
			{
				HttpContext.Response.Redirect($"/Authenticate/#Unable to get linkedPassword!");
			}
			var result = _shortlockService.AuthenticateURL(model.RawPassword, storedPassword);
			if (result)
			{
				HttpContext.Response.Redirect(HttpContext.Session.GetString("LinkURL"));
			}
			else
			{
				HttpContext.Response.Redirect($"/Authenticate/#Password not correct!");
			}

			return Task.CompletedTask;
		}
	}
}
