# Caseiro.Mvc.PagedList

Caseiro.Mvc.PagedList is a library that helps you to create paged lists with ease. It also enable you to maintain your filters and can even order your lists.
Caseiro.Mvc.PagedList have helpers to create customizable pagers and table header columns.

[If you liked it, buy me a beer!](https://www.gittip.com/sandrocaseiro/)

## Where can I get it?

You can get it on [Nuget](http://nuget.org) from the package manager console:
```
PM> Install-Package Caseiro.Mvc.PagedList
```

## Example: Simple Paging

**/Models/User.cs**

```csharp
public class User
{
  public int Id { get; set; }
  public string Name { get; set; }
  
  [Display(Name = "E-mail")]
  public string Email { get; set; }
}
```

**/Controllers/HomeController.cs**

```csharp
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
```

**/Views/Home/Index.cshtml**

```csharp
@model PagedList<Models.User>

<table class="table table-striped table-bordered table-hover">
  <thead>
    <tr>
      <th>Id</th>
      <th>Name</th>
      <th>Email</th>
    </tr>
  </thead>
  <tbody>
    @foreach (var item in Model)
    {
      <tr>
      <td>@item.Id</td>
      <td>@item.Name</td>
      <td>@item.Email</td>
      </tr>
    }
  </tbody>
  <tfoot>
    <tr>
      <td colspan="3">
        @Html.Pager(Model, page => Url.Action("index", new { page = page }))
      </td>
    </tr>
  </tfoot>
</table>
```

## Example: Paging with filters and ordering

**/Models/Filter.cs**


```csharp
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
```

**/Controllers/UsersController.cs**

```csharp
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
```

**/Views/Users/Index.cshtml**

```csharp
@model Models.Filter

<div class="well well-lg">
@using (@Html.BeginForm("index", "users", new { page = UrlParameter.Optional }, FormMethod.Get, new { @class = "form-inline" }))
{
  @Html.HiddenFor(m => m.OrderField)
  @Html.HiddenFor(m => m.OrderDirection)
  <div class="form-group">
    @Html.LabelFor(m => m.Name)
    @Html.TextBoxFor(m => m.Name, new { @class = "form-control" })
  </div>
  <div class="form-group">
    @Html.LabelFor(m => m.Email)
    @Html.TextBoxFor(m => m.Email, new { @class = "form-control" })
  </div>
  <button type="submit" class="btn btn-default">Search</button>
}
</div>

<table class="table table-striped table-bordered table-hover">
  <thead>
    <tr>
      @Html.TableHeaderColumnsFor(Url.Action("index"), m => m.OrderField, m => m.OrderDirection,
      m => new
      {
        m.Users.Model.Id,
        m.Users.Model.Name,
        m.Users.Model.Email,
      })
    </tr>
  </thead>
  <tbody>
    @foreach (var item in Model.Users)
    {
      <tr>
      <td>@item.Id</td>
      <td>@item.Name</td>
      <td>@item.Email</td>
      </tr>
    }
  </tbody>
  <tfoot>
    <tr>
      <td colspan="3">
        @Html.PagerFilterFor(m => m.Users, page => Url.Action("index", new { page = page }))
      </td>
    </tr>
  </tfoot>
</table>
```

<hr />

## Pager and TableHeaderColumn Helpers

You have two options for the Pager helper:

```csharp
@Html.Pager(PagedList, page => Url.Action("index", new { page = page }))
@Html.PagerFor(m => m.PagedList, page => Url.Action("index", new { page = page }))
```
for simple paging
or
```csharp
@Html.PagerFilter(PagedList, FilterModel, page => Url.Action("index", new { page = page }))
@Html.PagerFilterFor(m => m.PagedList, page => Url.Action("index", new { page = page }))
```
for paging filtering. In this case, you can use the TableHeaderColumn helpers to order the results while maintaining filters.

To display the columns individually, use:
```csharp
@Html.TableHeaderColumnFor(Url.Action("index"), m => m.OrderField, m => m.OrderDirection, m => m.PagedList.Model.Id)
```

or you can display all the columns:
```csharp
@Html.TableHeaderColumnsFor(Url.Action("index"), m => m.OrderField, m => m.OrderDirection, 
m => new 
{ 
   m.PagedList.Model.Id,
   m.PagedList.Model.Id
})
```

## Url QueryString Extension Methods

You can generate a querystring with the properties and values of an object
```csharp
public class User
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Email { get; set; }
}
var user = new User { Id = 1, Name = "User", Email = "user@domain.com" };
user.ToQueryString() //generates: ?Id=1&Name=User&Email=user@domain.com
```

If doesn't want a property in the querystring, you can use the `[IgnoreQueryString]` attribute:
```csharp
public class User
{
  [IgnoreQueryString]
  public int Id { get; set; }
  public string Name { get; set; }
  public string Email { get; set; }
}
var user = new User { Id = 1, Name = "User", Email = "user@domain.com" };
user.ToQueryString() //generates: ?Name=User&Email=user@domain.com
```

You can also change the value of some property directly on the querystring generation without changind the value on the property itself:
```csharp
public class User
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Email { get; set; }
}
var user = new User { Id = 1, Name = "User", Email = "user@domain.com" };
user.ToQueryString(new KeyValuePair<object, object>("Id", "2")) //generates: ?Id=2&Name=User&Email=user@domain.com
```

<hr />

## License

Licensed under the [MIT License](http://www.opensource.org/licenses/mit-license.php).
