using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Mapping;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceIt.Data.Interfaces;
using System.Threading.Tasks;
using PriceIt.Data.Models;
using PriceIt.Models;

namespace PriceIt.Controllers
{
    public class UserListController : Controller
    {
        private readonly IListRepository _listRepository;
        private readonly IProductsRepository _productsRepository;

        public UserListController(IListRepository listRepository,IProductsRepository productsRepository)
        {
            _listRepository = listRepository;
            _productsRepository = productsRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            var user = _listRepository.GetCurrentUser();
            var lists = _listRepository.GetUserLists(user);

            return View(lists);
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(string listname)
        {
            if (listname == null)
            {
                listname = "";
            }

            await _listRepository.AddListAsync(listname);

            if (!_listRepository.Save())
            {
                ViewData["SavingError"] = "error while saving";

                return View();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!_listRepository.HasAccessList(id))
            {
                return NotFound();
            }

            var list = _listRepository.GetUserListById(id);
            if (list == null)
                return NotFound();

            return View(list);
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            if (!_listRepository.DeleteList(id))
                return BadRequest("Could not delete!");

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!_listRepository.HasAccessList(id))
            {
                return NotFound();
            }

            var list = _listRepository.GetUserListById(id);
            if (list == null)
                return NotFound();

            return View(list);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditPost(int id,string listname)
        {
            var list = _listRepository.GetUserListById(id);
            if (list == null)
                return NotFound();

            if(listname == null)
                list.Name = "";

            list.Name = listname;

            if (!_listRepository.UpdateList(list))
                return BadRequest("Could not edit");

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            if (!_listRepository.HasAccessList(id))
                return NotFound();

            var list = _listRepository.GetUserListById(id);

            foreach (var listItem in list.ListItems)
            {
                listItem.Product = _productsRepository.GetProduct(listItem.ProductId);
            }

            return View(list);
        }

        [Authorize]
        public IActionResult AddProductToList(int productId)
        {
            var user = _listRepository.GetCurrentUser();
            var lists = _listRepository.GetUserLists(user);

            if (lists == null || !lists.Any())
            {
                return RedirectToAction("Index");
            }

            var viewModel = new AddProductToListViewModel
            {
                ProductId = productId,
                Lists = lists
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddProductToListPost(int productId, int listId)
        {
            var list = _listRepository.GetUserListById(listId);

            if (list == null)
                return NotFound();

            var product = _productsRepository.GetProduct(productId);

            if (product == null)
                return NotFound();

            if (!_listRepository.AddProductToList(product, list)) return BadRequest("Could not save!");
            
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public IActionResult IncreaseListItemCount(int listId, int itemId)
        {
            var list = _listRepository.GetUserListById(listId);

            if (list == null)
                return NotFound();

            if (!_listRepository.IncreaseListItemCount(list, itemId))
                return BadRequest("Failed to increase product quantity please try again");

            return RedirectToAction("Details", new { id = listId});
        }

        [Authorize]
        [HttpPost]
        public IActionResult DecreaseListItemOrDelete(int listId, int itemId)
        {
            var list = _listRepository.GetUserListById(listId);

            if (list == null)
                return NotFound();

            if (!_listRepository.DecreaseListItemOrDelete(list, itemId))
                return BadRequest("Failed to decrease product quantity please try again");

            return RedirectToAction("Details", new {id = listId});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
