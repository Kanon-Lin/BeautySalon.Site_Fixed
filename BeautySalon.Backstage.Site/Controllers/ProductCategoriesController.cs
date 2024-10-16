using BeautySalon.Backstage.Site.Models.Dtos;
using BeautySalon.Backstage.Site.Models.Services;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BeautySalon.Backstage.Site.Controllers
{
    public class ProductCategoriesController : Controller
    {
        // GET: ProductCategories
        [HttpGet]
        public JsonResult CategoryIndex()
        {
            var service = new ProductCategoryService();
            var data = service.GetAllCategories();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateCategory(ProductCategoryVm vm)
        {
            var service = new ProductCategoryService();

            if (ModelState.IsValid)
            {
                try
                {
                    ProductCategoryDto dto = vm.ToDto();
                    service.CreateCategory(dto);
                    return Json(new { success = true, message = "新增成功" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"新增產品類別時發生錯誤: {ex.Message}" });
                }
            }

            // 如果驗證失敗
            return Json(new { success = false, message = "驗證失敗" });
        }

        [HttpGet]
        public JsonResult GetCategory(int? id) // 接受可為 null 的 id
        {
            if (!id.HasValue)
            {
                return Json(new { success = false, message = "ID 是必需的" }, JsonRequestBehavior.AllowGet);
            }

            var service = new ProductCategoryService();
            var dto = service.GetCategory(id.Value); // 獲取數據

            if (dto == null)
            {
                return Json(new { success = false, message = "未找到產品類別" }, JsonRequestBehavior.AllowGet);
            }

            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCategory(ProductCategoryVm vm)
        {
            var service = new ProductCategoryService();
            ProductCategoryDto dto = WebApiApplication._mapper.Map<ProductCategoryDto>(vm);

            try
            {
                service.UpdateCategory(dto);
                return Json(new { success = true, message = "修改成功" });
            }
            catch
            {
                return Json(new { success = false, message = "未找到產品類別" }, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// </summary>



        private readonly HttpClient _httpClient;

        public ProductCategoriesController()
        {
            _httpClient = new HttpClient();
        }

        // 顯示服務類別列表
        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:44302/api/CategoriesApi/CategoryIndex");
            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadAsAsync<List<ProductCategoryVm>>();
                return View(categories);
            }
            return View(new List<ProductCategoryVm>()); // 返回空列表
        }

        // 顯示新增服務類別頁面
        public ActionResult Create()
        {
            return View();
        }

        // 顯示編輯服務類別頁面
        public async Task<ActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44302/api/CategoriesApi/GetCategory/{id}");
            if (response.IsSuccessStatusCode)
            {
                var category = await response.Content.ReadAsAsync<ProductCategoryVm>();
                return View(category);
            }
            return HttpNotFound();
        }

        // 顯示刪除確認頁面
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44302/api/CategoriesApi/GetCategory/{id}");
            if (response.IsSuccessStatusCode)
            {
                var category = await response.Content.ReadAsAsync<ProductCategoryVm>();
                return View(category);
            }
            return HttpNotFound();
        }

        /// <summary>
        /// </summary>
        /// 

        // 顯示服務項目列表
        public async Task<ActionResult> ProductIndex()
        {
            var response = await _httpClient.GetAsync("https://localhost:44302/api/ProductsApi/ProductIndext");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadAsAsync<List<ProductVm>>();
                return View(products);
            }
            return View(new List<ProductVm>()); // 返回空列表
        }

        // 顯示新增服務項目頁面
        public ActionResult ProductCreate()
        {
            return View();
        }

        // 顯示編輯服務項目頁面
        public async Task<ActionResult> ProductEdit(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44302/api/ProductsApi/GetProduct/{id}");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadAsAsync<ProductVm>();
                return View(products);
            }
            return HttpNotFound();
        }

        // 顯示刪除確認頁面
        public async Task<ActionResult> ProductDelete(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44302/api/ProductsApi/GetProduct/{id}");
            if (response.IsSuccessStatusCode)
            {
                var category = await response.Content.ReadAsAsync<ProductVm>();
                return View(category);
            }
            return HttpNotFound();
        }

    }
}