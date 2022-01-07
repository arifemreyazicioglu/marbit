using System.Collections.Generic;

namespace TradeAbstractions.Models.Engine
{
    public class EmailSettings
    {
        /// <summary> Geliştirme aşamasında false'ta kalsın </summary>
        public bool Active { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        /// <summary> Örn: smtp.gmail.com </summary>
        public string Host { get; set; }
        /// <summary> Örn: 587 </summary>
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        /// <summary> Email olarak yazılacak </summary>
        public string FromEmail { get; set; }
        /// <summary> Opsiyonel </summary>
        public string FromName { get; set; }
        public List<string> ToEmails { get; set; }
    }
}
