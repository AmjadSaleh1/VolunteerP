﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerP.ServerApi.Models
{
    public class User
    {

        public ObjectId Id { get; set; } // MongoDB uses ObjectId for unique identifiers
        public string Name { get; set; }
        public string UserName {  get; set; }
        public string ImageUrl {  get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public bool IsLocked { get; set; }
        public bool IsAdmin {  get; set; }
    }
}
