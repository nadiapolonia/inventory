using System.Collections.Generic;
using System.Linq;
using inventory;
using inventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private DatabaseContext context;

        public InventoryController(DatabaseContext _context)
        {
            this.context = _context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetAllItems()
        {
            var items = context.Items.OrderByDescending(i => i.DateOrdered);
            return items.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult GetOneItem(int id)
        {
            var item = context.Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(item);
            }
        }

        [HttpPost]
        public ActionResult<Item> CreateEntry([FromBody]Item entry)
        {
            context.Items.Add(entry);
            context.SaveChanges();
            return entry;
        }

        [HttpPut("{id}")]
        public ActionResult<Item> UpdateEntry([FromBody]Item entry, int id)
        {
            var updateItem = context.Items.FirstOrDefault(i => i.Id == id);
            context.Items.Update(updateItem);
            context.SaveChanges();
            return entry;
        }

        [HttpDelete("{id}")]

        public ActionResult<Item> DeleteEntry([FromBody]Item entry, int id)
        {
            var deleteItem = context.Items.FirstOrDefault(i => i.Id == id);
            context.Items.Remove(deleteItem);
            context.SaveChanges();
            return entry;
        }

        [HttpGet("outofstock")]
        public ActionResult OutOfStock()
        {
            var items = context.Items.Where(i => i.NumberInStock == 0).OrderByDescending(i => i.Name);
            if (items == null)
            {
                return NotFound();
            }
            else { return Ok(items); }
        }

        [HttpGet("sku/{SKU}")]
        public ActionResult SearchItem(string SKU)
        {
            var item = context.Items.FirstOrDefault(i => i.SKU == SKU);
            if (item == null)
            {
                return NotFound();
            }

            else
            {
                return Ok(item);
            }
        }
    }
}