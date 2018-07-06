using System;
using System.Collections.Generic;
using System.IO;
using JWT;
using JWT.Serializers;
using Newtonsoft.Json;

namespace src
{
    static class JwtHelper
    {
        public static string DecodeToJson(string token)
        {
            try
            {
                var serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                var validator = new JwtValidator(serializer, provider);
                var urlEncoder = new JwtBase64UrlEncoder();
                var decoder = new JwtDecoder(serializer, validator, urlEncoder);
                var result = decoder.Decode(token);
                // var json = decoder.Decode(token, secret, verify: true);
                return PrettyStringHelper.JsonPrettify(result);
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }
            return string.Empty;
        }
    }
}

