using System;
using System.Collections.Generic;


namespace logreg.Models
{
    public class Users
    {
        public int usersid {get;set;}
        public string name {get;set;}
        public string alias {get;set;}
        public string email {get;set;}
        public string password {get;set;}
        public DateTime created_at {get;set;}
        public DateTime updated_at {get;set;}
        public List<Posts> Posts {get;set;}
        public List<Likes> Likes {get;set;}

        public Users()
        {
            List<Posts> Posts = new List<Posts>();
            List<Likes> Likes =new List<Likes>();
        }
    }
}