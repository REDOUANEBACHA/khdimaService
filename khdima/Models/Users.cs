﻿using System.Text.Json.Serialization;

namespace khdima.Models
{
  

    public class Users
    {
        public int id { get; set; }
        public byte[] img { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public DateTime birthday_date { get; set; }
        public string sexe { get; set; } 
        public string email { get; set; }
        [JsonIgnore] public string password { get; set; }
        [JsonIgnore] public string? tmp_password { get; set; }
        [JsonIgnore] public DateTime? password_send_date { get; set; }
        public DateTime? last_access { get; set; }
        public DateTime? first_access { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string newsletter_status { get; set; }
    }
}

