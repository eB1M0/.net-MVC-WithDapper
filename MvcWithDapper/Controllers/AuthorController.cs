using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcWithDapper.Models;
using System.Data.SqlClient;

namespace MvcWithDapper.Controllers
{
    public class AuthorController : Controller
    {
        private readonly string conn = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AuthorDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        // GET: AuthorController
        //public ActionResult Index()
        //{
        //    string sqlAuthor = "SELECT * FROM Authors;";

        //    using (var connection = new SqlConnection(conn))
        //    {
        //        var author = connection.Query<Author>(sqlAuthor).ToList();
        //        return View(author);
        //    }
        //}


        // GET: AuthorController
        public ActionResult Index(string sortOrder, string searchString)
        {
            string sqlAuthors = "SELECT * FROM Authors;";

            using (var connection = new SqlConnection(conn))
            {
                var authors = connection.Query<Author>(sqlAuthors).AsQueryable();

                ViewData["FirstNameSortParm"] = sortOrder == "first_name" ? "first_name_desc" : "first_name";
                ViewData["LastNameSortParm"] = sortOrder == "last_name" ? "last_name_desc" : "last_name";
                ViewData["AddressSortParm"] = sortOrder == "address" ? "address_desc" : "address";
                ViewData["CitySortParm"] = sortOrder == "city" ? "city_desc" : "city";
                ViewData["PostalCodeSortParm"] = sortOrder == "postal_code" ? "postal_code_desc" : "postal_code";
                ViewData["CountrySortParm"] = sortOrder == "country" ? "country_desc" : "country";
                ViewData["CurrentFilter"] = searchString;

                if (!String.IsNullOrEmpty(searchString))
                {
                    authors = authors.Where(a => a.LastName.Contains(searchString)
                                           || a.FirstName.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "first_name_desc":
                        authors = authors.OrderByDescending(a => a.FirstName);
                        break;
                    case "first_name":
                        authors = authors.OrderBy(a => a.FirstName);
                        break;
                    case "last_name_desc":
                        authors = authors.OrderByDescending(a => a.LastName);
                        break;
                    case "last_name":
                        authors = authors.OrderBy(a => a.LastName);
                        break;
                    case "address":
                        authors = authors.OrderBy(a => a.Address);
                        break;
                    case "address_desc":
                        authors = authors.OrderByDescending(a => a.Address);
                        break;
                    case "city":
                        authors = authors.OrderBy(a => a.City);
                        break;
                    case "city_desc":
                        authors = authors.OrderByDescending(a => a.City);
                        break;
                    case "postal_code":
                        authors = authors.OrderBy(a => a.PostalCode);
                        break;
                    case "postal_code_desc":
                        authors = authors.OrderByDescending(a => a.PostalCode);
                        break;
                    case "country":
                        authors = authors.OrderBy(a => a.Country);
                        break;
                    case "country_desc":
                        authors = authors.OrderByDescending(a => a.Country);
                        break;
                    default:
                        authors = authors.OrderBy(a => a.LastName);
                        break;
                }
                return View(authors.ToList());
            }
        }

        // GET: AuthorController/Details/5
        public ActionResult Details(int id)
        {
            string sqlAuthor = "SELECT * FROM Authors WHERE AuthorId = " + id + ";";
            using (var connection = new SqlConnection(conn))
            {
                var author = connection.Query<Author>(sqlAuthor).FirstOrDefault();
                return View(author);
            }
        }

        // GET: AuthorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("FirstName,LastName,Address, City, PostalCode, Country")] Author author)
        {
            string sql = "INSERT INTO Authors (FirstName, LastName, Address, City, PostalCode, Country) VALUES (@FirstName, @LastName, @Address, @City, @PostalCode, @Country);";
            try
            {
                if (ModelState.IsValid)
                {
                    using (var connection = new SqlConnection(conn))
                    {
                        var affectedRows = connection.Execute(sql,
                            new
                            {
                                FirstName = author.FirstName,
                                LastName = author.LastName,
                                Address = author.Address,
                                City = author.City,
                                PostalCode = author.PostalCode,
                                Country = author.Country
                            });

                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(author);
        }

        //// POST: AuthorController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: AuthorController/Edit/5
        public ActionResult Edit(int id)
        {
            string sqlAuthor = "SELECT * FROM Authors WHERE AuthorId = " + id + ";";

            using (var connection = new SqlConnection(conn))
            {
                var author = connection.Query<Author>(sqlAuthor).FirstOrDefault();
                return View(author);
            }
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("FirstName,LastName,Address, City, PostalCode, Country")] Author author)
        {
            string sql = @"UPDATE Authors 
                    SET FirstName = @FirstName, 
                        LastName = @LastName, 
                        Address = @Address, 
                        City = @City, 
                        PostalCode = @PostalCode, 
                        Country = @Country
                    WHERE AuthorId = " + id + ";";
            try
            {
                if (ModelState.IsValid)
                {
                    using (var connection = new SqlConnection(conn))
                    {
                        var affectedRows = connection.Execute(sql,
                            new
                            {
                                FirstName = author.FirstName,
                                LastName = author.LastName,
                                Address = author.Address,
                                City = author.City,
                                PostalCode = author.PostalCode,
                                Country = author.Country
                            });

                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(author);
        }

        // GET: AuthorController/Delete/5
        public ActionResult Delete(int id)
        {
            string sqlAuthor = "SELECT * FROM Authors WHERE AuthorId = " + id + ";";

            using (var connection = new SqlConnection(conn))
            {
                var author = connection.Query<Author>(sqlAuthor).FirstOrDefault();
                return View(author);
            }
        }

        // POST: AuthorController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string sql = "DELETE FROM Authors WHERE AuthorId = " + id + ";";
            try
            {
                if (ModelState.IsValid)
                {
                    using (var connection = new SqlConnection(conn))
                    {
                        var affectedRows = connection.Execute(sql);

                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View();
        }

    }
}
