using LaboratoryOnlineJournal.Cryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LOJ_test
{
    [TestClass]
    public class EncodingTest
    {
        [TestMethod]
        public void OldTest()
        {
            var encoder = new OldEncryption();

            var bytes = new byte[] { 1, 3, 57, 6, 45, 23, 2, 6, 37, 65, 87, 4, 56, 4, 35, 56, 8, 255 };

            var xml = "<RSAKeyValue><Modulus>vSHFY3X54bKGI0lc8djFZhqOH/a86oORTH6ulnUBnvSlpj65CtOPTLpwOkmfnSlLu64qyazFMg7IrFuW3fkiSsiiTCVZInIh50De+V/dadYwjc96KDDb9qn9wb3JMikSBCWNQwqTyEgT1vCrUuHebhx8rjc6laGkRpFnzzrUJbtktYcDqYYU/xYUUmYb19HhtQV0GrQtudZ54oEHeGz+tZtopyqyVPm0ZEHUh3xf6THRWg9Jd1hnBAhyjlCgaTIwA9D25D7hJA74fArVolIRIPG0JMV+plBC9sfNRmGfPoT082l3AKIVWbO6/hb+3vxN1c+SoJpc9JD90JysC/Vkkw==</Modulus><Exponent>AQAB</Exponent><P>5Og3JEWla4H35QOq6Djt8eh1lpGVLZmEYTmrKIZoLXqOJR6NvMMlwpjDmnvZynLd8qIhGZBn7rsXBGHxNvfzwCxXcxzCmD2iDL/XK8NoA/tlZ4r70UvFyVZDjsr6XNL0gYegkTLklobW1ZFD0Typh9qboNYWydlACUzZWKvmnVM=</P><Q>04RipKn84c1yt0XfaKbe2Eg4bjx+g8ilv6vPaw5XlQlo27WCtHsD7MIrbzPLe5Be78ixdcEoKFIPucH2huSjtxxH/Y8oJ1b0VYgdk8445SKTCG6WcwJPYxmVY8Twz7hTDLoZzTSnPvSynM6PsBktuC+zPqrVU068gehPkRnR88E=</Q><DP>HFDpeVAwPVNPggHpI17feFxEJ4MMzB5AdPJ4TMQLoQyXBtp3uBD/28mf8L0/XL7G29vYclwdrzdving/KYiUm4IgszmsjL6bDC6zBFPgyxVPHvbfXa2c4uIL618Kh28FFfzcDPoZstEtRC/7DqgNZKPTOpshKIj6VewuurxRA8c=</DP><DQ>pd8RhFQSDfmRVowi8Oy7oRyxtDEYfbwhzzerBydOI4AnjPTAtUwq/cYfTatujU3gRWY7VD7PgR8pWeDztUEj6frxsbRMJt2X6mM93qVAFOCSMXCX50UOgIaVkpHkzuCbsEVY6oW6CjLWxwVtxQlZwzEU/bX2aMg8KBvIGeAHt4E=</DQ><InverseQ>sAnYc66Ze4RmwOdjqMUs2Z0NeALuI7b15ZroZ/m5qqOoaasNKsKphX3PM9GLWdw9NQd4r6AnCTWCz+2CdEftX6f7psFwrIVEJM7LjLJtSNzKVMhlz4edlxmRoM43rXxuVax++KWYrBPG0jw5zbH6e9NE+K6j125MpFnYZLvpssI=</InverseQ><D>X3zta4nk306C6s3fXztSbnp5xymLt9s1QKm0+8GXT+m0uHpyckTd1J9MiiEhtPdkhR0p/Sh9ZwiPyHV1dhySc69YQZmZpwp4k4jtCnqcDxNU8EQQKLqCU8b/lxF6wxh5QB61c2OjuTqqyZo45V+kLXO0f0DjEyjJB9fh0X6iHWng0fDz9WZLKyYnNFHD5/SqKub6CnX8UUdypkzbtdeAzI902+AlfJ8J6+sg/lhbev5uo9rFIM5BkSPjMtZBizuSiPA4E7hmOUlojGOaQFMxIE85W5ZBnso2VWnCgtzKjDQz5QHCKYhSNpiikUUB3HwYuuX/ib8v9tn8AwAu1cdgAQ==</D></RSAKeyValue>";

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);

            var bytesEncoded = encoder.Encode(bytes, rsa);

            var bytesDecoded = encoder.Decode(bytesEncoded, (id) => rsa);

            CollectionAssert.AreEqual(bytesDecoded, bytesDecoded);
        }

        [TestMethod]
        public void BlankTest()
        {
            var encoder = new BlankEncryption();

            var bytes = new byte[] { 1, 3, 57, 6, 45, 23, 2, 6, 37, 65, 87, 4, 56, 4, 35, 56, 8, 255 };

            var bytesEncoded = encoder.Encode(bytes, null);

            var bytesDecoded = encoder.Decode(bytesEncoded, null);

            CollectionAssert.AreEqual(bytesDecoded, bytesDecoded);
        }
    }
}