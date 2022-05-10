﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noodle_Assignment_API.Services;

namespace Noodle_Assignment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        private readonly IHelloWorldService _helloWorldService;
        public HelloWorldController(IHelloWorldService helloWorldService)
        {
            _helloWorldService = helloWorldService; 
        }

        [HttpGet]
         public string HelloWorld()
        {
            return _helloWorldService.HelloWorld();
        }
    }
}
