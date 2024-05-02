using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RinkLine.Public.Helper;
using ToDoAppDemo.WebApp.Models;

namespace ToDoAppDemo.WebApp.Controllers
{
    public class ToDoTaskController : Controller
    {
        private readonly string _apiBaseAddress;
        public ToDoTaskController(IConfiguration configuration)
        {
            _apiBaseAddress = configuration["API_BASE_ADDRESS"];
        }
        // GET: ToDoTaskController
        public async Task<ActionResult> Index()
        {
            var route = "/api/ToDoTasks";

            ViewBag.ErrorMessage = "";
            Response<List<ToDoTaskDto>> response = await DataHelper<List<ToDoTaskDto>>.Execute(_apiBaseAddress, route, OperationType.GET);
            if (response.Result.Data == null)
            {
                ViewBag.ErrorMessage = "Something went wrong.";
            }

            return View(response.Result?.Data ?? []);
        }

        // GET: ToDoTaskController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var route = $"/api/ToDoTasks/{id}";

            ViewBag.ErrorMessage = "";
            Response<ToDoTaskDto> response = await DataHelper<ToDoTaskDto>.Execute(_apiBaseAddress, route, OperationType.GET);
            if (response.Result.Data == null)
            {
                ViewBag.ErrorMessage = "Something went wrong.";
                return View(new ToDoTaskDto());
            }

            return View(response.Result.Data);
        }

        // GET: ToDoTaskController/Create
        public ActionResult Create()
        {
            return View(new ToDoTaskDto());
        }

        // POST: ToDoTaskController/Create
        [HttpPost]
        //The basic purpose of ValidateAntiForgeryToken attribute is to prevent cross-site request forgery attacks.
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ToDoTaskDto collection)
        {
            try
            {
                if (collection.Id > 0)
                {
                    var route = $"/api/ToDoTasks/{collection.Id}";

                    ViewBag.ErrorMessage = "";
                    Response<object> response = await DataHelper<object>.Execute(_apiBaseAddress, route, OperationType.PUT, collection);
                    if (!response.Result.Success)
                    {
                        ViewBag.ErrorMessage = "Something went wrong.";
                        return View(collection);
                    }
                }
                else
                {
                    var route = "/api/ToDoTasks";

                    ViewBag.ErrorMessage = "";
                    Response<ToDoTaskDto> response = await DataHelper<ToDoTaskDto>.Execute(_apiBaseAddress, route, OperationType.POST, collection);
                    if (!response.Result.Success)
                    {
                        ViewBag.ErrorMessage = "Something went wrong.";
                        return View(collection);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ToDoTaskController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var route = $"/api/ToDoTasks/{id}";

            ViewBag.ErrorMessage = "";
            Response<ToDoTaskDto> response = await DataHelper<ToDoTaskDto>.Execute(_apiBaseAddress, route, OperationType.GET);
            if (response.Result.Data == null)
            {
                ViewBag.ErrorMessage = "Something went wrong.";
                return View(new ToDoTaskDto());
            }

            return View("Create", response.Result.Data);
        }

        // GET: ToDoTaskController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var route = $"/api/ToDoTasks/{id}";

            ViewBag.ErrorMessage = "";
            Response<ToDoTaskDto> response = await DataHelper<ToDoTaskDto>.Execute(_apiBaseAddress, route, OperationType.GET);
            if (response.Result.Data == null)
            {
                ViewBag.ErrorMessage = "Something went wrong.";
                return View(new ToDoTaskDto());
            }

            return View(response.Result.Data);
        }

        // POST: ToDoTaskController/Delete/5
        [HttpPost]
        //The basic purpose of ValidateAntiForgeryToken attribute is to prevent cross-site request forgery attacks.
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var route = $"/api/ToDoTasks/{id}";

                ViewBag.ErrorMessage = "";
                Response<ToDoTaskDto> response = await DataHelper<ToDoTaskDto>.Execute(_apiBaseAddress, route, OperationType.DELETE);
                if (!response.Result.Success)
                {
                    ViewBag.ErrorMessage = "Something went wrong.";
                    return View(collection);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
