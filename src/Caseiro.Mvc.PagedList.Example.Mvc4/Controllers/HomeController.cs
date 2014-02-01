using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caseiro.Mvc.PagedList.Extensions;

namespace Caseiro.Mvc.PagedList.Example.Mvc4.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(int page = 1)
		{
			return View(GetUsers().ToPagedList(page, 10));
		}

		private IQueryable<Models.User> GetUsers()
		{
			var users = new List<Models.User>();
			for(int i = 1; i <= 30; i++)
			{
				users.Add(new Models.User
					{
						Id = i,
						Name = "User " + i.ToString(),
						Email = "user" + i.ToString() + "@something.com"
					});
			}

			return users.AsQueryable();
		}
	}
}
