using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caseiro.Mvc.PagedList.Example.Mvc4.Controllers
{
	public class UsersController : Controller
	{
		public ActionResult Index(Models.Filter model)
		{
			model.SetPagedList(GetUsers(model.Name, model.Email));

			return View(model);
		}

		private IQueryable<Models.User> GetUsers(string name, string email)
		{
			name = name ?? string.Empty;
			email = email ?? string.Empty;

			var users = new List<Models.User>();
			for (int i = 1; i <= 30; i++)
			{
				users.Add(new Models.User
				{
					Id = i,
					Name = "User " + i.ToString(),
					Email = "user" + i.ToString() + "@something.com"
				});
			}

			return users.Where(u => u.Name.Contains(name) && u.Email.Contains(email)).AsQueryable();
		}
	}
}
