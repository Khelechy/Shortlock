using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shortlock.Models
{
	public class ShortlinkViewModel
	{
		public string RawURL { get; set; }

#nullable enable
		public string? RawPassword { get; set; } 
	}
}
