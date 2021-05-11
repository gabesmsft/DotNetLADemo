using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevBootCampNetCoreLogging.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevBootCampNetCoreLogging.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> log;

        private readonly IConfiguration _configuration;

        //get an ILogger via dependency injection

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            log = logger;

            _configuration = configuration;
        }
        public IActionResult Index()
        {
            log.LogInformation("Entered Index method");

            Message message = new Message();
            message.MessageId = 123;
            message.MessageBody = "Hello world message";

            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
            };

            string jsonmessage = JsonSerializer.Serialize(message, options);

            log.LogInformation("MessageLabel1: " + jsonmessage);

            return View();
        }

        [HttpGet("cpu")]
        public IActionResult CPU()
        {
            log.LogInformation("Entering high CPU method");
            long num = 500000;
            bool isPrime = true;
            for (int i = 0; i <= num; i++)
            {
                for (int j = 2; j <= num; j++)
                {
                    if (i != j && i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    Console.WriteLine("Prime:" + i);
                }
                isPrime = true;
            }
            log.LogInformation("Survived high CPU method");

            return View();
        }

        [HttpGet("memory")]
        public IActionResult Memory()
        {
            log.LogInformation("Entering high memory method");
            long num = 5000;
            string[] stringArray = new string[num];

            stringArray[0] = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                     + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                     + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                     + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                     + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                     + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                    + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            for (int i = 1; i < num; i++)
            {
                stringArray[i] = stringArray[i - 1] + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            }
            log.LogInformation("Survived high memory method");

            return View();
        }

        [HttpGet("slow")]
        public IActionResult Slow()
        {
            log.LogInformation("Entering slow method");

            Thread.Sleep(120000);

            log.LogInformation("Survived slow method");

            return View();
        }

        [HttpGet("throw")]
        public IActionResult Throw()
        {
            log.LogInformation("Entering throw method");

            int i = 0;
            int j = 1;
            int k = j / i;
            log.LogInformation("bad denominator" + k);

            log.LogInformation("Survived throw method");

            return View();
        }

        [HttpGet("catch")]
        public IActionResult Catch()
        {
            log.LogInformation("Entering catch method");

            int i = 0;
            int j = 1;
            try
            {
                int k = j / i;
            }
            catch (Exception ex)
            {
                CaughtException caughtException = new CaughtException();
                caughtException.Method = "Catch";
                caughtException.ExceptionMessage = ex.ToString();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = false,
                };

                string jsonmessage = JsonSerializer.Serialize(caughtException, options);

                log.LogInformation("CaughtExceptionLabel: " + jsonmessage);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class Message
    {
        public int MessageId { get; set; }
        public string MessageBody { get; set; }
    }

    public class CaughtException
    {
        public string Method { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
