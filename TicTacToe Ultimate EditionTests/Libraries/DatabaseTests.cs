using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe_Ultimate_Edition.Libraries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TicTacToe_Ultimate_Edition.Libraries.Tests
{
    [TestClass()]
    public class DatabaseTests
    {
        [TestMethod()]
        public void newUserTest()
        {
            Database TC = new Database();
            TC.newUser("Dan");
            
           // Assert.Fail();
        }
    }
}
