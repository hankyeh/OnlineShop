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
using ShopBackend.ViewModels;

namespace ShopBackend.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopBackEndContext _context;
        //呼叫API 使用
        private readonly IHttpClientFactory _clientFactory;

        //呼叫Products API URL
        string url = ConstValue.API_PRODUCTS;
        //呼叫Category API URL
        string urlCategory = ConstValue.API_CATEGORIES;

        //取得wwwroot路徑
        private IWebHostEnvironment _hostingEnvironment;

        public ProductsController(ShopBackEndContext context, IHttpClientFactory clientFactory, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _clientFactory = clientFactory;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            WebApiConn wac = new WebApiConn();
            //取回Products清單
            List<Product> Result = wac.GetApi<List<Product>>(url);
            //取回Category清單
            List<ViewCategory> Category = wac.GetApi<List<ViewCategory>>(urlCategory);
            ViewBag.CategoryId = Category;

            return View(Result);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //呼叫Products/Details API
             url = string.Format("{0}/{1}", url, id);
            WebApiConn wac = new WebApiConn();
            Product Result = wac.GetApi<Product>(url);

            if (Result == null)
            {
                return NotFound();
            }

            return View(Result);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            //呼叫Category API 取得清單
            WebApiConn wac = new WebApiConn();
            List<Category> Result = wac.GetApi<List<Category>>(urlCategory);
            ViewBag.CategoryId = new SelectList(Result.Select(x => x), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Content,Price,Stock,ProdImg,CategoryId")] Product product, IFormFile myimg)
        {
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "img");

            if (myimg.Length > 0)
            {
                //取出副檔名
                string subName = myimg.FileName.Split(".").LastOrDefault<string>();
                //沒有副檔名，可能是壞檔
                if (subName != null || (subName != "jpg" && subName != "png"))
                {
                    //重新命名圖片
                    string fileName = string.Format("{0}.{1}", Guid.NewGuid(), subName);
                    string filePath = Path.Combine(uploads, fileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await myimg.CopyToAsync(fileStream);
                        product.ProdImg = fileName;
                    }
                }
                else
                {
                    //呼叫Category/Create API
                    string urlCategory = ConstValue.API_PRODUCTS;
                    WebApiConn wacCategory = new WebApiConn();
                    List<Category> ResultCategory = wacCategory.GetApi<List<Category>>(urlCategory);
                    ViewBag.CategoryId = new SelectList(ResultCategory.Select(x => x), "Id", "Name");
                    return View(product);
                }

            }

            string jsonCategory = JsonSerializer.Serialize(product);
            //呼叫Product/Create API
            string url = ConstValue.API_PRODUCTS;
            WebApiConn wac = new WebApiConn();
            Product Result = wac.PostApi<Product>(url, jsonCategory);
            return RedirectToAction(nameof(Index));

        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //呼叫Products/Details API
            string url = string.Format("{0}/{1}", ConstValue.API_PRODUCTS, id);
            WebApiConn wac = new WebApiConn();
            Product Result = wac.GetApi<Product>(url);

            if (Result == null)
            {
                return NotFound();
            }
            
            WebApiConn wacCategory = new WebApiConn();
            List<Category> ResultCategory = wacCategory.GetApi<List<Category>>(urlCategory);
            ViewBag.CategoryId = new SelectList(ResultCategory.Select(x => x), "Id", "Name");
            return View(Result);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Content,Price,Stock,ProdImg,CategoryId")] Product product, IFormFile myimg)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "img");

            //呼叫Category/Create API

            if (myimg.Length > 0)
            {
                //取出副檔名
                string subName = myimg.FileName.Split(".").LastOrDefault<string>();
                //沒有副檔名，可能是壞檔
                if (subName != null || (subName != "jpg" && subName != "png"))
                {
                    //重新命名圖片
                    string fileName = string.Format("{0}.{1}", Guid.NewGuid(), subName);
                    string filePath = Path.Combine(uploads, fileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await myimg.CopyToAsync(fileStream);
                        product.ProdImg = fileName;
                    }
                }
                else
                {
                    //呼叫Category/Create API
                    string urlCategory = ConstValue.API_PRODUCTS;
                    WebApiConn wacCategory = new WebApiConn();
                    List<Category> ResultCategory_2= wacCategory.GetApi<List<Category>>(urlCategory);
                    ViewBag.CategoryId = new SelectList(ResultCategory_2.Select(x => x), "Id", "Name");
                    return View(product);
                }

            }

            //取回數值先轉成json 字串
            string jsonCategory = JsonSerializer.Serialize(product);
            //呼叫Categories/Edit API
            string url = string.Format("{0}/{1}", ConstValue.API_PRODUCTS, id);
            WebApiConn wac = new WebApiConn();
            HttpStatusCode Result = wac.PutApi(url, jsonCategory);

            //呼叫Category/Create API
            List<Category> ResultCategory = wac.GetApi<List<Category>>(url);
            ViewBag.CategoryId = new SelectList(ResultCategory.Select(x => x), "Id", "Name");
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //呼叫Products/Details API
            string url = string.Format("{0}/{1}", ConstValue.API_PRODUCTS, id);
            WebApiConn wac = new WebApiConn();
            Product Result = wac.GetApi<Product>(url);
            if (Result == null)
            {
                return NotFound();
            }

            return View(Result);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //呼叫Products/delete API
            string url = string.Format("{0}/{1}", ConstValue.API_PRODUCTS, id);
            WebApiConn wac = new WebApiConn();
            HttpStatusCode Result = wac.DeleteApi(url);

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
