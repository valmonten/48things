using System;
using System.Collections.Generic;

namespace logreg.Models
{
    public class Posts
    {
        public int postsid {get;set;}
        public string message {get;set;}
        public int usersid {get;set;}
        public DateTime created_at {get;set;}
        public DateTime updated_at {get;set;}
        public List<Likes> Likes {get;set;}

        public Posts()
        {
            List<Likes> Likes = new List<Likes>();
        }

    }
}