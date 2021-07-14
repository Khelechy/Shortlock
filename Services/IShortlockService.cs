using Microsoft.AspNetCore.Http;
using Shortlock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shortlock.Services
{
	public interface IShortlockService
	{
		ShortLinkResponse HandleShortenURL(ShortlinkViewModel model);
		ShortLink GetShortlink(HttpContext context);
		bool AuthenticateURL(string password, string storedPassword);
	}
}
