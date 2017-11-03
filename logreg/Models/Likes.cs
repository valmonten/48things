using System;

namespace logreg.Models
{
    public class Likes
    {
        public int likesid {get;set;}
        public int likingit {get;set;}
        public int usersid {get;set;}
        public Users Users {get;set;}
        public int postsid {get;set;}
        public Posts Posts {get;set;}
        public DateTime created_at {get;set;}
        public DateTime updated_at {get;set;}

    }
}