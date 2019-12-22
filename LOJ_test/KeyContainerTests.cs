using LaboratoryOnlineJournal.Synch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOJ_test
{
    [TestClass]
    public class KeyContainerTests
    {
        private static KeysContainer _keysContainer = new KeysContainer(Encoding.UTF32);

        private uint _id = 0;
        private string _encodeKey = "test 1";
        private string _decodeKey = "test 2";

        /// <summary>
        /// Простой тест добавления
        /// </summary>
        [TestMethod]
        public void TestAdd()
        {
            _keysContainer.Add(_id, _encodeKey, _decodeKey);

            Assert.AreEqual(1, _keysContainer.Count);
        }

        /// <summary>
        /// Тест проверки полученного значения
        /// </summary>
        [TestMethod]
        public void TestGetEncodeKey()
        {
            var encodeKey = _keysContainer.GetEncodeKey(_id);

            Assert.AreEqual(_encodeKey, encodeKey);
        }

        /// <summary>
        /// Тест проверки полученного значения
        /// </summary>
        [TestMethod]
        public void TestGetDecodeKey()
        {
            var decodeKey = _keysContainer.GetDecodeKey(_id);

            Assert.AreEqual(_decodeKey, decodeKey);
        }

        [TestMethod]
        public void TestFalseGetDecodeKey()
        {
            var decodeKey = _keysContainer.GetDecodeKey(50);

            Assert.IsTrue(String.IsNullOrEmpty(decodeKey));
        }

        [TestMethod]
        public void TestFalseGetEncodeKey()
        {
            var encodeKey = _keysContainer.GetEncodeKey(50);

            Assert.IsTrue(String.IsNullOrEmpty(encodeKey));
        }
    }
}