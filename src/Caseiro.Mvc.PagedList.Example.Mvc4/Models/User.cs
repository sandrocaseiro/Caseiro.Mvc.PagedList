using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Caseiro.Mvc.PagedList.Example.Mvc4.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }

		[Display(Name = "E-mail")]
		public string Email { get; set; }
	}
}