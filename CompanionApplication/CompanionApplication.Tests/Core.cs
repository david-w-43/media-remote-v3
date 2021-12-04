using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CompanionApplication.Core;
using System.Collections.Generic;

namespace CompanionApplication.Tests
{
    [TestClass]
    public class CommandHandlerTests
    {
        [TestMethod]
        public void ProcessReceived()
        {
            TestMediaInterface testMediaInterface = new TestMediaInterface();
            TestRemoteInterface testRemoteInterface = new TestRemoteInterface();

            // Instantiate command handler with test interfaces
            CommandHandler _commandHandler = new CommandHandler(testMediaInterface, testRemoteInterface);

            Queue<string> commands = new Queue<string>();



            _commandHandler.ProcessReceived(commands);


        }
    }
}
