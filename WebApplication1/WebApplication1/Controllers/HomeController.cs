using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library.Model;
using WebApplication1.Models;
using Library.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IOptions<AppSettingsModel> _setting;
        private readonly IOptions<ApplicationConfiguration> _ApplicationConfiguration;
        public HomeController(IOptions<AppSettingsModel> setting, IOptions<ApplicationConfiguration> ApplicationConfiguration)
        {
            _setting = setting;
            _ApplicationConfiguration = ApplicationConfiguration;
        }
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
        public SearchInput GetA([FromQuery]SearchInput model)
        {
            //逻辑暂空，这里主要看生成的输入输出  
            try
            {
                string A = _setting.Value.WeixinAppID;
                string AC = _setting.Value.WeixinToken;
                string az = _ApplicationConfiguration.Value.AttachExtension;
                //string a = AppConfigurtaionServices.Configuration.GetConnectionString("CxyOrder");
                string c = AppConfigurtaionServices.Configuration["ApplicationConfiguration:AttachExtension"];
            }
            catch (Exception ex)
            {

                throw;
            }

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetProductList(int pageIndex,int pageSize, string callback)
        {           
            var sql = string.Format(@"select top {0} * from(select  ROW_NUMBER() OVER (ORDER BY pid desc) AS RowNumber,* from SmartPig_products)t
where t.RowNumber >{1}", pageSize, (pageIndex - 1) * pageSize);
            SqlParameter[] parm = {

            };
            DataTable dt = SqlHelper.GetTable(CommandType.Text, sql, parm);
            //DataTable dt = dtConnection[0];
            return callback+"(" + JsonConvert.SerializeObject(dt) + ")";
        }
    }
}