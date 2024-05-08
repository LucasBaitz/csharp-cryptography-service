﻿namespace Cryptography.Services.Interfaces
{
    public interface ICryptographyService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
