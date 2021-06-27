using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceIt.Data.Interfaces;
using System.Threading.Tasks;
using PriceIt.Data.Models;

namespace PriceIt.Controllers
{
    public class UserListController : Controller
    {
        private readonly IListRepository _listRepository;

        public UserListController(IListRepository listRepository)
        {
            _listRepository = listRepository;
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
    }
}
