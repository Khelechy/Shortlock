using LiteDB;
using Microsoft.AspNetCore.Http;
using Shortlock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shortlock.Services
{
	public class ShortlockService : IShortlockService
	{
		private readonly ILiteDatabase _liteDatabase;

		public ShortlockService(ILiteDatabase liteDatabase)
		{
            _liteDatabase = liteDatabase;
		}

		public bool AuthenticateURL(string password, string storedPassword)
		{
            
            bool verified = BCrypt.Net.BCrypt.Verify(password, storedPassword);
            return verified;
        }

		public ShortLink GetShortlink(HttpContext context)
		{
            var collection = _liteDatabase.GetCollection<ShortLink>();
            var path = context.Request.Path.ToUriComponent().Trim('/');
            if (path.Length == 6)
            {
                var id = ShortLink.GetId(path);
                var entry = collection.Find(p => p.Id == id).FirstOrDefault();
                return entry;
			}
			else
			{
                return null;
			}
        }

		public ShortLinkResponse HandleShortenURL(ShortlinkViewModel model)
		{
            var error = string.Empty;
            var requestedUrl = model.RawURL;
            var password = model.RawPassword;

            // Test our URL
            if (!Uri.TryCreate(requestedUrl, UriKind.Absolute, out Uri result))
            {
                return new ShortLinkResponse{
                    URL = null,
                    Error = "Could not understand URL, format URL properly",
                    IsError = true
                };
               


            }

            var url = result.ToString();
            // Ask for LiteDB and persist a short link
            var links = _liteDatabase.GetCollection<ShortLink>(BsonAutoId.Int32);

            bool isLocked = string.IsNullOrEmpty(password) ? false : true;
            string passwordHash = string.Empty;
			if (isLocked)
			{
                passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            }

            // Temporary short link 
            var entry = new ShortLink
            {
                Url = url,
                IsLocked = isLocked,
                Password = passwordHash
            };

            // Insert our short-link
            links.Insert(entry);

            var urlChunk = entry.GetUrlChunk();
            var responseUri = urlChunk;
            return new ShortLinkResponse
            {
                URL = responseUri,
                Error = "",
                IsError = false
            };
        }
	}
}
