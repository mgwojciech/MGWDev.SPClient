using MGWDev.SPClient.Tests.Model;
using MGWDev.SPClient.Utilities.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Tests.Utilities.OData
{
    [TestClass]
    public class SelectQueryMapperTests
    {
        [TestMethod]
        public void GivenAType_Should_GenerateSelectWithExpand()
        {
            var result = (new SelectQueryMapper()).MapToSelectQuery<InformationMessage>();

            Assert.AreEqual("ValoMessageDescription,ValoMessageStartDate,ValoMessageEndDate,Id,Title,Author/Id,Author/Name,Author/Title,Author/EMail", result.SelectQuery);
            Assert.AreEqual("Author", result.ExpandQuery);
        }
    }
}
