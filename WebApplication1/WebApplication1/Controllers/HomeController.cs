using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public List<SearchOutput> UserSearch(SearchInput input, string c) =>
            //逻辑暂空，这里主要看生成的输入输出
            null;

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public SearchInput GetA([FromQuery]SearchInput input)
        {
            //逻辑暂空，这里主要看生成的输入输出            
            return input;
        }
    }
}