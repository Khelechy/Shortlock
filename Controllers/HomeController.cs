using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shortlock.Models;
using Shortlock.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Shortlock.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IShortlockService _shortlockService;

		public HomeController(ILogger<HomeController> logger, IShortlockService shortlockService)
		{
			_logger = logger;
			_shortlockService = shortlockService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public Task Shorten(ShortlinkViewModel model)
		{
			var result = _shortlockService.HandleShortenURL(model);
			var fullUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{result.URL}";
			if (!result.IsError)
			{
				TempData["URL"] = fullUrl;
				HttpContext.Response.Redirect($"/#{fullUrl}");


			}
			else
			{
				HttpContext.Response.Redirect("/");
			}
			return Task.CompletedTask;
		}

		

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
