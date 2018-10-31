using BookLibrary.APP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BookLibrary.APP.Controllers
{
    public class BookController : Controller
    {
        #region Index
        // GET: Books
        public ActionResult Index(string search_param, string str)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:54162/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var apiRoute = "api/authors/books";

            if (str != null && str.Trim() != "")
            {
                if (search_param == "author")
                {
                    apiRoute = "api/authors/books/search/author/" + str;
                }
                else if (search_param == "title")
                {
                    apiRoute = "api/authors/books/search/title/" + str;
                }
                else if (search_param == "all")
                {
                    apiRoute = "api/authors/books/search/all/" + str;
                }
            }

            HttpResponseMessage response = client.GetAsync(apiRoute).Result;

            var book = response.Content.ReadAsAsync<IEnumerable<BookModel>>().Result;
            BookPageModel bpm = new BookPageModel();
            bpm.searchTerm = str;
            bpm.searchType = search_param;
            bpm.BookModels = book;

            return View(bpm);
        }
        #endregion

        #region Details
        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion

        #region Create
        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Create
        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Edit
        // GET: Books/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        #endregion

        #region Edit
        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Delete
        // GET: Books/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        #endregion

        #region Delete
        // POST: Books/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
