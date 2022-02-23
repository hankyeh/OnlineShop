#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopBackend.Data;
using ShopBackend.Models;
using ShopBackend.UTL;

namespace ShopBackend.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ShopBackEndContext _context;
        //呼叫API 使用
        private readonly IHttpClientFactory _clientFactory;

        public CategoriesController(ShopBackEndContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;

            }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            string url = ConstValue.API_CATEGORIES;
            WebApiConn wac = new WebApiConn();
            List<Category> Result = wac.GetApi<List<Category>>(url);
            return View(Result);
        }

        // GET: Categories/Details/5
        /// <summary>
        /// 單筆 Categories 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //呼叫Categories/Details API
            string url = string.Format("{0}/{1}", ConstValue.API_CATEGORIES, id);
            WebApiConn wac = new WebApiConn();
            Category Result = wac.GetApi<Category>(url);

            if (Result == null)
            {
                return NotFound();
            }

            return View(Result);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {

            string jsonCategory = JsonSerializer.Serialize(category);
            //呼叫Categories/Details API

            string url = ConstValue.API_CATEGORIES;
            WebApiConn wac = new WebApiConn();
            Category Result = wac.PostApi<Category>(url, jsonCategory);

            //if (ModelState.IsValid)
            //{
            //_context.Add(category);
            //    await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //}
            //return View(category);
        }

        // GET: Categories/Edit/5
        /// <summary>
        /// 開啟編輯Categories
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //呼叫Categories/Details API
            string url = string.Format("{0}/{1}", ConstValue.API_CATEGORIES, id);
            WebApiConn wac = new WebApiConn();
            Category Result = wac.GetApi<Category>(url);

            if (Result == null)
            {
                return NotFound();
            }
            return View(Result);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            string jsonCategory = JsonSerializer.Serialize(category);
            //呼叫Categories/delete API
            string url = string.Format("{0}/{1}", ConstValue.API_CATEGORIES, id); 
            WebApiConn wac = new WebApiConn();
            HttpStatusCode Result = wac.PutApi(url, jsonCategory);

            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string url = string.Format("{0}/{1}", ConstValue.API_CATEGORIES, id);
            WebApiConn wac = new WebApiConn();
            Category Result = wac.GetApi<Category>(url);

            if (Result == null)
            {
                return NotFound();
            }

            return View(Result);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //呼叫Categories/delete API
            string url = string.Format("{0}/{1}", ConstValue.API_CATEGORIES, id);
            WebApiConn wac = new WebApiConn();
            HttpStatusCode Result = wac.DeleteApi(url);

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
