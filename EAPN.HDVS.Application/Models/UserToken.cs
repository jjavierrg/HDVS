using System;
using System.Collections.Generic;
using System.Text;

namespace EAPN.HDVS.Application.Models
{
    public class UserToken
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
