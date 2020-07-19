using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobSDK
{

    public class Functions
    {
        public static void ProcessQueueMessage([QueueTrigger("queue")] Order order, ILogger logger)
        {
            Console.WriteLine($"Received The Message from {order.ResturantName}");
        }

        public static void TimerTrigger([TimerTrigger("0 */2 * * * *", RunOnStartup = true)] TimerInfo info, ILogger logger)
        {
            Console.WriteLine($"Received The Message from {DateTime.Now.ToString()}");
        }
    }

    public class Order
    {
        public string OrderId { get; set; }
        public string ResturantName { get; set; }
        public string Amount { get; set; }
    }

}
