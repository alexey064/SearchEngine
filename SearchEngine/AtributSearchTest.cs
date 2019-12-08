using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    [TestFixture]
    class AtributSearchTest
    {
        [Test]
        public void Test1() 
        {
            SearchWithParams search= new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName","?file",false, null));
        }
        [Test]
        public void Test2()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsTrue(search.Search("EnterfileName", "*file", false, null));
        }
        [Test]
        public void Test3()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "??file", false, null));
        }
        [Test]
        public void Test4()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsTrue(search.Search("EnterfileName", "**file", false, null));
        }
        [Test]
        public void Test5()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsTrue(search.Search("EnterfileName", "?????file", false, null));
        }
        [Test]
        public void Test6()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsTrue(search.Search("EnterfileName", "?*file", false, null));
        }
        [Test]
        public void Test7()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsTrue(search.Search("EnterfileName", "*?file", false, null));
        }
        [Test]
        public void Test8()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "f?ile", false, null));
        }
        [Test]
        public void Test9()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "f*ile", false, null));
        }
        [Test]
        public void Test10()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "fi??Na", false, null));
        }
        [Test]
        public void Test11()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "fi**le", false, null));
        }
        [Test]
        public void Test12()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "fi**me", false, null));
        }
        [Test]
        public void Test13()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "E*i?N", false, null));
        }
        [Test]
        public void Test14()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "file*", false, null));
        }
        [Test]
        public void Test15()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsTrue(search.Search("EnterfileName", "*file*", false, null));
        }
        [Test]
        public void Test16()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "?file?", false, null));
        }
        [Test]
        public void Test17()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "file?*", false, null));
        }
        [Test]
        public void Test18()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("Enterfile", "*file?", false, null));
        }
        [Test]
        public void Test19()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsTrue(search.Search("EnterfileName", "?n**ter", false, null));
        }
        [Test]
        public void Test20()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "file**", false, null));
        }
        [Test]
        public void Test21()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsTrue(search.Search("EnterfileName", "**file**", false, null));
        }
        [Test]
        public void Test22()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "file*?", false, null));
        }
        [Test]
        public void Test23()
        {
            SearchWithParams search = new SearchWithParams();
            Assert.IsFalse(search.Search("EnterfileName", "*azsa*", false, null));
        }
    }
}