using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LionHead.WebAPI.Controllers
{
    public class EncryptChestController : ApiController
    {
        [HttpGet]
        public string Get(string lootTable)
        {
            return  HttpUtility.UrlEncodeUnicode(GameChest.Encrypt(lootTable));
        }
    }
}
