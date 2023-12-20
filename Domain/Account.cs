﻿namespace Core.Domain
{
    public class Account
    {
        public ushort AccountId { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
    }
}