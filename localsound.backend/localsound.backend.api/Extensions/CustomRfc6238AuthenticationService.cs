using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace localsound.backend.api.Extensions
{
    public static class CustomRfc6238AuthenticationService
    {
        private static readonly TimeSpan _timestep = TimeSpan.FromMinutes(3);
        private static readonly Encoding _encoding = new UTF8Encoding(false, true);

        internal static int ComputeTotp(
        HashAlgorithm hashAlgorithm,
            ulong timestepNumber,
            byte[]? modifierBytes)
        {
            // # of 0's = length of pin
            const int Mod = 1000000;

            // See https://tools.ietf.org/html/rfc4226
            // We can add an optional modifier
            var timestepAsBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
            var hash = hashAlgorithm.ComputeHash(ApplyModifier(timestepAsBytes, modifierBytes));

            // Generate DT string
            var offset = hash[hash.Length - 1] & 0xf;
            Debug.Assert(offset + 4 < hash.Length);
            var binaryCode = (hash[offset] & 0x7f) << 24
                                | (hash[offset + 1] & 0xff) << 16
                                | (hash[offset + 2] & 0xff) << 8
                                | (hash[offset + 3] & 0xff);

            return binaryCode % Mod;
        }

        private static byte[] ApplyModifier(Span<byte> input, byte[] modifierBytes)
        {
            var combined = new byte[checked(input.Length + modifierBytes.Length)];
            input.CopyTo(combined);
            Buffer.BlockCopy(modifierBytes, 0, combined, input.Length, modifierBytes.Length);

            return combined;
        }

        // More info: https://tools.ietf.org/html/rfc6238#section-4
        private static ulong GetCurrentTimeStepNumber(TimeSpan timeStep)
        {
            var delta = DateTimeOffset.UtcNow - DateTimeOffset.UnixEpoch;

            return (ulong)(delta.Ticks / timeStep.Ticks);
        }

        public static int GenerateCode(byte[] securityToken, string? modifier = null, TimeSpan? timeStep = null)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            // Allow a variance of no greater than time step in either direction
            var currentTimeStep = GetCurrentTimeStepNumber(timeStep ?? _timestep);

            var modifierBytes = modifier is not null ? _encoding.GetBytes(modifier) : null;

            using (var hashAlgorithm = new HMACSHA1(securityToken))
            {
                return ComputeTotp(hashAlgorithm, currentTimeStep, modifierBytes);
            }
        }

        public static bool ValidateCode(byte[] securityToken, int code, string? modifier = null, TimeSpan? timeStep = null)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            // Allow a variance of no greater than time step in either direction
            var currentTimeStep = GetCurrentTimeStepNumber(timeStep ?? _timestep);

            using (var hashAlgorithm = new HMACSHA1(securityToken))
            {
                var modifierBytes = modifier is not null ? _encoding.GetBytes(modifier) : null;
                for (var i = -2; i <= 2; i++)
                {
                    var computedTotp = ComputeTotp(hashAlgorithm, (ulong)((long)currentTimeStep - i), modifierBytes);
                    if (computedTotp == code)
                    {
                        return true;
                    }
                }
            }

            // No match
            return false;
        }
    }
}
