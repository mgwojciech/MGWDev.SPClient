using MGWDev.SPClient.Tests.Model;
using MGWDev.SPClient.Utilities.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Tests.Utilities.OData
{
    [TestClass]
    public class FilterQueryMapperTests
    {
        FilterQueryMapper mapper = new FilterQueryMapper();
        [TestMethod]
        public void Given_PrimitiveExpression_Should_BuildFilterQuery()
        {
            Expression<Func<InformationMessage, bool>> expression = i => i.Title == "Test title";
            var filter = mapper.BuildFilterQuery(expression);

            Assert.AreEqual("(Title eq 'Test%20title')", filter);
        }
        [TestMethod]
        public void Given_PrimitiveExpressionWithDate_Should_BuildFilterQuery()
        {
            DateTime date = new DateTime(2023, 1, 1);
            Expression<Func<InformationMessage, bool>> expression = message => message.EndDate <= date;
            var filter = mapper.BuildFilterQuery(expression);

            Assert.AreEqual("(ValoMessageEndDate le '2023-01-01T00:00:00')", filter);
        }
        [TestMethod]
        public void Given_PrimitiveExpressionWithDateAndString_Should_BuildFilterQuery()
        {
            var date = new DateTime(2023, 1, 1);
            Expression<Func<InformationMessage, bool>> expression = message => message.EndDate <= date || message.Title == "Test title";
            var filter = mapper.BuildFilterQuery(expression);

            Assert.AreEqual("((ValoMessageEndDate le '2023-01-01T00:00:00') or (Title eq 'Test%20title'))", filter);
        }
        [TestMethod]
        public void Given_ExpressionOnComplexObject_Should_BuildFilterQuery()
        {
            Expression<Func<InformationMessage, bool>> expression = i => i.Author.Id == 1;
            var filter = mapper.BuildFilterQuery(expression);

            Assert.AreEqual("(Author/Id eq 1)", filter);
        }
    }
}
