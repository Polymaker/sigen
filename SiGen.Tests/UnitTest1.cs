using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiGen.Measuring;

namespace SiGen.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAreaSquare()
        {
            var side = Measure.Inches(0.54);
            var area = side.Squared();
            var areaCM1 = side[UnitOfMeasure.Cm] * side[UnitOfMeasure.Cm];
            var areaIN1 = side[UnitOfMeasure.In] * side[UnitOfMeasure.In];
            var areaCM2 = area[UnitOfMeasure.Cm];
            var areaIN2 = area[UnitOfMeasure.In];
            Assert.AreEqual(areaCM1, areaCM2);
            Assert.AreEqual(areaIN1, areaIN2);
        }

        [TestMethod]
        public void TestAreaCircle()
        {
            var radius = Measure.Inches(0.27);
            
            var areaCM1 = (radius[UnitOfMeasure.Cm] * radius[UnitOfMeasure.Cm]) * Math.PI;
            var areaIN1 = (radius[UnitOfMeasure.In] * radius[UnitOfMeasure.In]) * Math.PI;

            var area = radius.Squared() * Math.PI;

            var areaCM2 = area[UnitOfMeasure.Cm];
            var areaIN2 = area[UnitOfMeasure.In];

            Assert.AreEqual(areaCM1, areaCM2, 0.00001);
            Assert.AreEqual(areaIN1, areaIN2, 0.00001);
        }
    }
}
