using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lektion6.Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Lektion6.Models.Entities
{
    public class News : IEntity
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageName { get; set; }
        public Guid CreatedByID { get; set; }
        public DateTime CreateDate { get; set; }

        public string ShortMessage
        {
            get
            {
                return Body.Length > 25 ? string.Format("{0}...", Body.Substring(0, 25)) : Body;
            }
        }
    }
}