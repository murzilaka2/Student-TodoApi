﻿namespace TodoApi.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public string HashedPasssword { get; set; }
        public string Salt { get; set; }
    }
}
