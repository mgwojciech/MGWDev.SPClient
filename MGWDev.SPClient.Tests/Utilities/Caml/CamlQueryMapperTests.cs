using MGWDev.SPClient.Tests.Model;
using MGWDev.SPClient.Utilities.Caml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Tests.Utilities.Caml
{
    [TestClass]
    public class CamlQueryMapperTests
    {
        private CamlQueryMapper mapper =new CamlQueryMapper();
        [TestMethod]
        public void Given_PrimitiveElementExpression_Should_BuildCaml()
        {
            string expected = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">Test</Value></Eq></Where>";

            Expression<Func<InformationMessage, bool>> expression = i => i.Title == "Test";
            var filter = mapper.BuildFilterQuery(expression);

            Assert.AreEqual(expected, filter);
        }
        [TestMethod]
        public void Given_DateElementExpression_Should_BuildCaml()
        {
            DateTime date = new DateTime(2019, 06, 01, 12, 00, 00);
            string expected = "<Where><Eq><FieldRef Name=\"ValoMessageStartDate\" /><Value Type=\"DateTime\">2019-06-01T12:00:00</Value></Eq></Where>";

            Expression<Func<InformationMessage, bool>> expression = i => i.StartDate == date;
            var filter = mapper.BuildFilterQuery(expression);

            Assert.AreEqual(expected, filter);
        }
    }
}
