﻿namespace ERP.API.Models.ClientContacts
{
    public class ClientContactPostVM
    {
        public int Id { get; set; }  
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        //public int ClientId { get; set; }
    }
}
