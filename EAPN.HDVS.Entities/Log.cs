using System;

namespace EAPN.HDVS.Entities
{
    public class Log
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public DateTime Date { get; set; }
        public string Logger { get; set; }
        public string Level { get; set; }
        public int LevelOrder { get; set; }
        public string Exception { get; set; }
        public string CallSite { get; set; }
        public string Message { get; set; }
        public string Ip { get; set; }
    }
}
