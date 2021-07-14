using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shortlock.Models
{
	public class ShortLinkResponse
	{
		public string URL { get; set; }
		public string Error { get; set; }
		public bool IsError { get; set; }
	}
}
