﻿

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace AzureKeyVault.Maui
{
    public class CustomSecretManager : KeyVaultSecretManager
    {

        private readonly string _prefix;

        public CustomSecretManager(string prefix)
        {
            _prefix = $"{prefix}-";
        }

        public override bool Load(SecretProperties secret)
        {
            return secret.Name.StartsWith(_prefix);
        }

        public override string GetKey(KeyVaultSecret secret)
        {
            return secret.Name.Substring(_prefix.Length).Replace("--", ConfigurationPath.KeyDelimiter);
        }
    }
}
