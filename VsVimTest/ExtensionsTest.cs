﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VsVim;
using System.Windows.Input;
using Microsoft.VisualStudio.Utilities;
using System.Windows;

namespace VsVimTest
{
    [TestClass]
    public class ExtensionsTest
    {
        #region KeyBindings

        [TestMethod, Description("Bindings as an array")]
        public void GetKeyBindings1()
        {
            var com = new Mock<EnvDTE.Command>();
            com.Setup(x => x.Bindings).Returns(new object[] { "::f" });
            com.Setup(x => x.Name).Returns("name");
            var list = Extensions.GetKeyBindings(com.Object).ToList();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(Key.F, list[0].KeyBinding.FirstKeyInput.Key);
            Assert.AreEqual("name", list[0].Name);
        }

        [TestMethod]
        public void GetKeyBindings2()
        {
            var com = new Mock<EnvDTE.Command>();
            com.Setup(x => x.Bindings).Returns(new object[] { "foo::f", "bar::b" });
            com.Setup(x => x.Name).Returns("name");
            var list = Extensions.GetKeyBindings(com.Object).ToList();
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(Key.F, list[0].KeyBinding.FirstKeyInput.Key);
            Assert.AreEqual("foo", list[0].KeyBinding.Scope);
            Assert.AreEqual(Key.B, list[1].KeyBinding.FirstKeyInput.Key);
            Assert.AreEqual("bar", list[1].KeyBinding.Scope);
        }

        [TestMethod, Description("Bindings as a string which is what the documentation indicates it should be")]
        public void GetKeyBindings3()
        {
            var com = new Mock<EnvDTE.Command>();
            com.Setup(x => x.Bindings).Returns("::f");
            com.Setup(x => x.Name).Returns("name");
            var list = Extensions.GetKeyBindings(com.Object).ToList();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(Key.F, list[0].KeyBinding.FirstKeyInput.Key);
            Assert.AreEqual(String.Empty, list[0].KeyBinding.Scope);
        }

        [TestMethod, Description("A bad key binding should just return as an empty result set")]
        public void GetKeyBindings4()
        {
            var com = new Mock<EnvDTE.Command>();
            com.Setup(x => x.Bindings).Returns(new object[] { "::notavalidkey" });
            com.Setup(x => x.Name).Returns("name");
            var e = Extensions.GetKeyBindings(com.Object).ToList();
            Assert.AreEqual(0, e.Count);
        }

        #endregion

        #region PropertyCollection

        [TestMethod]
        public void AddTypedProperty1()
        {
            var col = new PropertyCollection();
            col.AddTypedProperty("foo");
            Assert.AreEqual(1, col.PropertyList.Count);
            Assert.IsTrue(col.ContainsProperty(typeof(string)));
        }

        [TestMethod]
        public void TryGetTypedProperty1()
        {
            var col = new PropertyCollection();
            col.AddTypedProperty("foo");
            var opt = col.TryGetTypedProperty<string>();
            Assert.IsTrue(opt.IsSome());
            Assert.AreEqual("foo", opt.Value);
        }

        [TestMethod]
        public void TryGetTypedProperty2()
        {
            var col = new PropertyCollection();
            var opt = col.TryGetTypedProperty<string>();
            Assert.IsFalse(opt.IsSome());
        }

        #endregion
    }
}