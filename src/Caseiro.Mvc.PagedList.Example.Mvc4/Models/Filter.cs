using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caseiro.Mvc.PagedList;
using Caseiro.Mvc.PagedList.Attributes;
using Caseiro.Mvc.PagedList.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Caseiro.Mvc.PagedList.Example.Mvc4.Models
{
	public class Filter
	{
		public Filter()
		{
			this.Page = 1;
			this.OrderField = "Id";
			this.OrderDirection = OrderDirection.Ascending;
		}

		[IgnoreQueryString]
		public int Page { get; set; }
		public string OrderField { get; set; }
		public OrderDirection OrderDirection { get; set; }

		public string Name { get; set; }

		[Display(Name = "E-mail")]
		public string Email { get; set; }

		public PagedList<User> Users { get; private set; }

		public void SetPagedList(IQueryable<User> queryable)
		{
			this.Users = queryable.ToPagedList(this.Page, 10, this.OrderField, this.OrderDirection);
		}
	}
}