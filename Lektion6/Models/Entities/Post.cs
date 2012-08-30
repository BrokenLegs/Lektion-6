using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lektion6.Models.Entities.Abstract;

namespace Lektion6.Models.Entities
{
    public class Post : IEntity
    {
        public Post() { }
        public Post(Guid createdByID, string title, string body)
        {
            ID = Guid.NewGuid();
            CreatedByID = createdByID;
            Title = title;
            Body = body;
            CreateDate = DateTime.UtcNow;
        }
        public Guid ID { get; set; }

        private User _CreatedBy { get; set; }
        public Guid CreatedByID { get; set; }
        public User CreatedBy { get { return _CreatedBy; } }

        public DateTime CreateDate { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<PostTags> Tags { get; set; }

        public string FormattedTagList
        {
            get
            {
                if (Tags.Count == 0)
                    return "";

                string tagListString = "";
                foreach (var tag in Tags)
                {
                    if (!string.IsNullOrEmpty(tagListString))
                        tagListString += ", ";
                    tagListString += tag;
                }

                return tagListString;
            }
        }

        public string ToString(bool ShortFormat = true)
        {
            string postString = string.Format("\tTitle: '{0}' - By: '{1}' - Posted: '{2:d}'", Title, CreatedBy != null ? CreatedBy.UserName : "?", CreateDate);
            if (!ShortFormat)
                postString += string.Format("\n\t\t Message: {0}\n\t\t Tags: {1}", Body, FormattedTagList);

            return postString;
        }

        /*
         * OBS! Denna konstruktion för att ladda relaterad data är varken en bra eller vanlig konstruktion.
         * 
         * Vi kommer se hur detta kan skötas i verkligheten senare i kursen
         * 
         */
        public void LoadUser(List<User> users)
        {
            _CreatedBy = users.Where(u => u.ID == CreatedByID).FirstOrDefault();
        }

        public enum PostTags
        {
            Interesting,
            Funny,
            Troll,
            JawDropping,
            Useful
        }
    }
}
