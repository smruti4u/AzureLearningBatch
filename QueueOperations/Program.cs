using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QueueOperations
{
    class Program
    {
        static async Task Main(string[] args)
        {
            QueueClient client = new QueueClient("UseDevelopmentStorage=true", "vsqueue");

            if(await client.ExistsAsync())
            {
                await client.SendMessageAsync("vstext");

                while(true)
                {
                    
                    QueueMessage[] messages =  client.ReceiveMessages();
                    if (messages.Count() > 0)
                    {
                        foreach (var message in messages)
                        {
                            var result = await ProcessMessage(message);
                            if (result)
                            {
                                await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                            }
                            else
                            {
                                await client.SendMessageAsync(message.MessageText);
                            }
                        }
                    }
                }
            }
        }

        private static Task<bool> ProcessMessage(QueueMessage messages)
        {
            try
            {
                Thread.Sleep(1000);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
            
        }
    }
}
