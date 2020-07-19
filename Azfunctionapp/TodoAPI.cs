using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Azfunctionapp
{
    public static class TodoAPI
    {
        static List<TodoItem> items = new List<TodoItem>();

        [FunctionName("Create")]
        public static async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            string reqBody = await new StreamReader(req.Body).ReadToEndAsync().ConfigureAwait(false);
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(reqBody);

            TodoItem newItem = new TodoItem(input.Name);
            newItem.Id = Guid.NewGuid().ToString();
            newItem.IsCompleted = false;

            items.Add(newItem);

            return new OkObjectResult(newItem);
        }

        [FunctionName("GetAllItems")]
        public static async Task<IActionResult> GetAllItems(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo")] HttpRequest req,
    ILogger log)
        {
            var todoItems = items.Where(x => x.IsCompleted == false);
            return new OkObjectResult(todoItems);
        }

        [FunctionName("GetItemById")]
        public static async Task<IActionResult> GetItemById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo/{id}")] HttpRequest req,
        ILogger log, string id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            var currentItem = items.Where(x => x.Id == id).FirstOrDefault();

            if(currentItem == null)
            {
                return new NotFoundObjectResult($"Item Is not present with Id {id}");
            }

            return new OkObjectResult(currentItem);
        }


        [FunctionName("CompleteItem")]
        public static async Task<IActionResult> CompleteItem(
    [HttpTrigger(AuthorizationLevel.Function, "put", Route = "todo/{id}")] HttpRequest req,
    ILogger log, string id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            var currentItem = items.Where(x => x.Id == id).FirstOrDefault();
            if (currentItem == null)
            {
                return new NotFoundObjectResult($"Item Is not present with Id {id}");
            }
            currentItem.IsCompleted = true;

            return new OkObjectResult(currentItem);
        }

        [FunctionName("DeleteItem")]
        public static async Task<IActionResult> DeleteItem(
[HttpTrigger(AuthorizationLevel.Function, "delete", Route = "todo/{id}")] HttpRequest req,
ILogger log, string id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            var currentItem = items.Where(x => x.Id == id).FirstOrDefault();
            if (currentItem == null)
            {
                return new NotFoundObjectResult($"Item Is not present with Id {id}");
            }


            items.Remove(currentItem);
            return new OkResult();
        }


    }



    public class TodoItem
    {
        public TodoItem(string name)
        {
            this.Name = name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoCreateModel
    {
        public string Name { get; set; }
    }
}
