// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftCircuits.CommandLineParser;
using System.Linq;

namespace CommandLineParser.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestArguments()
        {
            CommandLine cl = new CommandLine(true);
            cl.Parse("abc \"def ghi\" \"jkl\":abcdef");

            Assert.AreEqual(3, cl.Arguments.Count);
            Assert.AreEqual("abc", cl.Arguments[0].Argument);
            Assert.AreEqual(false, cl.Arguments[0].IsFlag);
            Assert.AreEqual(null, cl.Arguments[0].ExtendedArgument);
            Assert.AreEqual("def ghi", cl.Arguments[1].Argument);
            Assert.AreEqual(false, cl.Arguments[1].IsFlag);
            Assert.AreEqual(null, cl.Arguments[1].ExtendedArgument);
            Assert.AreEqual("jkl", cl.Arguments[2].Argument);
            Assert.AreEqual(false, cl.Arguments[2].IsFlag);
            Assert.AreEqual("abcdef", cl.Arguments[2].ExtendedArgument);

            Assert.AreEqual(true, cl.HasArgument("abc"));
            Assert.AreEqual(false, cl.HasFlagArgument("abc"));
            Assert.AreEqual(true, cl.HasArgument("def ghi"));
            Assert.AreEqual(false, cl.HasFlagArgument("def ghi"));
            Assert.AreEqual(true, cl.HasArgument("jkl"));
            Assert.AreEqual(false, cl.HasFlagArgument("jkl"));

            Assert.AreEqual(false, cl.HasArgument("Abc"));
            Assert.AreEqual(true, cl.HasArgument("Abc", true));

            Assert.IsNotNull(cl.GetArgument("abc"));
            Assert.IsNull(cl.GetArgument("ABC"));
            Assert.IsNotNull(cl.GetArgument("ABC", true));
            Assert.IsNull(cl.GetArgument("XYZ", true));
        }

        [TestMethod]
        public void TestFlags()
        {
            CommandLine cl = new CommandLine(true);
            cl.Parse("-a/b/c-d -e:arg/f:arg/g:arg-h:arg -i /j -k:\"a b c\"/l:\"a b c\" /m-n-o/p");

            Assert.AreEqual(16, cl.Arguments.Count);
            Assert.AreEqual("a", cl.Arguments[0].Argument);
            Assert.AreEqual(true, cl.Arguments[0].IsFlag);
            Assert.AreEqual(null, cl.Arguments[0].ExtendedArgument);
            Assert.AreEqual("b", cl.Arguments[1].Argument);
            Assert.AreEqual(true, cl.Arguments[1].IsFlag);
            Assert.AreEqual(null, cl.Arguments[1].ExtendedArgument);
            Assert.AreEqual("c", cl.Arguments[2].Argument);
            Assert.AreEqual(true, cl.Arguments[2].IsFlag);
            Assert.AreEqual(null, cl.Arguments[2].ExtendedArgument);
            Assert.AreEqual("d", cl.Arguments[3].Argument);
            Assert.AreEqual(true, cl.Arguments[3].IsFlag);
            Assert.AreEqual(null, cl.Arguments[3].ExtendedArgument);
            Assert.AreEqual("e", cl.Arguments[4].Argument);
            Assert.AreEqual(true, cl.Arguments[4].IsFlag);
            Assert.AreEqual("arg", cl.Arguments[4].ExtendedArgument);
            Assert.AreEqual("f", cl.Arguments[5].Argument);
            Assert.AreEqual(true, cl.Arguments[5].IsFlag);
            Assert.AreEqual("arg", cl.Arguments[5].ExtendedArgument);
            Assert.AreEqual("g", cl.Arguments[6].Argument);
            Assert.AreEqual(true, cl.Arguments[6].IsFlag);
            Assert.AreEqual("arg", cl.Arguments[6].ExtendedArgument);
            Assert.AreEqual("h", cl.Arguments[7].Argument);
            Assert.AreEqual(true, cl.Arguments[7].IsFlag);
            Assert.AreEqual("arg", cl.Arguments[7].ExtendedArgument);
            Assert.AreEqual("i", cl.Arguments[8].Argument);
            Assert.AreEqual(true, cl.Arguments[8].IsFlag);
            Assert.AreEqual(null, cl.Arguments[8].ExtendedArgument);
            Assert.AreEqual("j", cl.Arguments[9].Argument);
            Assert.AreEqual(true, cl.Arguments[9].IsFlag);
            Assert.AreEqual(null, cl.Arguments[9].ExtendedArgument);
            Assert.AreEqual("k", cl.Arguments[10].Argument);
            Assert.AreEqual(true, cl.Arguments[10].IsFlag);
            Assert.AreEqual("a b c", cl.Arguments[10].ExtendedArgument);
            Assert.AreEqual("l", cl.Arguments[11].Argument);
            Assert.AreEqual(true, cl.Arguments[11].IsFlag);
            Assert.AreEqual("a b c", cl.Arguments[11].ExtendedArgument);
            Assert.AreEqual("m", cl.Arguments[12].Argument);
            Assert.AreEqual(true, cl.Arguments[12].IsFlag);
            Assert.AreEqual(null, cl.Arguments[12].ExtendedArgument);
            Assert.AreEqual("n", cl.Arguments[13].Argument);
            Assert.AreEqual(true, cl.Arguments[13].IsFlag);
            Assert.AreEqual(null, cl.Arguments[13].ExtendedArgument);
            Assert.AreEqual("o", cl.Arguments[14].Argument);
            Assert.AreEqual(true, cl.Arguments[14].IsFlag);
            Assert.AreEqual(null, cl.Arguments[14].ExtendedArgument);
            Assert.AreEqual("p", cl.Arguments[15].Argument);
            Assert.AreEqual(true, cl.Arguments[15].IsFlag);
            Assert.AreEqual(null, cl.Arguments[15].ExtendedArgument);

            Assert.AreEqual(true, cl.HasFlagArgument("a"));
            Assert.AreEqual(false, cl.HasFlagArgument("A"));
            Assert.AreEqual(true, cl.HasFlagArgument("A", true));
            Assert.AreEqual(true, cl.HasFlagArgument("b"));
            Assert.AreEqual(true, cl.HasFlagArgument("c"));
            Assert.AreEqual(true, cl.HasFlagArgument("d"));
            Assert.AreEqual(true, cl.HasFlagArgument("e"));
            Assert.AreEqual(true, cl.HasFlagArgument("f"));
            Assert.AreEqual(true, cl.HasFlagArgument("g"));
            Assert.AreEqual(true, cl.HasFlagArgument("h"));
            Assert.AreEqual(true, cl.HasFlagArgument("i"));
            Assert.AreEqual(true, cl.HasFlagArgument("j"));
            Assert.AreEqual(true, cl.HasFlagArgument("k"));
            Assert.AreEqual(true, cl.HasFlagArgument("l"));
            Assert.AreEqual(true, cl.HasFlagArgument("m"));
            Assert.AreEqual(true, cl.HasFlagArgument("n"));
            Assert.AreEqual(true, cl.HasFlagArgument("o"));
            Assert.AreEqual(true, cl.HasFlagArgument("p"));
            Assert.AreEqual(false, cl.HasFlagArgument("q"));
            Assert.AreEqual(false, cl.HasFlagArgument("r"));

            Assert.AreEqual(false, cl.HasArgument("a"));
            Assert.AreEqual(false, cl.HasArgument("b"));
            Assert.AreEqual(false, cl.HasArgument("c"));
            Assert.AreEqual(false, cl.HasArgument("d"));
            Assert.AreEqual(false, cl.HasArgument("e"));
            Assert.AreEqual(false, cl.HasArgument("f"));
            Assert.AreEqual(false, cl.HasArgument("g"));
            Assert.AreEqual(false, cl.HasArgument("h"));
            Assert.AreEqual(false, cl.HasArgument("i"));
            Assert.AreEqual(false, cl.HasArgument("j"));
            Assert.AreEqual(false, cl.HasArgument("k"));
            Assert.AreEqual(false, cl.HasArgument("l"));
            Assert.AreEqual(false, cl.HasArgument("m"));
            Assert.AreEqual(false, cl.HasArgument("n"));
            Assert.AreEqual(false, cl.HasArgument("o"));
            Assert.AreEqual(false, cl.HasArgument("p"));
            Assert.AreEqual(false, cl.HasArgument("q"));
            Assert.AreEqual(false, cl.HasArgument("r"));

            CollectionAssert.AreEqual(new string[] { }, cl.GetArguments().ToArray());
            CollectionAssert.AreEqual(new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p" },
                cl.GetFlagArguments().Select(a => a.Argument).ToArray());
        }

        [TestMethod]
        public void TestExtendedArguments()
        {
            CommandLine cl = new CommandLine(false);
            cl.Parse("-mode:read -mode2:write");
            Assert.AreEqual(2, cl.Arguments.Count);
            Assert.AreEqual(true, cl.Arguments[0].IsFlag);
            Assert.AreEqual("mode:read", cl.Arguments[0].Argument);
            Assert.AreEqual(null, cl.Arguments[0].ExtendedArgument);
            Assert.AreEqual(true, cl.Arguments[1].IsFlag);
            Assert.AreEqual("mode2:write", cl.Arguments[1].Argument);
            Assert.AreEqual(null, cl.Arguments[1].ExtendedArgument);
            CollectionAssert.AreEqual(new string[] { },
                cl.GetArguments().Select(a => a.Argument).ToArray());
            CollectionAssert.AreEqual(new string[] { "mode:read", "mode2:write" },
                cl.GetFlagArguments().Select(a => a.Argument).ToArray());

            cl = new CommandLine(true);
            cl.Parse("  -mode:read -mode2:write  ");
            Assert.AreEqual(2, cl.Arguments.Count);
            Assert.AreEqual(true, cl.Arguments[0].IsFlag);
            Assert.AreEqual("mode", cl.Arguments[0].Argument);
            Assert.AreEqual("read", cl.Arguments[0].ExtendedArgument);
            Assert.AreEqual(true, cl.Arguments[1].IsFlag);
            Assert.AreEqual("mode2", cl.Arguments[1].Argument);
            Assert.AreEqual("write", cl.Arguments[1].ExtendedArgument);
            CollectionAssert.AreEqual(new string[] { },
                cl.GetArguments().Select(a => a.Argument).ToArray());
            CollectionAssert.AreEqual(new string[] { "mode", "mode2" },
                cl.GetFlagArguments().Select(a => a.Argument).ToArray());
        }
    }
}
